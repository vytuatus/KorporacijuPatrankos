using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TRexGame.Graphics;

namespace TRexGame.Entities
{
    public class BackgroundTile: IGameEntity
    {

        // we don't need a Vector2 property background tile won't really move in Y axis

        private float _positionY;
        public float PositionX { get; set; }
        public Sprite Sprite { get; }

        public int DrawOrder { get; set; }

        public BackgroundTile(float positionX, float positionY, Sprite sprite)
        {
            PositionX = positionX;
            Sprite = sprite;
            _positionY = positionY;
        }

        public void Update(GameTime gameTime)
        {
            // wont do anything
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Sprite.Draw(spriteBatch, new Vector2(PositionX, _positionY));
        }
    }
}
