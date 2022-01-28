using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TRexGame.Graphics;
using System.Linq;
namespace TRexGame.Entities
{
    public class BackgroundManager : IGameEntity
    {
        private const float GROUND_TILE_POS_Y = 0;

        // actuall ground sprite width is 1200 but we will devide it in two halfs and span it randomly
        // the numbers represent from where in spriteSheet to take the texture
        private const int SPRITE_WIDTH = 600;
        private const int SPRITE_HEIGHT = 150;
        private const int SPRITE_ONE_POS_X = 1;
        private const int SPRITE_ONE_POS_Y = 1;
        private const int SPRITE_TWO_POS_X = 1;
        private const int SPRITE_TWO_POS_Y = 1;


        private List<Texture2D> _spriteSheets = new List<Texture2D>();
        private List<Texture2D> _beginCannonGameSpriteSheets = new List<Texture2D>();
        private List<Texture2D> _cannonGameSpriteSheets = new List<Texture2D>();
        private List<Texture2D> _endCannonGameSpriteSheets = new List<Texture2D>();

        private readonly EntityManager _entityManager;
        private readonly List<MovingSpriteAnimation> _backgroundTileAnimations;

        private Sprite _regularBackgroundSprite;
        private Sprite _backgroundSpriteV2;
        private Sprite _backgroundSpriteV3;
        private SpriteAnimation _backgroundAnimation1;
        private SpriteAnimation _beginCannonGameBackgroundAnimation;
        private SpriteAnimation _cannonGameBackgroundAnimation;
        private SpriteAnimation _endCannonGameBackgroundAnimation;

        private TRex _trex;
        public bool _isCannonGame { get; set; }

        private Random _random;

        public int DrawOrder { get; set; }

        public BackgroundManager(List<Texture2D> spriteSheets, List<Texture2D> beginCannonGameSpriteSheets, List<Texture2D> cannonGameSpriteSheets, 
            List<Texture2D> endCannonGameSpriteSheets, EntityManager entityManager, TRex trex)
        {
            _spriteSheets = spriteSheets;
            _beginCannonGameSpriteSheets = beginCannonGameSpriteSheets;
            _cannonGameSpriteSheets = cannonGameSpriteSheets;
            _endCannonGameSpriteSheets = endCannonGameSpriteSheets;

            _backgroundTileAnimations = new List<MovingSpriteAnimation>();
            _entityManager = entityManager;
            _trex = trex;
            _random = new Random();

            _backgroundAnimation1 = new SpriteAnimation();
            _beginCannonGameBackgroundAnimation = new SpriteAnimation();
            _cannonGameBackgroundAnimation = new SpriteAnimation();
            _endCannonGameBackgroundAnimation = new SpriteAnimation();

            _backgroundAnimation1 = CreateAnimation(_spriteSheets, _backgroundAnimation1, true, 1f);
            _beginCannonGameBackgroundAnimation = CreateAnimation(_beginCannonGameSpriteSheets, _beginCannonGameBackgroundAnimation, false, 0.5f);
            _cannonGameBackgroundAnimation = CreateAnimation(_cannonGameSpriteSheets, _cannonGameBackgroundAnimation, true, 0.5f);
            _endCannonGameBackgroundAnimation = CreateAnimation(_endCannonGameSpriteSheets, _endCannonGameBackgroundAnimation, false, 0.5f);

            
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            
            // you don't really draw the GroundManager. Since it is a manager - not a groundTile. 
            // you only need groudManager as part of the _entityManager in order to update groundTiles (ex. positionX) 
            foreach (MovingSpriteAnimation msa in _backgroundTileAnimations)
            {
                msa.Draw(spriteBatch, gameTime);
            }
        }

        // so every time this method is called from TRexRunnerGame Class, the method will loop through the background tiles and update the
        // Position X for all the background tales every frame
        public void Update(GameTime gameTime)
        {

            // spawn new tile. thw moment any of the tiles goes to negative X, we will have space on right side of screen since SPRITE_WIDTH
            // will no longer cover the whole game screen

            // First check if there are any tiles, if yes, only then we can get the Max position. If we would call Max on empty list, we
            // will get an exception
            if (_backgroundTileAnimations.Any())
            {
                float maxPosX = _backgroundTileAnimations.Max(bta => bta.PositionX);
               
                if (maxPosX < 0)
                    SpawnTile(maxPosX);
            }

            // since we cannot remove ground tile during loop. we need new list
            List<MovingSpriteAnimation> tilesToRemove = new List<MovingSpriteAnimation>();

            foreach (MovingSpriteAnimation msa in _backgroundTileAnimations)
            {

                // so that tiles move only if we are not in cannon game
                msa._positionXShouldChange = !_isCannonGame;
                msa.Update(gameTime);
                if (msa.PositionX < -SPRITE_WIDTH)
                {
                    // so here we will remove all ground tiles from entityManager we added during initialize(). However, we will not remove
                    // the groundManager. It will always stay in _entityManager. 

                    // No need to remove groundTile from _entity manager since we have decided to never put it there. 
                    //_entityManager.RemoveEntity(gt);
                    tilesToRemove.Add(msa);
                }
            }

            foreach (MovingSpriteAnimation msa in tilesToRemove)
            {
                _backgroundTileAnimations.Remove(msa);
            }
        }

        // when we start the game first background tile should already be spawned. Will be called by TRexRunnerGame class
        public void Initialize()
        {
            // Make sure first that before we initialize, our groundTile list is empty
            _backgroundTileAnimations.Clear();

            _isCannonGame = false;
            // First tile to appear in the game will always be the regular one
            MovingSpriteAnimation backgroundTile = CreateMovingAnimationTile(0);
            _backgroundTileAnimations.Add(backgroundTile);
            // you add groundTile to _entityManager so that you could call the the Draw method. 
            // or you could skip this and simply implement the draw method in ground manager which is done in this case
            //_entityManager.AddEntity(groundTile);
        }

        private MovingSpriteAnimation CreateMovingAnimationTile(float positionX)
        {

            
            _cannonGameBackgroundAnimation.Stop();
            _cannonGameBackgroundAnimation.ResetPlaybackProgress();
            _endCannonGameBackgroundAnimation.Stop();
            _endCannonGameBackgroundAnimation.ResetPlaybackProgress();
            _beginCannonGameBackgroundAnimation.Stop();
            _beginCannonGameBackgroundAnimation.ResetPlaybackProgress();
            _backgroundAnimation1.Play();
            _backgroundAnimation1.ResetPlaybackProgress();

            // PositionX and positionY are the start position where the ground tile should be drawn on the screen
            MovingSpriteAnimation msa = new MovingSpriteAnimation(_trex, _backgroundAnimation1, positionX, GROUND_TILE_POS_Y, BackgroundAnimationType.Normal);


            return msa;
        }

        private MovingSpriteAnimation CreateBeginCannonGameMovingAnimationTile(float positionX)
        {
            _backgroundAnimation1.Stop();
            _backgroundAnimation1.ResetPlaybackProgress();
            _cannonGameBackgroundAnimation.Stop();
            _cannonGameBackgroundAnimation.ResetPlaybackProgress();
            _endCannonGameBackgroundAnimation.Stop();
            _endCannonGameBackgroundAnimation.ResetPlaybackProgress();
            _beginCannonGameBackgroundAnimation.Play();
            _beginCannonGameBackgroundAnimation.ResetPlaybackProgress();
            
            // PositionX and positionY are the start position where the ground tile should be drawn on the screen
            _beginCannonGameBackgroundAnimation = new SpriteAnimation();
            _beginCannonGameBackgroundAnimation = CreateAnimation(_beginCannonGameSpriteSheets, _beginCannonGameBackgroundAnimation, false, 0.5f);
            _beginCannonGameBackgroundAnimation.Play();
            _beginCannonGameBackgroundAnimation.ResetPlaybackProgress();

            MovingSpriteAnimation msa = new MovingSpriteAnimation(_trex, _beginCannonGameBackgroundAnimation, positionX, GROUND_TILE_POS_Y, BackgroundAnimationType.CannonBegin);

            return msa;
        }

        private MovingSpriteAnimation CreateCannonGameMovingAnimationTile(float positionX)
        {
            _backgroundAnimation1.Stop();
            _backgroundAnimation1.ResetPlaybackProgress();
            _cannonGameBackgroundAnimation.Play();
            _cannonGameBackgroundAnimation.ResetPlaybackProgress();
            _endCannonGameBackgroundAnimation.Stop();
            _endCannonGameBackgroundAnimation.ResetPlaybackProgress();
            _beginCannonGameBackgroundAnimation.Stop();
            _beginCannonGameBackgroundAnimation.ResetPlaybackProgress();
            // PositionX and positionY are the start position where the ground tile should be drawn on the screen
            MovingSpriteAnimation msa = new MovingSpriteAnimation(_trex, _cannonGameBackgroundAnimation, positionX, GROUND_TILE_POS_Y, BackgroundAnimationType.Cannon);

            return msa;
        }

        private MovingSpriteAnimation CreateEndGameMovingAnimationTile(float positionX)
        {

            _backgroundAnimation1.Stop();
            _backgroundAnimation1.ResetPlaybackProgress();
            _cannonGameBackgroundAnimation.Stop();
            _cannonGameBackgroundAnimation.ResetPlaybackProgress();
            _endCannonGameBackgroundAnimation.Play();
            _endCannonGameBackgroundAnimation.ResetPlaybackProgress();
            _beginCannonGameBackgroundAnimation.Stop();
            _beginCannonGameBackgroundAnimation.ResetPlaybackProgress();
            // PositionX and positionY are the start position where the ground tile should be drawn on the screen
            MovingSpriteAnimation msa = new MovingSpriteAnimation(_trex, _endCannonGameBackgroundAnimation, positionX, GROUND_TILE_POS_Y, BackgroundAnimationType.CannonEnd);

            return msa;
        }


        public void SpawnNormalBackgroundAnimation(float maxPosX)
        {
           
            // if there are any times in the list, then take their positionX and use it for spawning new tiles
            if (_backgroundTileAnimations.Any())
            {
                List<float> positionsX = new List<float>();
                foreach (MovingSpriteAnimation movingSpriteAnimation in _backgroundTileAnimations)
                {
                    positionsX.Add(movingSpriteAnimation.PositionX);
                }

                // first clear all the animations 
                _backgroundTileAnimations.Clear();

                foreach (float positionX in positionsX)
                {
                    // PositionX is = 0 since we want to tile to be added at the beg of window and positionY are the start position where the ground tile should be drawn on the screen
                    MovingSpriteAnimation msa = CreateMovingAnimationTile(positionX);
                    _backgroundTileAnimations.Add(msa);
                }

            }
            else
            {
                _backgroundTileAnimations.Clear();
                float posX = maxPosX + SPRITE_WIDTH;
                MovingSpriteAnimation backgroundTile = CreateMovingAnimationTile(posX);
                _backgroundTileAnimations.Add(backgroundTile);

            }


        }

        public void SpawnTile(float maxPosX)
        {
  
            float posX = maxPosX + SPRITE_WIDTH;
            MovingSpriteAnimation backgroundTile = CreateMovingAnimationTile(posX);
            _backgroundTileAnimations.Add(backgroundTile);
            
        }

        public void SpawnBeginCannonGameAnimationTile()
        {
            // here we will store temporarily all positions of the previous backgorund Time animations so that we can use them when spawning
            // new tiles (we don't want new tiles to appear in position 0 if previous tiles were in different position

            List<float> positionsX = new List<float>();

            // we don't need to load same animation again if it is already loaded
            if (_backgroundTileAnimations.Max(mta => mta._backgroundAnimationType) == BackgroundAnimationType.CannonBegin)
                return;

            foreach (MovingSpriteAnimation movingSpriteAnimation in _backgroundTileAnimations)
            {
                positionsX.Add(movingSpriteAnimation.PositionX);
            }

            // first clear all the animations 
            _backgroundTileAnimations.Clear();

            foreach (float positionX in positionsX)
            {
                // PositionX is = 0 since we want to tile to be added at the beg of window and positionY are the start position where the ground tile should be drawn on the screen
                MovingSpriteAnimation msa = CreateBeginCannonGameMovingAnimationTile(positionX);
                _backgroundTileAnimations.Add(msa);
            }
            

        }

        public void SpawnCannonGameAnimationTile()
        {

            List<float> positionsX = new List<float>();

            // we don't need to load same animation again if it is already loaded
            if (_backgroundTileAnimations.Max(mta => mta._backgroundAnimationType) == BackgroundAnimationType.Cannon)
                return;

            foreach (MovingSpriteAnimation movingSpriteAnimation in _backgroundTileAnimations)
            {
                positionsX.Add(movingSpriteAnimation.PositionX);
            }

            // first clear all the animations
            _backgroundTileAnimations.Clear();

            foreach (float positionX in positionsX)
            {
                // PositionX is = 0 since we want to tile to be added at the beg of window and positionY are the start position where the ground tile should be drawn on the screen
                MovingSpriteAnimation msa = CreateCannonGameMovingAnimationTile(positionX);
                _backgroundTileAnimations.Add(msa);
            }

        }

        public void SpawnEndCannonGameAnimationTile()
        {

            List<float> positionsX = new List<float>();

            // we don't need to load same animation again if it is already loaded
            if (_backgroundTileAnimations.Max(mta => mta._backgroundAnimationType) == BackgroundAnimationType.CannonEnd)
                return;

            foreach (MovingSpriteAnimation movingSpriteAnimation in _backgroundTileAnimations)
            {
                positionsX.Add(movingSpriteAnimation.PositionX);
            }

            // first clear all the animations
            _backgroundTileAnimations.Clear();

            foreach (float positionX in positionsX)
            {
                // PositionX is = 0 since we want to tile to be added at the beg of window and positionY are the start position where the ground tile should be drawn on the screen
                MovingSpriteAnimation msa = CreateEndGameMovingAnimationTile(positionX);
                _backgroundTileAnimations.Add(msa);
            }    
        }


        private SpriteAnimation CreateAnimation(List<Texture2D> spriteSheets, SpriteAnimation backgroundAnimation, bool shouldLoop, float speedOfAnimation)
        {
            // load one background animation
            //_backgroundAnimation1.AddFrame(new Sprite(_spriteSheets.First(), SPRITE_ONE_POS_X, SPRITE_ONE_POS_Y, SPRITE_WIDTH, SPRITE_HEIGHT), 0);
            float animationFrequency = 0f;
            foreach (Texture2D backgroundSpritesheet in spriteSheets)
            {
                backgroundAnimation.ShouldLoop = shouldLoop;
                backgroundAnimation.AddFrame(new Sprite(backgroundSpritesheet, SPRITE_ONE_POS_X, SPRITE_ONE_POS_Y, SPRITE_WIDTH, SPRITE_HEIGHT), animationFrequency);
                animationFrequency += speedOfAnimation;
            }

            backgroundAnimation.AddFrame(new Sprite(spriteSheets.Last(), SPRITE_ONE_POS_X, SPRITE_ONE_POS_Y, SPRITE_WIDTH, SPRITE_HEIGHT), animationFrequency + speedOfAnimation);
            // backgroundAnimation.Play();
            backgroundAnimation.ResetPlaybackProgress();
            return backgroundAnimation;

        }

    }
}
