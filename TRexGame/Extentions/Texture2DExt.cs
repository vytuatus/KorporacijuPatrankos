using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRexGame.Extentions
{
    public static class Texture2DExt
    {

        public static Texture2D InvertColors(this Texture2D texture, Color? excludeColor = null)
        {
            // extention methods are simply static methods
            // We can call this 'InvertColors' method as if it was an instance method of Texture2D, but it is actually a static method
            // Ex. texture.InvertColors() == Texture2DExt.InvertColors(texture);
            if (texture is null)
                throw new ArgumentNullException(nameof(texture));

            // we need to get pixel data of the texture and invert all pixel colors and set new texture
            Texture2D result = new Texture2D(texture.GraphicsDevice, texture.Width, texture.Height);

            Color[] pixelData = new Color[texture.Width * texture.Height];
            // will set each element of color array to the respective pixel color of that coordinate
            // we have array that contains all of the colors for each pixel
            texture.GetData(pixelData);

            // now invert colors
            // itrate over each element in the pixelData array, apply below function and return new color and covert to new array
            Color[] invertedPixelData = pixelData.Select(p => excludeColor.HasValue && p == excludeColor ? p : new Color(255 - p.R, 255 - p.G, 255 - p.B, p.A)).ToArray();

            result.SetData(invertedPixelData);

            return result;
        }
    }
}
