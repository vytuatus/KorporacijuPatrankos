using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TRexGame.Graphics;

namespace TRexGame.Entities
{
    public class FlyingDino : Obstacle
    {
        private const float ANIMATION_FRAME_LENGTH = 0.2f;
        private const float VERTICAL_COLLISION_INSET = 10;
        private const float HORIZONTAL_COLLISION_INSET = 6;

        private const int PAPER_PLANE_TEXTURE_ONE_COORDS_X = 45;
        private const int PAPER_PLANE_TEXTURE_ONE_COORDS_Y = 189;
        private const int PAPER_PLANE_TEXTURE_ONE_SPRITE_WIDTH = 53;
        private const int PAPER_PLANE_TEXTURE_ONE_SPRITE_HEIGHT = 23;

        private const int PAPER_PLANE_TEXTURE_TWO_COORDS_X = 112;
        private const int PAPER_PLANE_TEXTURE_TWO_COORDS_Y = 189;
        private const int PAPER_PLANE_TEXTURE_TWO_SPRITE_WIDTH = 53;
        private const int PAPER_PLANE_TEXTURE_TWO_SPRITE_HEIGHT = 23;


        private const int PAPER_TRASH_TEXTURE_ONE_COORDS_X = 190;
        private const int PAPER_TRASH_TEXTURE_ONE_COORDS_Y = 194;
        private const int PAPER_TRASH_TEXTURE_ONE_SPRITE_WIDTH = 16;
        private const int PAPER_TRASH_TEXTURE_ONE_SPRITE_HEIGHT = 16;

        private const int PAPER_TRASH_TEXTURE_TWO_COORDS_X = 211;
        private const int PAPER_TRASH_TEXTURE_TWO_COORDS_Y = 194;
        private const int PAPER_TRASH_TEXTURE_TWO_SPRITE_WIDTH = 16;
        private const int PAPER_TRASH_TEXTURE_TWO_SPRITE_HEIGHT = 16;

        private const int PAPER_TRASH_TEXTURE_THREE_COORDS_X = 245;
        private const int PAPER_TRASH_TEXTURE_THREE_COORDS_Y = 194;
        private const int PAPER_TRASH_TEXTURE_THREE_SPRITE_WIDTH = 16;
        private const int PAPER_TRASH_TEXTURE_THREE_SPRITE_HEIGHT = 16;

        private const int PAPER_TRASH_TEXTURE_FOUR_COORDS_X = 250;
        private const int PAPER_TRASH_TEXTURE_FOUR_COORDS_Y = 194;
        private const int PAPER_TRASH_TEXTURE_FOUR_SPRITE_WIDTH = 16;
        private const int PAPER_TRASH_TEXTURE_FOUR_SPRITE_HEIGHT = 16;

        private const float SPEED_PPS = 80f;
        
        private SpriteAnimation _animation;
        private Sprite _spriteA;
        private Sprite _spriteB;

        private Sprite _paperTrashspriteA;
        private Sprite _paperTrashspriteB;
        private Sprite _paperTrashspriteC;
        private Sprite _paperTrashspriteD;
        

        private TRex _trex;


        public FlyingDino(TRex trex, Vector2 position, Texture2D spriteSheet) : base(trex, position)
        {
            _spriteA = new Sprite(spriteSheet, PAPER_PLANE_TEXTURE_ONE_COORDS_X, PAPER_PLANE_TEXTURE_ONE_COORDS_Y, PAPER_PLANE_TEXTURE_ONE_SPRITE_WIDTH, PAPER_PLANE_TEXTURE_ONE_SPRITE_HEIGHT); ;
            _spriteB = new Sprite(spriteSheet, PAPER_PLANE_TEXTURE_TWO_COORDS_X, PAPER_PLANE_TEXTURE_TWO_COORDS_Y, PAPER_PLANE_TEXTURE_TWO_SPRITE_WIDTH, PAPER_PLANE_TEXTURE_TWO_SPRITE_HEIGHT);
            _paperTrashspriteA = new Sprite(spriteSheet, PAPER_TRASH_TEXTURE_ONE_COORDS_X, PAPER_TRASH_TEXTURE_ONE_COORDS_Y, PAPER_TRASH_TEXTURE_ONE_SPRITE_WIDTH, PAPER_TRASH_TEXTURE_ONE_SPRITE_HEIGHT);
            _paperTrashspriteB = new Sprite(spriteSheet, PAPER_TRASH_TEXTURE_TWO_COORDS_X, PAPER_TRASH_TEXTURE_TWO_COORDS_Y, PAPER_TRASH_TEXTURE_TWO_SPRITE_WIDTH, PAPER_TRASH_TEXTURE_TWO_SPRITE_HEIGHT);
            _paperTrashspriteC = new Sprite(spriteSheet, PAPER_TRASH_TEXTURE_THREE_COORDS_X, PAPER_TRASH_TEXTURE_THREE_COORDS_Y, PAPER_TRASH_TEXTURE_THREE_SPRITE_WIDTH, PAPER_TRASH_TEXTURE_THREE_SPRITE_HEIGHT);
            _paperTrashspriteD = new Sprite(spriteSheet, PAPER_TRASH_TEXTURE_FOUR_COORDS_X, PAPER_TRASH_TEXTURE_FOUR_COORDS_Y, PAPER_TRASH_TEXTURE_FOUR_SPRITE_WIDTH, PAPER_TRASH_TEXTURE_FOUR_SPRITE_HEIGHT);


            _trex = trex;
            _animation = new SpriteAnimation();

            // +1 since the max value is not inclusive. it will spawn a number between 0 and 100
            // (or 60 if min score is not reached and flyingDinoSpawn rate is 0)
            Random random = new Random();
            int rng = random.Next(0, 2);

            if (rng == 0) // spawn cactus

                CreateDinoAnimation();
            else
            {
                CreatePaperTrashAnimation();
            }
        }

        public override Rectangle CollisionBox
        {
            get
            {
                Rectangle collisionBox = new Rectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), PAPER_PLANE_TEXTURE_TWO_SPRITE_WIDTH, PAPER_PLANE_TEXTURE_TWO_SPRITE_HEIGHT);
                collisionBox.Inflate(-HORIZONTAL_COLLISION_INSET, -VERTICAL_COLLISION_INSET); 
                return collisionBox;
            }
        }

        public override void Update(GameTime gameTime)
        {
            // first call the base implementation of Obstacle class
            base.Update(gameTime);

            // additionally we want to update animation
            if(_trex.IsAlive)
            {
                _animation.Update(gameTime);
                // we want flying Dino to move a bit faster at Trex since its flying to it. move SPEED_PPS faster than TRex
                // only manipulate FlyingDino position if Trex alive. otherwise dino will still move when Trex is dead
                Position = new Vector2(Position.X - SPEED_PPS * (float) gameTime.ElapsedGameTime.TotalSeconds, Position.Y);

            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _animation.Draw(spriteBatch, Position);
        }

        private void CreateDinoAnimation()
        {

            _animation.ShouldLoop = true;

            // the first frame would start at 0 seconds. _spriteA was initiated in constructor
            _animation.AddFrame(_spriteA, 0);
            // second frame is 1/20f._SpriteB was initiated in constructor.
            _animation.AddFrame(_spriteB, ANIMATION_FRAME_LENGTH);
            // another frame to indicate the end of the animation or in other word, how long animation should last
            _animation.AddFrame(_spriteB, ANIMATION_FRAME_LENGTH * 2);
            _animation.ShouldLoop = true;
            _animation.Play();

        }

        private void CreatePaperTrashAnimation()
        {

            _animation.ShouldLoop = true;

            // the first frame would start at 0 seconds. _spriteA was initiated in constructor
            _animation.AddFrame(_paperTrashspriteA, 0);
            // second frame is 1/20f._SpriteB was initiated in constructor.
            _animation.AddFrame(_paperTrashspriteB, ANIMATION_FRAME_LENGTH);
            _animation.AddFrame(_paperTrashspriteC, ANIMATION_FRAME_LENGTH * 2);
            _animation.AddFrame(_paperTrashspriteD, ANIMATION_FRAME_LENGTH * 4);
            // another frame to indicate the end of the animation or in other word, how long animation should last
            _animation.AddFrame(_paperTrashspriteD, ANIMATION_FRAME_LENGTH * 4);
            _animation.ShouldLoop = true;
            _animation.Play();

        }
    } 
}
