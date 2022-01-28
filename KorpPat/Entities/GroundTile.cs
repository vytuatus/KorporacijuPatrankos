using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TRexGame.Graphics;

namespace TRexGame.Entities
{
    // we can move this class later to its own file. doesn't really need to implement IGameEntity. we just need to store position
    // of ground tile Sprite
    public class GroundTile : IGameEntity
    {

        // we don't need a Vector2 property since ground time won't really move in Y axis

        private float _positionY;
        public float PositionX { get; set; }
        public Sprite Sprite { get; }

        public int DrawOrder { get; set; }

        public GroundTile(float positionX, float positionY, Sprite sprite)
        {
            PositionX = positionX;
            Sprite = sprite;
            _positionY = positionY ;
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
