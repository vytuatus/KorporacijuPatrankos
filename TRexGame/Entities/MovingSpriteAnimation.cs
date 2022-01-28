using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TRexGame.Graphics;
using Microsoft.Xna.Framework.Graphics;


namespace TRexGame.Entities
{
    public enum BackgroundAnimationType
    {
        Normal = 1,
        CannonBegin = 2,
        Cannon = 3,
        CannonEnd = 4
    }

    public class MovingSpriteAnimation: IGameEntity
    {

        public int DrawOrder { get; set; }
        public float PositionX { get; set; }
        private float _positionY;
        public SpriteAnimation _spriteAnimation;
        private TRex _trex;
        public bool _positionXShouldChange = true;
        public BackgroundAnimationType _backgroundAnimationType;

        public MovingSpriteAnimation(TRex trex, SpriteAnimation spriteAnimation, float positionX, float positionY, BackgroundAnimationType backgroundAnimationType)
        {
            _spriteAnimation = spriteAnimation;
            PositionX = positionX;
            _trex = trex;
            _positionY = positionY;
            _backgroundAnimationType = backgroundAnimationType;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _spriteAnimation.Draw(spriteBatch, new Vector2(PositionX, _positionY));
        }

        // we want to make it virtual so that we could make different implementation in subclasses if needed. Ex. FlyingDino class will probably need PositionX to be changing
        // much faster than what it is implemented below  
        public virtual void Update(GameTime gameTime)
        {
            if (_positionXShouldChange)
            {
                // current position X minus Trex speed and then use this X position as a new Position.X
                PositionX = PositionX - _trex.Speed * 0.2f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                PositionX = PositionX;
            }
                
            _spriteAnimation.Update(gameTime);
        }

    }
}
