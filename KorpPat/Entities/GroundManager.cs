using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRexGame.Graphics;

namespace TRexGame.Entities
{

    public class GroundManager : IGameEntity
    {

        private const float GROUND_TILE_POS_Y = 119;

        // actuall ground sprite width is 1200 but we will devide it in two halfs and span it randomly
        // the numbers represent from where in spriteSheet to take the texture
        private const int SPRITE_WIDTH = 600;
        private const int SPRITE_HEIGHT = 12;
        private const int SPRITE_ONE_POS_X = 2;
        private const int SPRITE_ONE_POS_Y = 54;
        private const int SPRITE_TWO_POS_X = 2 + SPRITE_WIDTH;
        private const int SPRITE_TWO_POS_Y = 54;


        private Texture2D _spriteSheet;
        private readonly EntityManager _entityManager;
        private readonly List<GroundTile> _groundTiles;

        private Sprite _regularSprite;
        private Sprite _bumpySprite;

        private TRex _trex;

        private Random _random;

        public int DrawOrder { get; set; }

        public GroundManager(Texture2D spriteSheet, EntityManager entityManager, TRex trex)
        {
            _spriteSheet = spriteSheet;
            _groundTiles = new List<GroundTile>();
            _entityManager = entityManager;
            _regularSprite = new Sprite(spriteSheet, SPRITE_ONE_POS_X, SPRITE_ONE_POS_Y, SPRITE_WIDTH, SPRITE_HEIGHT);
            _bumpySprite = new Sprite(spriteSheet, SPRITE_TWO_POS_X, SPRITE_TWO_POS_Y, SPRITE_WIDTH, SPRITE_HEIGHT);
            _trex = trex;
            _random = new Random();

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
           // you don't really draw the GroundManager. Since it is a manager - not a groundTile. 
           // you only need groudManager as part of the _entityManager in order to update groundTiles (ex. positionX) 
           foreach(GroundTile gt in _groundTiles)
            {
                gt.Draw(spriteBatch, gameTime);
            }
        }

        // so every time this method is called from TRexRunnerGame Class, the method will loop through the ground tiles and update the
        // Position X for all the ground tales every frame
        public void Update(GameTime gameTime)
        {
            // spawn new tile. thw moment any of the tiles goes to negative X, we will have space on right side of screen since SPRITE_WIDTH
            // will no longer cover the whole game screen

            // First check if there are any tiles, if yes, only then we can get the Max position. If we would call Max on empty list, we
            // will get an exception
            if (_groundTiles.Any())
            {
                float maxPosX = _groundTiles.Max(g => g.PositionX);

                if (maxPosX < 0)
                    SpawnTile(maxPosX);
            }

            // since we cannot remove ground tile during loop. we need new list
            List<GroundTile> tilesToRemove = new List<GroundTile>();

            foreach (GroundTile gt in _groundTiles)
            {

                // trex speed is measured by pixels per second. say its 2pixs/sec. If frames change every 0.25 seconds, then on every frame
                // PositionX will move 2 * 0.25 = 0.5 pixels to the left. So after 1 second (or 4 frames), the positionX will move
                // 2 * 0.25 * 4 = 2 pixel positions to the left
                gt.PositionX -= _trex.Speed * (float) gameTime.ElapsedGameTime.TotalSeconds;
                if (gt.PositionX < -SPRITE_WIDTH)
                {
                    // so here we will remove all ground tiles from entityManager we added during initialize(). However, we will not remove
                    // the groundManager. It will always stay in _entityManager. 
                    
                    // No need to remove groundTile from _entity manager since we have decided to never put it there. 
                    //_entityManager.RemoveEntity(gt);
                    tilesToRemove.Add(gt);
                }
            }

            foreach(GroundTile gt in tilesToRemove)
            {
                _groundTiles.Remove(gt);
            }
        }

        // when we start the game first gould tile should already be spawned. Will be called by TRexRunnerGame class
        public void Initialize()
        {
            // Make sure first that before we initialize, our groundTile list is empty
            _groundTiles.Clear();
            // here if you would put the ground tiles in entityManager you would need to remove them
            // in our case, we simply have a list

            // First tile to appear in the game will always be the regular one
            GroundTile groundTile = CreateRegularTile(0);
            _groundTiles.Add(groundTile);
            // you add groundTile to _entityManager so that you could call the the Draw method. 
            // or you could skip this and simply implement the draw method in ground manager which is done in this case
            //_entityManager.AddEntity(groundTile);
        }

        // responsible for creating the left ground tile without the bumps
        private GroundTile CreateRegularTile(float positionX)
        {
            // PositionX and positionY are the start position where the ground tile should be drawn on the screen
            GroundTile groundTile = new GroundTile(positionX, GROUND_TILE_POS_Y, _regularSprite);
            groundTile.DrawOrder = 100;

            return groundTile;
        }

        // responsible for creating the right ground tile with the bumps
        private GroundTile CreateBumpyTile(float positionX)
        {
            // PositionX and positionY are the start position where the ground tile should be drawn on the screen
            GroundTile groundTile = new GroundTile(positionX, GROUND_TILE_POS_Y, _bumpySprite);
            groundTile.DrawOrder = 100;

            return groundTile;
        }

        private void SpawnTile(float maxPosX)
        {
            // random number between 0 and 1
            double randomNumber = _random.NextDouble();

            float posX = maxPosX + SPRITE_WIDTH;

            GroundTile groundTile;
            if (randomNumber > 0.5)
                groundTile = CreateBumpyTile(posX);
            else
                groundTile = CreateRegularTile(posX);

            _groundTiles.Add(groundTile);
        }
    }
}
