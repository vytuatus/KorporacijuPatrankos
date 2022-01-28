using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TRexGame.Entities
{
    public abstract class SkyObject : IGameEntity
    {
        // we will use Trex speed to calculate the speeds of different SkyObjects
        protected readonly TRex _trex;
        public int DrawOrder { get; set; }

        public abstract float Speed { get; }
        public Vector2 Position { get; set; }

        protected SkyObject(TRex trex, Vector2 position)
        {
            _trex = trex;
            Position = position;
        }

        // subclasses will implement this themselves. Hence - abstract.
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);

        // virtual so that we can implement some default code and let subclassed implement the rest
        public virtual void Update(GameTime gameTime)
        {
            if(_trex.IsAlive)
                Position = new Vector2(Position.X - Speed * (float)gameTime.ElapsedGameTime.TotalSeconds, Position.Y);
           
        }
    }
}
