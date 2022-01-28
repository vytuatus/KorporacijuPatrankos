using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TRexGame.Graphics
{
    public class Sprite
    {
        public Texture2D Texture { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public Color TintColor { get; set; } = Color.White;

        // Constructor to initialize the Sprite with some values passed in
        public Sprite(Texture2D texture, int x, int y, int width, int height)
        {
            Texture = texture;
            X = x;
            Y = y;
            Height = height;
            Width = width;
        }

        /// <summary>
        /// This method draws a spriteBatch on the game screen
        /// </summary>
        /// <param name="spriteBath">holds sprites that need to be drawn on the screen</param>
        /// <param name="position">where to draw on the screen</param>
        public void Draw(SpriteBatch spriteBath, Vector2 position)
        {
            spriteBath.Draw(Texture, position, new Rectangle(X, Y, Width, Height), TintColor);
        }
    }
}
