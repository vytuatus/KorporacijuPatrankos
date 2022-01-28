using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TRexGame.Graphics;

namespace TRexGame.Entities
{
    public class Cannon : Obstacle
    {

        public PatrankaShootingDirection PatrankaShootingDirection { get; set; }

        private const int CANNON_FACING_RIGHT_TEXTURE_POS_X = 59;
        private const int CANNON_FACING_RIGHT_TEXTURE_POS_Y = 24;
        private const int CANNON_FACING_RIGHT_SPRITE_WIDTH = 43;
        private const int CANNON_FACING_RIGHT_HEIGHT = 30;

        private const int CANNON_FACING_LEFT_TEXTURE_POS_X = 10;
        private const int CANNON_FACING_LEFT_TEXTURE_POS_Y = 24;
        private const int CANNON_FACING_LEFT_SPRITE_WIDTH = 43;
        private const int CANNON_FACING_LEFT_SPRITE_HEIGHT = 30;

        private const int COLLISION_BOX_INSET = 5;
        public bool ReachedFiringPosition { get; set;  }

        private const int RIGHT_CANNON_DESTINATION_POSITION_X = 50;
        private const int LEFT_CANNON_DESTINATION_POSITION_X = 10;

        public Sprite Sprite { get; private set; }
        public Sprite SpriteFacingLeft { get; private set; }
        private ObstacleManager _obstacleManager;

        public float _PatrankaSpawnTime { get; set; }

        public Cannon(PatrankaShootingDirection patrankaShootingDirection, ObstacleManager obstacleManager, Texture2D spriteSheet, TRex trex, Vector2 position) : base(trex, position)
        {
            if (patrankaShootingDirection == PatrankaShootingDirection.Left)
            {
                Sprite = new Sprite(spriteSheet, CANNON_FACING_LEFT_TEXTURE_POS_X, CANNON_FACING_LEFT_TEXTURE_POS_Y,
                CANNON_FACING_LEFT_SPRITE_WIDTH, CANNON_FACING_LEFT_SPRITE_HEIGHT);
            }

            else if (patrankaShootingDirection == PatrankaShootingDirection.Right)
            {
                Sprite = new Sprite(spriteSheet, CANNON_FACING_RIGHT_TEXTURE_POS_X, CANNON_FACING_RIGHT_TEXTURE_POS_Y,
                CANNON_FACING_RIGHT_SPRITE_WIDTH, CANNON_FACING_RIGHT_HEIGHT);
            }
                  

            _obstacleManager = obstacleManager;
            PatrankaShootingDirection = patrankaShootingDirection;
            _PatrankaSpawnTime = 0;
        }

        public override Rectangle CollisionBox
        {
            get
            {
                Rectangle box = new Rectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), Sprite.Width, Sprite.Height);
                box.Inflate(-COLLISION_BOX_INSET, -COLLISION_BOX_INSET);
                return box;
            }
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Sprite.Draw(spriteBatch, Position);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _PatrankaSpawnTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            float posX = Position.X;
            if (PatrankaShootingDirection == PatrankaShootingDirection.Left)
            {
                
                if (!_obstacleManager._stoppedShootingPatrakas) // if Patrankas are still shooting
                {
                    if (TRexRunnerGame.GAME_WINDOW_WIDTH - Position.X < RIGHT_CANNON_DESTINATION_POSITION_X)
                    {
                        // it means we haven't reached the final target position of where the cannon should be to stop moving
                        posX = Position.X - 10 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    else
                    {
                        // we reached our target position, stop cannon movement
                        posX = Position.X;
                        ReachedFiringPosition = true;
                    }
                }
                else // if Patrankas have stopped shooting
                {
                    posX = Position.X + 22 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                
                
            }
            else if (PatrankaShootingDirection == PatrankaShootingDirection.Right)
            {
                if (!_obstacleManager._stoppedShootingPatrakas) // if Patrankas are still shooting
                {
                    if (Position.X < LEFT_CANNON_DESTINATION_POSITION_X)
                    {
                        posX = Position.X + 10 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    else
                    {
                        ReachedFiringPosition = true;
                        posX = Position.X;
                    }
                }

                else // If patrankas have stopped shooting
                {
                    posX = Position.X - 22 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                
            }

            Position = new Vector2(posX, Position.Y);


        }

    }
}
