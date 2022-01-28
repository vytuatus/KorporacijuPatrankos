using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TRexGame.Graphics;

namespace TRexGame.Entities
{
    public class ObstacleManager : IGameEntity

    {
        // we will use when deciding at which hight to spawn flying dino
        private static readonly int[] FLYING_DINO_Y_POSITIONS = new int[] { 90, 62, 40};
        // min spawn distance that needs to be accumulated before obstacles are spawned
        private const float MIN_SPAWN_DISTANCE = 20;
        // min/max space between obstacles in pixels
        private const int MIN_OBSTACLE_DISTANCE = 10;
        private const int MAX_OBSTACLE_DISTANCE = 50;
        private const int OBSTACLE_DISTANCE_SPEED_TOLERANCE = 5;
        private const int LARGE_CACTUS_POS_Y = 80;
        private const int SMALL_CACTUS_POS_Y = 94;
        private const int FURNITURE_POS_Y = 65;

        private const int OBSTACLE_DRAW_ORDER = 12;
        private const int OBSTACLE_DESPAWN_POS_X = 200;

        // we need to have min 150 score to spawn flying dino
        private const int FLYING_DINO_SPAWN_SCORE_MIN = 150;

        private const int CANNON_SMOKE_ONE_TEXTURE_POS_X = 61;
        private const int CANNON_SMOKE_ONE_TEXTURE_POS_Y = 10;
        private const int CANNON_SMOKE_ONE_SPRITE_WIDTH = 13;
        private const int CANNON_SMOKE_ONE_SPRITE_HEIGHT = 13;

        private const int CANNON_SMOKE_TWO_TEXTURE_POS_X = 75;
        private const int CANNON_SMOKE_TWO_TEXTURE_POS_Y = 10;
        private const int CANNON_SMOKE_TWO_SPRITE_WIDTH = 13;
        private const int CANNON_SMOKE_TWO_SPRITE_HEIGHT = 13;

        private const int CANNON_SMOKE_THREE_TEXTURE_POS_X = 88;
        private const int CANNON_SMOKE_THREE_TEXTURE_POS_Y = 10;
        private const int CANNON_SMOKE_THREE_SPRITE_WIDTH = 10;
        private const int CANNON_SMOKE_THREE_SPRITE_HEIGHT = 13;

        private const int CANNON_SMOKE_FOUR_TEXTURE_POS_X = 120;
        private const int CANNON_SMOKE_FOUR_TEXTURE_POS_Y = 10;
        private const int CANNON_SMOKE_FOUR_SPRITE_WIDTH = 10;
        private const int CANNON_SMOKE_FOUR_SPRITE_HEIGHT = 13;

        // going to track score value as it was the last time obstacle was spawn
        // negative value indicates that obstacle has never been spawned
        private double _lastSpawnScore = -1;
        // store random value between min/max allowed obstacle distance.
        // will determine distacne between previously spawned obstacle and next one to spawn
        private double _currentTargetDistance;

        private readonly EntityManager _entityManager;
        private readonly TRex _trex;
        private readonly ScoreBoard _scoreBoard;

        private readonly Random _random;

        // enable and disable ObstacleManager since we don't need it to spawn new obstables all the time
        public bool IsEnabled { get; set; }

        public bool CanSpawnObstacles => IsEnabled && _scoreBoard.Score >= MIN_SPAWN_DISTANCE;

        public int DrawOrder => 0;
        private Texture2D _spriteSheet;
        private Texture2D _cannonsSpriteSheet;

        private Texture2D _officeFurnitureSpriteSheet;

        private int _previousScore;
        public bool _isCannonGame = false;
        private const float FIRST_CANNON_GAME_TIME = 22f;
        private const float FIRST_CANNON_GAME_TIME_END = 50f;
        private const float SECOND_CANNON_GAME_TIME = 84f;
        private const float SECOND_CANNON_GAME_TIME_END = 109f;
        private const float THIRD_CANNON_GAME_TIME = 144f;
        private const float THIRD_CANNON_GAME_TIME_END = 180f;

        private const int CANNON_TIME_STOP_SCORE = 700;
        private const int CANNON_SPAWN_DISTANCE = 2;
        private const int CANNON_POS_Y = 90;

        private float _trexSpeedBeforeCannon;
        private float _timeInCanonGame = 0;
        private float _cannonSpawnTime = 0;
        private int _numberOfPatrankasSpawned = 0;
        private PatrankaShootingDirection _patrankaShootingDirection = PatrankaShootingDirection.Right;
        public bool _stoppedShootingPatrakas = false;
        private Obstacle _cannon;
        private BackgroundManager _backgroundManager;
        private int _cannonGameCount = 0;
        private TRexRunnerGame _trexRunnerGame;
        private List<KeyValuePair<float, float>> listOfShootingFrequency = new List<KeyValuePair<float, float>>();
        private float _shootingFrequencyLeft;
        private float _shootingFrequencyRight;

        private SoundEffect _cannonShot;
        private SoundEffectInstance _sei;
        private SoundEffect _crowdChating;

        private SpriteAnimation _cannonSmokeSpriteAnimationRight;
        private SpriteAnimation _cannonSmokeSpriteAnimationLeft;
        private Sprite _cannonSmokeSpriteA;
        private Sprite _cannonSmokeSpriteB;
        private Sprite _cannonSmokeSpriteC;
        private Sprite _cannonSmokeSpriteD;
        private Vector2 _positionOfCannonSmokeAnimationRight;
        private Vector2 _positionOfCannonSmokeAnimationLeft;


        public ObstacleManager(Texture2D spriteSheet, Texture2D cannonsSpriteSheet, Texture2D officeFurnitureSpriteSheet, EntityManager entityManager, TRex trex, ScoreBoard scoreBoard, BackgroundManager backgroundManager, TRexRunnerGame trexRunnerGame, 
            SoundEffect cannonShot, SoundEffect crowdChanting)
        {
            _cannonShot = cannonShot;
            _crowdChating = crowdChanting;
            _sei = _crowdChating.CreateInstance();
            _backgroundManager = backgroundManager;
            _entityManager = entityManager;
            _trex = trex;
            // so we know the distance Trex traveled and based on that spawn new obstacles
            _scoreBoard = scoreBoard;
            _random = new Random();
            _spriteSheet = spriteSheet;
            _officeFurnitureSpriteSheet = officeFurnitureSpriteSheet;
            _cannonsSpriteSheet = cannonsSpriteSheet;
            _trexRunnerGame = trexRunnerGame;

            listOfShootingFrequency.Add(new KeyValuePair<float, float>(2f, 1.5f));
            listOfShootingFrequency.Add(new KeyValuePair<float, float>(1.3f, 1.7f));
            listOfShootingFrequency.Add(new KeyValuePair<float, float>(1.4f, 1.5f));
            listOfShootingFrequency.Add(new KeyValuePair<float, float>(1.5f, 1.7f));
            listOfShootingFrequency.Add(new KeyValuePair<float, float>(1.5f, 1.2f));
            listOfShootingFrequency.Add(new KeyValuePair<float, float>(1.1f, 1.2f));
            listOfShootingFrequency.Add(new KeyValuePair<float, float>(1.8f, 1.5f));
            listOfShootingFrequency.Add(new KeyValuePair<float, float>(1.5f, 1.7f));

            _shootingFrequencyLeft = 3f;
            _shootingFrequencyRight = 2.8f;

            _cannonSmokeSpriteA = new Sprite(cannonsSpriteSheet, CANNON_SMOKE_ONE_TEXTURE_POS_X, CANNON_SMOKE_ONE_TEXTURE_POS_Y, CANNON_SMOKE_ONE_SPRITE_WIDTH, CANNON_SMOKE_ONE_SPRITE_HEIGHT);
            _cannonSmokeSpriteB = new Sprite(cannonsSpriteSheet, CANNON_SMOKE_TWO_TEXTURE_POS_X, CANNON_SMOKE_TWO_TEXTURE_POS_Y, CANNON_SMOKE_TWO_SPRITE_WIDTH, CANNON_SMOKE_TWO_SPRITE_HEIGHT);
            _cannonSmokeSpriteC = new Sprite(cannonsSpriteSheet, CANNON_SMOKE_THREE_TEXTURE_POS_X, CANNON_SMOKE_THREE_TEXTURE_POS_Y, CANNON_SMOKE_THREE_SPRITE_WIDTH, CANNON_SMOKE_THREE_SPRITE_HEIGHT);
            _cannonSmokeSpriteD = new Sprite(cannonsSpriteSheet, CANNON_SMOKE_FOUR_TEXTURE_POS_X, CANNON_SMOKE_FOUR_TEXTURE_POS_Y, CANNON_SMOKE_FOUR_SPRITE_WIDTH, CANNON_SMOKE_FOUR_SPRITE_HEIGHT);
            _cannonSmokeSpriteAnimationRight = new SpriteAnimation();
            _cannonSmokeSpriteAnimationLeft = new SpriteAnimation();


        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _cannonSmokeSpriteAnimationRight.Draw(spriteBatch, _positionOfCannonSmokeAnimationRight);
            _cannonSmokeSpriteAnimationLeft.Draw(spriteBatch, _positionOfCannonSmokeAnimationLeft);

        }

        public void Update(GameTime gameTime)
        {
            // if not enabled just return
            if (!IsEnabled)
                return;

            // so if current score - the score that we had when last obstacle was spawn is >= our distance between obstables, 
            // then try to spawn new obstacle
            if(CanSpawnObstacles && 
                (_lastSpawnScore <0 || (_scoreBoard.Score - _lastSpawnScore >= _currentTargetDistance)))
            {
                _currentTargetDistance = _random.NextDouble() * (MAX_OBSTACLE_DISTANCE - MIN_OBSTACLE_DISTANCE) + MIN_OBSTACLE_DISTANCE;

                // say if we reached only half the Max speed, we will add only half the tolerance level to distance
                // we do this to increase the distance between obstacles when speed increases
                _currentTargetDistance += (_trex.Speed - TRex.START_SPEED) / (TRex.MAX_SPEED - TRex.START_SPEED) * OBSTACLE_DISTANCE_SPEED_TOLERANCE;
                _lastSpawnScore = _scoreBoard.Score;

                SpawnRandomObstacle();
                               
                
            }

            SpawnPatranka();
            _cannonSmokeSpriteAnimationRight.Update(gameTime);
            _cannonSmokeSpriteAnimationLeft.Update(gameTime);
            

            foreach (Obstacle obstacle in _entityManager.GetEntitiesOfType<Obstacle>())
            {
                // 200 instead of 0, because we need to factor in the width of the obstacle
                // -200 because we want obstacle to disapear when it is off the screen
                if (obstacle.Position.X < -OBSTACLE_DESPAWN_POS_X || obstacle.Position.X > TRexRunnerGame.GAME_WINDOW_WIDTH + OBSTACLE_DESPAWN_POS_X)
                    _entityManager.RemoveEntity(obstacle);
                
            }

            // trow first statements to make sure we don't continue with night time after restart of the game   
            if (!_isCannonGame && _previousScore != 0 && _previousScore < _scoreBoard.DisplayScore && 
                //(_scoreBoard.DisplayScore == FIRST_CANNON_GAME_SCORE || _scoreBoard.DisplayScore == SECOND_CANNON_GAME_SCORE || _scoreBoard.DisplayScore == THIRD_CANNON_GAME_SCORE))
                (_trexRunnerGame._korporacijuPatrankosSongElapsed > FIRST_CANNON_GAME_TIME && _cannonGameCount == 0) ||
                (_trexRunnerGame._korporacijuPatrankosSongElapsed > SECOND_CANNON_GAME_TIME && _cannonGameCount == 1) ||
                (_trexRunnerGame._korporacijuPatrankosSongElapsed > THIRD_CANNON_GAME_TIME && _cannonGameCount == 2))
            {
                _cannonGameCount += 1;
                
                _trexSpeedBeforeCannon = _trex.Speed;
                TransitionToCannonGame();
            }

            

            if (_isCannonGame && _previousScore != 0 && _timeInCanonGame > 10f)
            {
                IEnumerable<Patranka> patrankos = _entityManager.GetEntitiesOfType<Patranka>();
                IEnumerable<Cannon> cannons = _entityManager.GetEntitiesOfType<Cannon>();
                if (patrankos.Count() == 0 && cannons.Count() == 0)
                    TransitionToNormalGame();

            }
            
            CannonTransition(gameTime);

            _previousScore = _scoreBoard.DisplayScore;
        }


        private void CannonTransition(GameTime gameTime)
        {
            if (_isCannonGame)
            {
                // Let the Trex finish the jumping/falling actions first
                if (_trex.State == TrexState.Running)
                    _trex.State = TrexState.Canons;
                _trex.Speed = 0;
                _timeInCanonGame += (float)gameTime.ElapsedGameTime.TotalSeconds;
                _cannonSpawnTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            }
        }
        private void TransitionToNormalGame()
        {
            _numberOfPatrankasSpawned = 0;
            _timeInCanonGame = 0;
            _cannonSpawnTime = 0;
            _trex.Speed = _trexSpeedBeforeCannon;
            _isCannonGame = false;
            _backgroundManager._isCannonGame = false;
            _backgroundManager.SpawnNormalBackgroundAnimation(0);

            if (_cannonGameCount == 3)
            {
                _cannonGameCount = 0;
                _trexRunnerGame._korporacijuPatrankosSongElapsed = 0;
                _trexRunnerGame.StopSong();
                _trexRunnerGame.PlaySong();
            }
            

            //_backgroundManager.Initialize();
            // could be that we are in jumping or other state. if so no need to transition to Running
            if (_trex.State == TrexState.Canons)
                _trex.State = TrexState.Running;
            _trex.SetCannonGameFalse();
            _trex.MOVE_LEFT_RIGHT_ACCELERATION = 60f;
            _stoppedShootingPatrakas = false;
            _cannon = null;
        }

        private bool TransitionToCannonGame()
        {
            if (_isCannonGame)
                return false;

            _stoppedShootingPatrakas = false;
            _numberOfPatrankasSpawned = 0;
            _timeInCanonGame = 0;
            _cannonSpawnTime = 0;
            _isCannonGame = true;
            _shootingFrequencyLeft -= 0.5f;
            _shootingFrequencyRight -= 0.5f;
          
            SpawnCannon(PatrankaShootingDirection.Left);
            SpawnCannon(PatrankaShootingDirection.Right);
            _backgroundManager._isCannonGame = true;
            _backgroundManager.SpawnBeginCannonGameAnimationTile();
            // Let the Trex finish the jumping/falling actions first
            if (_trex.State == TrexState.Running)
                _trex.State = TrexState.Canons;
            _trex.SetCannonGameTrue();
            _trex.MOVE_LEFT_RIGHT_ACCELERATION = 120f;
            // if it is the last value of enum, restart the value from first one
            if (_patrankaShootingDirection == PatrankaShootingDirection.Right)
            {
                _patrankaShootingDirection = PatrankaShootingDirection.Left;
            }
            else
            {
                _patrankaShootingDirection += 1;
            }

            return true;

        }

        private void SpawnRandomObstacle()
        {
            // TODO: Create Instance of obstacle and add it to entityManager
            // for testing. we will always spawn cactuses

            Obstacle obstacle = null;

            int cactusGroupSpawnRate = 60;
            int flyingDinoSpawnRate = _scoreBoard.Score >= FLYING_DINO_SPAWN_SCORE_MIN ? 40 : 0;

            // +1 since the max value is not inclusive. it will spawn a number between 0 and 100
            // (or 60 if min score is not reached and flyingDinoSpawn rate is 0)
            int rng = _random.Next(0, cactusGroupSpawnRate + flyingDinoSpawnRate + 1);

            if (rng <= cactusGroupSpawnRate) // spawn cactus
            {
                // generate random value between 0 and 2 inclusively (hence we add +1 to max value)
                // CactusGroup.GroupSize randomGroupSize = (CactusGroup.GroupSize)_random.Next((int)CactusGroup.GroupSize.Small, (int)CactusGroup.GroupSize.Large + 1);

                // generate random value between 0 and 8 inclusively (hence we add +1 to max value)
                OfficeFurniture.FurnitureType randomFurnitureType = (OfficeFurniture.FurnitureType)_random.Next((int)OfficeFurniture.FurnitureType.GreyDesk, (int)OfficeFurniture.FurnitureType.PeopleOverTable + 1);
                
                // bool isLarge = _random.NextDouble() > 0.5f;

                // if cactus group is large than it will sit higher on the screen
                //float posY = isLarge ? LARGE_CACTUS_POS_Y : SMALL_CACTUS_POS_Y;

                // the position can be GAME_WINDOW_WIDTH since initially we want our obstacle to be off screen
                obstacle = new OfficeFurniture(_officeFurnitureSpriteSheet, randomFurnitureType, _trex, new Vector2(TRexRunnerGame.GAME_WINDOW_WIDTH, FURNITURE_POS_Y));

            }
            else //spawn Flying dino
            {
                int verticalPosINdex = _random.Next(0, FLYING_DINO_Y_POSITIONS.Length);
                float posY = FLYING_DINO_Y_POSITIONS[verticalPosINdex];
                obstacle = new FlyingDino(_trex, new Vector2(TRexRunnerGame.GAME_WINDOW_WIDTH, posY), _officeFurnitureSpriteSheet);
            }
            obstacle.DrawOrder = OBSTACLE_DRAW_ORDER;

            _entityManager.AddEntity(obstacle); 
        }


        private void SpawnCannon(PatrankaShootingDirection patrankaShootingDirection)
        {
            //IEnumerable<Cannon> cannons = _entityManager.GetEntitiesOfType<Cannon>(); && cannons.Count() == 0
            if (_isCannonGame) // if we are in cannon game and there are no cannonts on screen
            {
                Obstacle cannon = null;

                // TODO: check for which cannonts are spawned. and do not spawn a the same cannon twice (ex. two cannons on left side)

                if (patrankaShootingDirection == PatrankaShootingDirection.Left)
                {
                    cannon = new Cannon(patrankaShootingDirection, this, _cannonsSpriteSheet, _trex, new Vector2(TRexRunnerGame.GAME_WINDOW_WIDTH, CANNON_POS_Y));
                }
                else if (patrankaShootingDirection == PatrankaShootingDirection.Right)
                {
                    cannon = new Cannon(patrankaShootingDirection, this, _cannonsSpriteSheet, _trex, new Vector2(-30, CANNON_POS_Y));
                }

                _cannon = cannon;
                cannon = null;

                _entityManager.AddEntity(_cannon);
            }
                
        }

        private void SpawnPatranka()
        {
            // spawn only if it is cannon game
            if (_isCannonGame)
            {

                // first check if we can spawn another cannon ball
                if (_stoppedShootingPatrakas == false)
                {

                    IEnumerable<Cannon> cannons = _entityManager.GetEntitiesOfType<Cannon>().ToList();
                    int numberOfCannons = cannons.Count();
                    
                    // if we can spawn, then start looping cannons
                    for (int i = 0; i < numberOfCannons; i++)
                    {

                        Cannon cannon1 = cannons.ElementAt(i);
                        if (cannon1.ReachedFiringPosition)
                        {
                            _backgroundManager.SpawnCannonGameAnimationTile();
                            if (_sei.State == SoundState.Playing)
                            {
                                // Already playing no need to play again
                            }
                            else
                            {
                                _sei.Play();
                                _sei.Volume = 0.5f;
                            }
                            

                            Console.WriteLine(i);

                            Obstacle obstacle = null;

                            Random random = new Random();
                            int shootingFrequency = random.Next(0, listOfShootingFrequency.Count);

                            if (cannon1.PatrankaShootingDirection == PatrankaShootingDirection.Left && cannon1._PatrankaSpawnTime > _shootingFrequencyLeft)
                            {
                                cannon1._PatrankaSpawnTime = 0;
                                obstacle = new Patranka(cannon1.PatrankaShootingDirection, _cannonsSpriteSheet, _trex, new Vector2(TRexRunnerGame.GAME_WINDOW_WIDTH - 60, CANNON_POS_Y));
                                obstacle.DrawOrder = OBSTACLE_DRAW_ORDER;
                                _entityManager.AddEntity(obstacle);
                                _numberOfPatrankasSpawned += 1;
                                _cannonShot.Play(1,0,0);
                                _positionOfCannonSmokeAnimationLeft = new Vector2(TRexRunnerGame.GAME_WINDOW_WIDTH - 60, CANNON_POS_Y);
                                _cannonSmokeSpriteAnimationLeft = CreateCannonSmokeAnimation(_cannonSmokeSpriteAnimationLeft);
                                _cannonSmokeSpriteAnimationLeft.ResetPlaybackProgress();
                                _cannonSmokeSpriteAnimationLeft.Play();

                            }
                            else if (cannon1.PatrankaShootingDirection == PatrankaShootingDirection.Right && cannon1._PatrankaSpawnTime > _shootingFrequencyRight)
                            {
                                cannon1._PatrankaSpawnTime = 0;
                                obstacle = new Patranka(cannon1.PatrankaShootingDirection, _cannonsSpriteSheet, _trex, new Vector2(50, CANNON_POS_Y));
                                obstacle.DrawOrder = OBSTACLE_DRAW_ORDER;
                                _entityManager.AddEntity(obstacle);
                                _numberOfPatrankasSpawned += 1;
                                _cannonShot.Play(1,0,0);
                                _positionOfCannonSmokeAnimationRight = new Vector2(50, CANNON_POS_Y);
                                _cannonSmokeSpriteAnimationRight = CreateCannonSmokeAnimation(_cannonSmokeSpriteAnimationRight);
                                _cannonSmokeSpriteAnimationRight.ResetPlaybackProgress();
                                _cannonSmokeSpriteAnimationRight.Play();
                            }

                            
                            _stoppedShootingPatrakas = false;
                            

                            _cannonSpawnTime = 0;
                                                       
                        }
                                        
                    } 
                }

                // this will be needed for Cannon, so that it knows when to start disapearing from screen
                if ((_trexRunnerGame._korporacijuPatrankosSongElapsed > FIRST_CANNON_GAME_TIME_END && _cannonGameCount == 1) ||
                    (_trexRunnerGame._korporacijuPatrankosSongElapsed > SECOND_CANNON_GAME_TIME_END && _cannonGameCount == 2) ||
                    (_trexRunnerGame._korporacijuPatrankosSongElapsed > THIRD_CANNON_GAME_TIME_END && _cannonGameCount == 3))
                {
                   
                    _stoppedShootingPatrakas = true;
                    _backgroundManager.SpawnEndCannonGameAnimationTile();
                }
            }
        }

        public void Reset()
        {
            foreach(Obstacle obstacle in _entityManager.GetEntitiesOfType<Obstacle>())
            {
                _entityManager.RemoveEntity(obstacle);
            }

            _cannonGameCount = 0;
            _currentTargetDistance = 0;
            _lastSpawnScore = -1;
            _isCannonGame = false;
            _trex.SetCannonGameFalse();
            _trex.StopMoveLeft();
            _trex.StopMoveRight();
        }

        private SpriteAnimation CreateCannonSmokeAnimation(SpriteAnimation spriteAnimation)
        {

            spriteAnimation.ShouldLoop = false;

            // the first frame would start at 0 seconds. _spriteA was initiated in constructor
            spriteAnimation.AddFrame(_cannonSmokeSpriteA, 0);
            // second frame is 1/20f._SpriteB was initiated in constructor.
            spriteAnimation.AddFrame(_cannonSmokeSpriteB, 0.2f);
            // thrid frame.
            spriteAnimation.AddFrame(_cannonSmokeSpriteC, 0.2f * 2);
            spriteAnimation.AddFrame(_cannonSmokeSpriteD, 0.2f * 3);


            // another frame to indicate the end of the animation or in other word, how long animation should last
            spriteAnimation.AddFrame(_cannonSmokeSpriteD, 0.2f * 4);
            spriteAnimation.ShouldLoop = false;

            return spriteAnimation;
        }
    }
}
