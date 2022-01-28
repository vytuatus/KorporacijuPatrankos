using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Text;

namespace TRexGame.Entities
{

    // cannot be instantiated (calling construct doesn't work)
    public abstract class Obstacle : IGameEntity, ICollidable
    {
        public int DrawOrder { get; set; }
        public Vector2 Position { get; protected set; }
        private TRex _trex;

        // this way we can implement this differently in each obstacle
        public abstract Rectangle CollisionBox { get; }

        protected Obstacle(TRex trex, Vector2 position)
        {
            _trex = trex;
            Position = position;
        }

        // since we have obstacles that are sprites or spriteAnimations, each seperate subclass of 'Obstacle' will implement this method seperately
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);        

        // we want to make it virtual so that we could make different implementation in subclasses if needed. Ex. FlyingDino class will probably need PositionX to be changing
        // much faster than what it is implemented below  
        public virtual void Update(GameTime gameTime)
        {
            // current position X minus Trex speed and then use this X position as a new Position.X
            float posX = Position.X - _trex.Speed * (float) gameTime.ElapsedGameTime.TotalSeconds;
            Position = new Vector2(posX, Position.Y);

            CheckCollisions();
        }

        private void CheckCollisions()
        {
            // when we call getter of 'CollisionBox' thans to polymorfizm we will call the getter of "CactusGroup"
            // or other sub-class
            Rectangle obstacleCollisionBox = CollisionBox;
            Rectangle trexCollisionBox = _trex.CollisionBox;

            if (obstacleCollisionBox.Intersects(trexCollisionBox))
            {
                _trex.Die();
            }
        }
    }
}
