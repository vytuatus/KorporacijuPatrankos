using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TRexGame.Graphics;

namespace TRexGame.Entities
{
    public class TRex : IGameEntity, ICollidable
    {

        // in pixels
        private const float MIN_JUMP_HEIGHT = 40f;
        private const float GRAVITY = 1600f;
        // pixels per second
        private const float JUMP_START_VELOCITY = -480f;
        // so that we smoothly transition to falling
        private const float CANCEL_JUMP_VELOCITY = -100f;
        private const float DROP_VELOCITY = 600f;
        private const float RUN_ANIMATION_FRAME_RATE = 1 / 10f;
        private const float DUCK_ANIMATION_FRAME_RATE = 1 / 10f;
        public const float START_SPEED = 380f;
        public const float MAX_SPEED = 900f;
        public const float ACCELERATION_PPS_PER_SECOND = 3f;

        public const int COLLISION_SPRITE_WIDTH = 21;
        public const int COLLISION_SPRITE_HEIGHT = 59;

        public const int IDLE_BACKGROUND_SPRITE_POS_X = 9;
        public const int IDLE_BACKGROUND_SPRITE_POS_Y = 24;
        public const int IDLE_BACKGROUND_SPRITE_WIDTH = 20;
        public const int IDLE_BACKGROUND_SPRITE_HEIGHT = 59;

        public const int RUNNING_SPRITE_ONE_POS_X = 39;
        public const int RUNNING_SPRITE_ONE_POS_Y = 26;
        public const int RUNNING_SPRITE_ONE_WIDTH = 35;
        public const int RUNNING_SPRITE_ONE_HEIGHT = 57;
        public const int RUNNING_SPRITE_TWO_POS_X = 84;
        public const int RUNNING_SPRITE_TWO_POS_Y = 26;
        public const int RUNNING_SPRITE_TWO_WIDTH = 39;
        public const int RUNNING_SPRITE_TWO_HEIGHT = 57;

        public const int DUCKING_SPRITE_ONE_POS_X = 137;
        public const int DUCKING_SPRITE_ONE_POS_Y = 26;
        public const int DUCKING_SPRITE_ONE_WIDTH = 46;
        public const int DUCKING_SPRITE_ONE_HEIGHT = 57;
        public const int DUCKING_SPRITE_TWO_POS_X = 194;
        public const int DUCKING_SPRITE_TWO_POS_Y = 26;
        public const int DUCKING_SPRITE_TWO_WIDTH = 46;
        public const int DUCKING_SPRITE_TWO_HEIGHT = 57;

        private const int DEAD_SPRITE_POS_X = 257;
        private const int DEAD_SPRITE_POS_Y = 22;
        private const int DEAD_SPRITE_WIDTH = 34;
        private const int DEAD_SPRITE_HEIGHT = 60;


        private const int COLLISION_BOX_INSET = 7;

        private const float BLINK_ANIMATION_RANDOM_MIN = 2f;
        private const float BLINK_ANIMATION_RANDOM_MAX = 10f;
        private const float BLINK_ANIMATION_EYE_CLOSE_TIME = 0.2f;
        private const int DUCK_COLLISION_REDUCTION = 20;
        public float MOVE_LEFT_RIGHT_ACCELERATION = 60f;
        private SpriteAnimation _blinkAnimation;
        private SpriteAnimation _runAnimation;
        private SpriteAnimation _duckAnimation;
        

        private Sprite _idleTrexBackgroundSprite;
        private Sprite _idleSprite; 
        private Sprite _idleBlinkSprite;
        private Sprite _runSpriteOne;
        private Sprite _runSpriteTwo;
        private Sprite _duckSpriteOne;
        private Sprite _duckSpriteTwo;
        private Sprite _deadSprite;

        private SoundEffect _jumpSound;

        private Random _random;
        private float _verticalVelocity;
        private float _dropVelocity;
        private float _startPosX;
        private float _startPosY;
        private bool _moveRight = false;
        private bool _moveLeft = false;
        private bool _isCannonGame = false;

        public event EventHandler JumpComplete;
        public event EventHandler Died;

        public Vector2 Position { get; set; }
        // state should be changed only by this class
        public TrexState State { get; set; }
        public bool IsAlive { get; private set; }
        // initialize speed. 20 pixels a second
        public float Speed { get; set; }
        public int DrawOrder { get; set; }

        // represents a box on a screen where the sprite of Trex is rendered
        public Rectangle CollisionBox
        {
            get
            {
                Rectangle box = new Rectangle(
                                    (int)Math.Round(Position.X),
                                    (int)Math.Round(Position.Y),
                                    COLLISION_SPRITE_WIDTH,
                                    COLLISION_SPRITE_HEIGHT);
                box.Inflate(-COLLISION_BOX_INSET, -COLLISION_BOX_INSET);

                if (State == TrexState.Ducking)
                {
                    // Decrease the size of the collision box when ducking
                    box.Y += DUCK_COLLISION_REDUCTION;
                    box.Height -= DUCK_COLLISION_REDUCTION;
                }

                return box;
            }
        } 
            
            
        // Instead o having 'Sprite' as a parameter, we can put a 'spriteSheet' as parameter and initialize the Trex Sprite within constructor
        // if we had 'Sprite' as parameter, then we would need to initialize Trex Sprite somewhere else which makes less sense
        public TRex(Texture2D spriteSheet, Vector2 position, SoundEffect jumpSound)
        {
            Position = position;

            _idleTrexBackgroundSprite = new Sprite(spriteSheet, IDLE_BACKGROUND_SPRITE_POS_X, IDLE_BACKGROUND_SPRITE_POS_Y,
                IDLE_BACKGROUND_SPRITE_WIDTH, IDLE_BACKGROUND_SPRITE_HEIGHT);
            _idleSprite = new Sprite(spriteSheet, IDLE_BACKGROUND_SPRITE_POS_X, IDLE_BACKGROUND_SPRITE_POS_Y, IDLE_BACKGROUND_SPRITE_WIDTH, IDLE_BACKGROUND_SPRITE_HEIGHT);
            // Here we take Trex sprite with closed eyes (hence, X position is + width)
            _idleBlinkSprite = new Sprite(spriteSheet, IDLE_BACKGROUND_SPRITE_POS_X, IDLE_BACKGROUND_SPRITE_POS_Y, IDLE_BACKGROUND_SPRITE_WIDTH, IDLE_BACKGROUND_SPRITE_HEIGHT);
            _runSpriteOne = new Sprite(spriteSheet, RUNNING_SPRITE_ONE_POS_X, RUNNING_SPRITE_ONE_POS_Y, RUNNING_SPRITE_ONE_WIDTH, RUNNING_SPRITE_ONE_HEIGHT);
            _runSpriteTwo = new Sprite(spriteSheet, RUNNING_SPRITE_TWO_POS_X, RUNNING_SPRITE_TWO_POS_Y, RUNNING_SPRITE_TWO_WIDTH, RUNNING_SPRITE_TWO_HEIGHT);
            
            _duckSpriteOne = new Sprite(spriteSheet, DUCKING_SPRITE_ONE_POS_X, DUCKING_SPRITE_ONE_POS_Y, DUCKING_SPRITE_ONE_WIDTH, DUCKING_SPRITE_ONE_HEIGHT);
            _duckSpriteTwo = new Sprite(spriteSheet, DUCKING_SPRITE_TWO_POS_X, DUCKING_SPRITE_TWO_POS_Y, DUCKING_SPRITE_TWO_WIDTH, DUCKING_SPRITE_TWO_HEIGHT);

            // When TRex class is initiated, make it Idle state first
            State = TrexState.Idle;
            _jumpSound = jumpSound;

            _random = new Random();

           
            _blinkAnimation = new SpriteAnimation();
            CreateBlinkAnimation();
            _blinkAnimation.Play();

            _runAnimation = new SpriteAnimation();
            CreateRunAnimation();

            _duckAnimation = new SpriteAnimation();
            CreateDuckAnimation();

            _deadSprite = new Sprite(spriteSheet, DEAD_SPRITE_POS_X, DEAD_SPRITE_POS_Y, DEAD_SPRITE_WIDTH, DEAD_SPRITE_HEIGHT);
            IsAlive = true;

            _startPosX = position.X;
            _startPosY = position.Y;

        }

        public void Initialize()
        {

            Speed = START_SPEED;
            State = TrexState.Running;
            IsAlive = true;
            Position = new Vector2(_startPosX, _startPosY);
            MOVE_LEFT_RIGHT_ACCELERATION = 60f;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            if (IsAlive)
            {
                // this will draw the TRex with a bit of background dirt
                if (State == TrexState.Idle)
                {
                    _idleTrexBackgroundSprite.Draw(spriteBatch, Position);
                    _blinkAnimation.Draw(spriteBatch, Position);

                }
                else if (State == TrexState.Jumping || State == TrexState.Falling)
                {
                    _idleSprite.Draw(spriteBatch, Position);
                }
                else if (State == TrexState.Running)
                {
                    _runAnimation.Draw(spriteBatch, Position);
                }
                else if (State == TrexState.Ducking)
                {
                    _duckAnimation.Draw(spriteBatch, Position);
                    
                }
                else if (State == TrexState.Canons && (!_moveLeft && !_moveRight))
                {
                    _idleTrexBackgroundSprite.Draw(spriteBatch, Position);
                }
                else if (State == TrexState.Canons && (_moveLeft || _moveRight))
                {
                    _runAnimation.Draw(spriteBatch, Position);
                }
            }

            else
            {
                _deadSprite.Draw(spriteBatch, Position);
            }
            
        }

        public void Update(GameTime gameTime)
        {
            // Now we need to update our idle animation
            if(State == TrexState.Idle)
            {
               
                if (!_blinkAnimation.IsPlaying)
                {
                    CreateBlinkAnimation();
                    _blinkAnimation.Play();
                }

                _blinkAnimation.Update(gameTime);
            }

            else if (State == TrexState.Jumping || State == TrexState.Falling)
            {
                // so here every time update method is called the Y position will be updated. multiply by elapsed time for frame independence
                // and add _dropVelocity which would not be zero when Drop() method is called
                Position = new Vector2(Position.X, Position.Y + _verticalVelocity * (float) gameTime.ElapsedGameTime.TotalSeconds + _dropVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
                // decrease velocity so that we don't fly away
                _verticalVelocity += GRAVITY * (float) gameTime.ElapsedGameTime.TotalSeconds;

                // when velocity is more than 0 it means we are falling. change the state so that we don't continue canceling jump when falling
                if (_verticalVelocity >= 0)
                    State = TrexState.Falling;

                // if Trex's sprite is by any chance lower than its starting position then it means jump is over  
                if (Position.Y >= _startPosY)
                {
                    Position = new Vector2(Position.X, _startPosY);
                    _verticalVelocity = 0;
                    if (_isCannonGame)
                        State = TrexState.Canons;
                    else
                        State = TrexState.Running;

                    // everytime Trex finish jumping the event will be called
                    OnJumpComplete();
                }
                    
            }

            else if (State == TrexState.Running)
            {
                _runAnimation.Play();
                _runAnimation.Update(gameTime);
            }

            else if (State == TrexState.Ducking)
            {
                if (!_isCannonGame)
                {
                    _duckAnimation.Play();
                    _duckAnimation.Update(gameTime);
                }
                else if (_isCannonGame && (_moveLeft || _moveRight))
                {
                    _duckAnimation.Play();
                    _duckAnimation.Update(gameTime);
                }
                else
                {
                    _duckAnimation.Stop();
                }
            }

            if (State != TrexState.Idle)
                Speed += ACCELERATION_PPS_PER_SECOND * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Speed > MAX_SPEED)
                Speed = MAX_SPEED;

            // do something when moveRight is true
            if (State != TrexState.Idle && _moveRight)
            {
                Position = new Vector2(Math.Min(Position.X + MOVE_LEFT_RIGHT_ACCELERATION * (float)gameTime.ElapsedGameTime.TotalSeconds, TRexRunnerGame.GAME_WINDOW_WIDTH), Position.Y);
                if (_isCannonGame)
                {
                    _runAnimation.Play();
                    _runAnimation.Update(gameTime);
                }
            }

            // do something when moveLeft is true
            if (State != TrexState.Idle && _moveLeft)
            {
                Position = new Vector2(Math.Max(Position.X - MOVE_LEFT_RIGHT_ACCELERATION * (float)gameTime.ElapsedGameTime.TotalSeconds, _startPosX), Position.Y);
                if (_isCannonGame)
                {
                    _runAnimation.Play();
                    _runAnimation.Update(gameTime);
                }
            }

            // set dropVelocity to 0 on every frame
            _dropVelocity = 0;
        }

        private void CreateBlinkAnimation()
        {
            _blinkAnimation.Clear();
            // we don't want TRex to constantly blink. just once
            _blinkAnimation.ShouldLoop = false;

            // NextDouble generates the value between 0 and 1, then we multiply by 10-2=8, which gibes value between 0 and 8 and then add
            // our minimum value of 2 to get result between 2 and 10. 
            double blinkTimeStamp = BLINK_ANIMATION_RANDOM_MIN + _random.NextDouble() * (BLINK_ANIMATION_RANDOM_MAX - BLINK_ANIMATION_RANDOM_MIN);
            // the first frame would start at 0 seconds. _idleSprite was initiated in constructor
            _blinkAnimation.AddFrame(_idleSprite, 0);
            // second frame is 1/20f._idleBlink Sprite was initiated in constructor.
            _blinkAnimation.AddFrame(_idleBlinkSprite, (float) blinkTimeStamp);
            // another frame to indicate the end of the animation or in other word, how long animation should last
            _blinkAnimation.AddFrame(_idleSprite, (float) blinkTimeStamp + BLINK_ANIMATION_EYE_CLOSE_TIME);
    
        }

        private void CreateRunAnimation()
        {
            
            _runAnimation.ShouldLoop = true;

            // the first frame would start at 0 seconds. _runSpriteOne was initiated in constructor
            _runAnimation.AddFrame(_runSpriteOne, 0);
            // second frame is 1/20f._RunSpriteTwo was initiated in constructor.
            _runAnimation.AddFrame(_runSpriteTwo, RUN_ANIMATION_FRAME_RATE);
            // another frame to indicate the end of the animation or in other word, how long animation should last
            _runAnimation.AddFrame(_runSpriteOne, RUN_ANIMATION_FRAME_RATE * 2);

        }

        private void CreateDuckAnimation()
        {

            _duckAnimation.ShouldLoop = true;

            // the first frame would start at 0 seconds. _runSpriteOne was initiated in constructor
            _duckAnimation.AddFrame(_duckSpriteOne, 0);
            // second frame is 1/20f._RunSpriteTwo was initiated in constructor.
            _duckAnimation.AddFrame(_duckSpriteTwo, DUCK_ANIMATION_FRAME_RATE);
            // another frame to indicate the end of the animation or in other word, how long animation should last
            _duckAnimation.AddFrame(_duckSpriteOne, DUCK_ANIMATION_FRAME_RATE * 2);

        }

        public bool BeginJump()
        {
            if (State == TrexState.Jumping || State == TrexState.Falling)
                return false;

            _jumpSound.Play();

            State = TrexState.Jumping;

            _verticalVelocity = JUMP_START_VELOCITY;

            return true;
        }

        public bool CancelJump()
        {
            // if Trex is not currently jumping or the altitute is less than his min jump height, we shouldn't be able to cancel the jump
            if (State != TrexState.Jumping || _startPosY - Position.Y < MIN_JUMP_HEIGHT)
                return false;

            // Not needed since in Update method we set state to 'Falling' when velocity is positive
            // State = TrexState.Falling;
            
            // this would make the sprite halt in the air or get the smooth CANCEL_JUMP_VELOCITY, but since there is gravity, sprite will go down
            _verticalVelocity = _verticalVelocity < CANCEL_JUMP_VELOCITY ? CANCEL_JUMP_VELOCITY : 0;
                
            
            return true;
        }

        public bool Duck()
        {
            // we cannot duck if we are in the air
            if (State == TrexState.Jumping || State == TrexState.Falling)
            {
                return false;
            }

            State = TrexState.Ducking;

            return true;
        }

        public bool GetUp()
        {
            // if trex is not ducking, then there is no way to get up
            if (State != TrexState.Ducking)
            {
                return false;
            }

            State = TrexState.Running;

            return true;
        }

        public bool Drop()
        {
            // We can only fast drop when in midair, so if any other state -> return false
            if (State != TrexState.Falling && State != TrexState.Jumping)
            {
                return false;
                  
            }
            
            State = TrexState.Falling;
            _dropVelocity = DROP_VELOCITY;

            return true;
        }

        protected virtual void OnJumpComplete()
        {

            // with this we can raise an events and clients can subscribe to this event
            EventHandler handler = JumpComplete;
            handler?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnDied()
        {
            EventHandler handler = Died;
            handler?.Invoke(this, EventArgs.Empty);
        }

        public bool Die()
        {
            if (!IsAlive)
                return false;

            State = TrexState.Idle;
            Speed = 0; 

            IsAlive = false;

            // when trex dies then this even is triggered
            OnDied();
            return true;
        }

        public void MoveRight()
        {
            _moveRight = true;
        }

        public void StopMoveRight()
        {
            _moveRight = false;
            
        }

        public void MoveLeft()
        {
            _moveLeft = true;
        }

        public void StopMoveLeft()
        {
            _moveLeft = false;
           
            
        }

        public void SetCannonGameTrue()
        {
            _isCannonGame = true;
        }

        public void SetCannonGameFalse()
        {
            _isCannonGame = false;
        }

    }
} 
