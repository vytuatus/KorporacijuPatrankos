using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TRexGame.Graphics;

namespace TRexGame.Entities
{
    public class Patranka : Obstacle, ICollidable
    {

        private const int PATRANKA_TEXTURE_POS_X = 110;
        private const int PATRANKA_TEXTURE_POS_Y = 40;
        private const int PATRANKA_SPRITE_WIDTH = 20;
        private const int PATRANKA_SPRITE_HEIGHT = 20;

        private const int COLLISION_BOX_INSET = 5;
        public PatrankaShootingDirection PatrankaShootingDirection { get; set; }


        public override Rectangle CollisionBox
        {
            get
            {
                Rectangle box = new Rectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), Sprite.Width, Sprite.Height);
                box.Inflate(-COLLISION_BOX_INSET, -COLLISION_BOX_INSET);
                return box;
            }
        }

        public Sprite Sprite { get; private set; }

        public Patranka(PatrankaShootingDirection patrankaShootingDirection, Texture2D spriteSheet, TRex trex, Vector2 position) : base(trex, position)
        {

            Sprite = new Sprite(spriteSheet, PATRANKA_TEXTURE_POS_X, PATRANKA_TEXTURE_POS_Y, PATRANKA_SPRITE_WIDTH, PATRANKA_SPRITE_HEIGHT);
            PatrankaShootingDirection = patrankaShootingDirection;
        }


        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // simple. we just need to draw a sprite
            Sprite.Draw(spriteBatch, Position);
        }

        public override void Update(GameTime gameTime)
        {
            // first call the base implementation of Obstacle class
            base.Update(gameTime);

            float posX = Position.X;
            if (PatrankaShootingDirection == PatrankaShootingDirection.Left)
            {
                posX = Position.X - 200 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (PatrankaShootingDirection == PatrankaShootingDirection.Right)
            {
                posX = Position.X + 200 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            Position = new Vector2(posX, Position.Y);

        }
    }
}
