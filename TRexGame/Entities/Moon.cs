using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TRexGame.Graphics;

namespace TRexGame.Entities
{

    public class Moon : SkyObject
    {

        private const int RIGHTMOST_SPRITE_COORDS_X = 624;
        private const int RIGHTMOST_SPRITE_COORDS_Y = 2;
        private const int SPRITE_WIDTH = 20;
        private const int SPRITE_HEIGHT = 40;

        private const int SPRITE_COUNT = 7;


        public readonly IDayNightCycle _dayNightCycle;
        public override float Speed => _trex.Speed * 0.1f;
        private Sprite _sprite;
        public Moon(IDayNightCycle dayNightCycle, Texture2D spriteSheet, TRex trex, Vector2 position) : base(trex, position)
        {
            _dayNightCycle = dayNightCycle;
            _sprite = new Sprite(spriteSheet, RIGHTMOST_SPRITE_COORDS_X, RIGHTMOST_SPRITE_COORDS_Y, SPRITE_WIDTH, SPRITE_HEIGHT);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            UpdateSprite();

            // draw moon when it is night time only
            if(_dayNightCycle.IsNight)
                _sprite.Draw(spriteBatch, Position);
        }

        private void UpdateSprite()
        {
            int spriteIndex = _dayNightCycle.NightCount % SPRITE_COUNT;
            int spriteWidth = SPRITE_WIDTH;
            int spriteHeight = SPRITE_HEIGHT;

            if (spriteIndex == 3) // it is full moon
                spriteWidth *= 2;

            if (spriteIndex >= 3)
                spriteIndex++; //  we are on the right side of full moon, but in order to correctly increment to the next moon sprite, we need to increment the index one more time so we start from left side of the moon

            _sprite.Height = spriteHeight;
            _sprite.Width = spriteWidth;
            _sprite.X = RIGHTMOST_SPRITE_COORDS_X - spriteIndex * SPRITE_WIDTH;
            _sprite.Y = RIGHTMOST_SPRITE_COORDS_Y;
        
        } 
    }
}
