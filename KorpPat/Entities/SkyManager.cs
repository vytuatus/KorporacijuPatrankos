using KorpPat;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRexGame.Entities
{
    public class SkyManager : IGameEntity, IDayNightCycle
    {

        private const float EPSILON = 0.01f;

        private const int CLOUD_DRAW_ORDER = -1;
        private const int STAR_DRAW_ORDER = -3;
        private const int MOON_DRAW_ORDER = -2;


        private const int CLOUD_MIN_POS_Y = 20;
        private const int CLOUD_MAX_POS_Y = 70;
        private const int CLOUD_MIN_DISTANCE = 160; // at least 80 pixels apart
        private const int CLOUD_MAX_DISTANCE = 300; // max 200 pixels apart

        private const int STAR_MIN_POS_Y = 10;
        private const int STAR_MAX_POS_Y = 60;
        private const int STAR_MIN_DISTANCE = 120;
        private const int STAR_MAX_DISTANCE = 380;

        private const int MOON_POS_Y = 20;

        private const int NIGHT_TIME_SCORE = 200;
        private const int NIGHT_TIME_DURATION_SCORE = 250;
        private const float TRANSITION_DURATION = 2f;

        private float _normalizedScreenColor = 1f;
        private int _previousScore;
        private int _nightTimeStartScore;
        private bool _isTransitioningToNight = false;
        private bool _isTransitioningToDay = false;

        private readonly EntityManager _entityManager;
        private readonly ScoreBoard _scoreBoard;
        private Texture2D _spriteSheet;
        private Texture2D _invertedSpriteSheet;
        private Color[] _textureData;
        private Color[] _invertedTextureData;

        private readonly TRex _trex;
        private Moon _moon;

        private double _lastCloudSpawnScore = -1;
        private int _targetCloudDistance; // that's where we will store random distance between clounds
        private int _targetStarDistance;

        private Random _random;

        // 0 would give black, 1 would give white and if all 3 RGB colors are same number - greyscale
        public Color ClearColor => new Color(_normalizedScreenColor, _normalizedScreenColor, _normalizedScreenColor);
        public int DrawOrder => int.MaxValue;

        public int NightCount { get; private set; }

        public bool IsNight => _normalizedScreenColor < 0.5f;

        private Texture2D _overlay;

        // first 0.25f represents the full distance between 0 and 1 of the OverlayTransparancy.
        // we are interested in the _normalizedColor from 0.25f until 0.5f and from 0.5f until 0.75f
        // at 0.25f of normalized color, OverlayVisibility should be 0, at 0.5f => 1, and at 0.75f => again 0
        // when we calculate 'Distance', if we have _normalized color, say, 0.3 it means it is between 0.25f and 0.5f, which also means that our OverlayVisibility should be <> 0
        // 0.3 is 0.05f more than 0.25f. So when _normalizedColor is 0.3, we are at 0.05 point of the full 0.25f distance
        // To get to our OverlayVisibility value, we now need to normalize 0.05, which is deviding it by 0.25f (the full distance). Basically when normalizeColor moves 0.01, our 
        // OverlayVisibility moves by 4x times that. 
        // check picture saved at: C:\Users\petko\Documents\Learning\Monogame\Projects\Trex
        private float OverlayVisibility => MathHelper.Clamp((0.20f - MathHelper.Distance(0.5f, _normalizedScreenColor)) / 0.3f, 0, 1);

        public SkyManager(TRex trex, Texture2D spriteSheet, Texture2D invertedSpriteSheet, EntityManager entityManager, ScoreBoard scoreBoard)
        {
            _entityManager = entityManager;
            _scoreBoard = scoreBoard;
            _random = new Random();
            _spriteSheet = spriteSheet;
            _trex = trex;
            _invertedSpriteSheet = invertedSpriteSheet;

            _textureData = new Color[_spriteSheet.Width * _spriteSheet.Height];
            _invertedTextureData = new Color[_invertedSpriteSheet.Width * _invertedSpriteSheet.Height];
            // get data from spriteSheet and store it in the _textureData
            _spriteSheet.GetData(_textureData);
            _invertedSpriteSheet.GetData(_invertedTextureData);

            _overlay = new Texture2D(spriteSheet.GraphicsDevice, 1, 1);
            Color[] overlayData = new Color[] { Color.Gray };
            _overlay.SetData(overlayData);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // just so that we don't call this millions of times
            if (OverlayVisibility > EPSILON)
                spriteBatch.Draw(_overlay, new Rectangle(0, 0, TRexRunnerGame.GAME_WINDOW_WIDTH, TRexRunnerGame.GAME_WINDOW_HEIGHT), Color.White * OverlayVisibility); // Color White has 255 on all RGBA, so if we multiply by, say, 0, then RGBA will be 0 including the Alpha which is responsible for transparancy
        }

        public void Update(GameTime gameTime)
        {
            if (_moon == null)
            {
                _moon = new Moon(this, _spriteSheet, _trex, new Vector2(TRexRunnerGame.GAME_WINDOW_WIDTH, MOON_POS_Y));
                _moon.DrawOrder = MOON_DRAW_ORDER;
                _entityManager.AddEntity(_moon);
            }

            HandleCloucSpawning();
            HandleStarSpawning();
            // we also need to despawn the skyobjects
            // remove clound/star if any of their position X is less than -100 (which means its already way off the game window screen)
            
          
            foreach(SkyObject skyObject in _entityManager.GetEntitiesOfType<SkyObject>().Where(s => s.Position.X < -100))
            {
                if (skyObject is Moon moon)
                {
                    // move it to the right hand side instead of removing in
                    moon.Position = new Vector2(TRexRunnerGame.GAME_WINDOW_WIDTH, MOON_POS_Y);
                }
                else
                    _entityManager.RemoveEntity(skyObject);     
            }
            // trow first statements to make sure we don't continue with night time after restart of the game   
            if(_previousScore != 0 && _previousScore < _scoreBoard.DisplayScore && _previousScore / NIGHT_TIME_SCORE != _scoreBoard.DisplayScore / NIGHT_TIME_SCORE)
            {
                TransitionToNightTime();
            }

            // so that we get daytime after restart of game
            if (_scoreBoard.DisplayScore < NIGHT_TIME_SCORE && (IsNight || _isTransitioningToNight))
            {
                TransitionToDayTime();
            }

            if (IsNight && (_scoreBoard.DisplayScore - _nightTimeStartScore >= NIGHT_TIME_DURATION_SCORE))
            {
                TransitionToDayTime();
            }

            UpdateTransition(gameTime);
            _previousScore = _scoreBoard.DisplayScore;
  
        }

        private void UpdateTransition(GameTime gameTime)
        {
            // if we are not in transition just return nothing
            if (_isTransitioningToNight)
            {
                // if we didn't devide by DURATION then _normalizedScreenColor would always reach value 1 after 1 second
                // but if we devide by target seconds, it should take exactly that any seconds
                _normalizedScreenColor -= (float)gameTime.ElapsedGameTime.TotalSeconds / TRANSITION_DURATION;

                if (_normalizedScreenColor < 0)
                    _normalizedScreenColor = 0;

                // when we transition to night, we need to call the UpdateTexture to set textures to "night mode"
                if (_normalizedScreenColor < 0.5f)
                {
                    InvertTextures();                           
                }

            }

            else if (_isTransitioningToDay)
            {
                // if we didn't devide by DURATION then _normalizedScreenColor would always reach value 1 after 1 second
                // but if we devide by target seconds, it should take exactly that any seconds
                _normalizedScreenColor += (float)gameTime.ElapsedGameTime.TotalSeconds / TRANSITION_DURATION;

                if (_normalizedScreenColor > 1)
                    _normalizedScreenColor = 1;

                if (_normalizedScreenColor >= 0.5f)
                {
                    InvertTextures();
                }
            }
              
        }

        // invert spriteSheet based on day time
        private void InvertTextures()
        {
            if (IsNight)
            {
                _spriteSheet.SetData(_invertedTextureData);
            }
            else
            {
                _spriteSheet.SetData(_textureData);
            }
        }

        private bool TransitionToNightTime()
        {
            if (IsNight || _isTransitioningToNight)
                return false;

            _nightTimeStartScore = _scoreBoard.DisplayScore;
            _isTransitioningToNight = true;
            _isTransitioningToDay = false;
            _normalizedScreenColor = 1f; // 1 represents white, and 0 to night
            NightCount++;
            
            return true;
        }

        private bool TransitionToDayTime()
        {
            if (!IsNight || _isTransitioningToDay)
                return false;

            _isTransitioningToDay = true;
            _isTransitioningToNight = false;
            _normalizedScreenColor = 0; // 1 represents white, and 0 to night

            return true;
        }

        private void HandleCloucSpawning()
        {
            IEnumerable<Cloud> clouds = _entityManager.GetEntitiesOfType<Cloud>();
            // if there are no clouds OR Game window width - Largest cloud X position >= target distance (that we generated randomly)
            if (clouds.Count() <= 0 || (TRexRunnerGame.GAME_WINDOW_WIDTH - clouds.Max(c => c.Position.X)) >= _targetCloudDistance)
            {
                // randomly generate cloud distance and Y spawn cordinate
                _targetCloudDistance = _random.Next(CLOUD_MIN_DISTANCE, CLOUD_MAX_DISTANCE + 1);
                int posY = _random.Next(CLOUD_MIN_POS_Y, CLOUD_MAX_POS_Y + 1);

                Cloud cloud = new Cloud(_spriteSheet, _trex, new Vector2(TRexRunnerGame.GAME_WINDOW_WIDTH, posY));
                cloud.DrawOrder = CLOUD_DRAW_ORDER;
                
                _entityManager.AddEntity(cloud);

            }
        }

        private void HandleStarSpawning()
        {
            IEnumerable<Star> stars = _entityManager.GetEntitiesOfType<Star>();
            // if there are no stars OR Game window width - Largest cloud X position >= target distance (that we generated randomly)
            if (stars.Count() <= 0 || (TRexRunnerGame.GAME_WINDOW_WIDTH - stars.Max(s => s.Position.X)) >= _targetStarDistance)
            {
                // randomly generate star distance and Y spawn cordinate
                _targetStarDistance = _random.Next(STAR_MIN_DISTANCE, STAR_MAX_DISTANCE + 1);
                int posY = _random.Next(STAR_MIN_POS_Y, STAR_MAX_POS_Y + 1);

                Star star = new Star(this, _spriteSheet, _trex, new Vector2(TRexRunnerGame.GAME_WINDOW_WIDTH, posY));
                star.DrawOrder = STAR_DRAW_ORDER;

                _entityManager.AddEntity(star);

            }
        }


    }
 }
