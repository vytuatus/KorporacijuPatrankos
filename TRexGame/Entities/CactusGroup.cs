using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TRexGame.Graphics;

namespace TRexGame.Entities
{
    public class CactusGroup : Obstacle, ICollidable
    {
        // groups of the cactuses
        public enum GroupSize
        {
            Small = 0,
            Medium = 1,
            Large = 2
        }

        private const int SMALL_CACTUS_SPRITE_HEIGHT = 36;
        private const int SMALL_CACTUS_SPRITE_WIDTH = 17;
        private const int SMALL_CACTUS_TEXTURE_POS_X = 228;
        private const int SMALL_CACTUS_TEXTURE_POS_Y = 0;

        private const int LARGE_CACTUS_SPRITE_HEIGHT = 51;
        private const int LARGE_CACTUS_SPRITE_WIDTH = 25;
        private const int LARGE_CACTUS_TEXTURE_POS_X = 332;
        private const int LARGE_CACTUS_TEXTURE_POS_Y = 0;
        
        private const int COLLISION_BOX_INSET = 7;

        public override Rectangle CollisionBox
        {
            get
            {
                // inflate to reduce the collision box by couple of pixels
                Rectangle box = new Rectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), Sprite.Width, Sprite.Height);
                box.Inflate(-COLLISION_BOX_INSET, -COLLISION_BOX_INSET);
                return box;
            }
        }
            

        // only getter since it doesn't make sense to change the size of cactus one it has been spawned
        public bool IsLarge { get; }
        public GroupSize Size { get; }
        public Sprite Sprite { get; private set; }

        // this constructor will take parameters and chain call the base constructor and pass the variables to base constructor 
        public CactusGroup(Texture2D spriteSheet, bool isLarge, GroupSize size, TRex trex, Vector2 position) : base(trex, position) 
        {
            IsLarge = isLarge;
            Size = size;
            Sprite = GenerateSprite(spriteSheet);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // simple. we just need to draw a sprite
            Sprite.Draw(spriteBatch, Position);
        }

        private Sprite GenerateSprite(Texture2D spriteSheet)
        {

            Sprite sprite = null;
            if(!IsLarge) // Create small cactus group
            {
                // variable to show how many times to the right we need to go to pick the correct starting coord of the texture
                int offsetX = 0;
                int width = SMALL_CACTUS_SPRITE_WIDTH;

                if (Size == GroupSize.Small)
                {
                    offsetX = 0; // start posX of texture is the same one as the one difined in const field
                    width = SMALL_CACTUS_SPRITE_WIDTH;                
                }
                else if (Size == GroupSize.Medium)
                {     
                    offsetX = 1; // start posX is shifted one small cactus texture to the right
                    width = SMALL_CACTUS_SPRITE_WIDTH * 2;
                }
                else
                {     
                    offsetX = 3; // start posX is shifter 3 small cactuses to the right
                    width = SMALL_CACTUS_SPRITE_WIDTH * 3;
                } 

                sprite = new Sprite
                (   spriteSheet, 
                    SMALL_CACTUS_TEXTURE_POS_X + offsetX * SMALL_CACTUS_SPRITE_WIDTH, 
                    SMALL_CACTUS_TEXTURE_POS_Y, 
                    width, 
                    SMALL_CACTUS_SPRITE_HEIGHT
                );
            }  
            else // Create large cactus group
            {
                // variable to show how many times to the right we need to go to pick the correct starting coord of the texture
                int offsetX = 0;
                int width = LARGE_CACTUS_SPRITE_WIDTH;

                if (Size == GroupSize.Small)
                {
                    offsetX = 0; // start posX of texture is the same one as the one difined in const field
                    width = LARGE_CACTUS_SPRITE_WIDTH;
                }
                else if (Size == GroupSize.Medium)
                {
                    offsetX = 1; // start posX is shifted one large cactus texture to the right
                    width = LARGE_CACTUS_SPRITE_WIDTH * 2;
                }
                else
                {
                    offsetX = 3; // start posX is shifter 3 large cactuses to the right
                    width = LARGE_CACTUS_SPRITE_WIDTH * 3;
                }

                sprite = new Sprite
                (spriteSheet,
                    LARGE_CACTUS_TEXTURE_POS_X + offsetX * LARGE_CACTUS_SPRITE_WIDTH,
                    LARGE_CACTUS_TEXTURE_POS_Y,
                    width,
                    LARGE_CACTUS_SPRITE_HEIGHT
                );

            }

            return sprite;
        }
    }
}
