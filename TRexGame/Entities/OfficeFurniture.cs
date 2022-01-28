using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TRexGame.Graphics;

namespace TRexGame.Entities
{
    public class OfficeFurniture : Obstacle, ICollidable
    {

        // groups of the cactuses
        public enum FurnitureType
        {
            GreyDesk = 0,
            TrippleSeat = 1,
            WaterTower = 2,
            SmallBrownDesk = 3,
            GreyFileCabinet = 4,
            BrownFileCabinet = 5,
            YellowChair = 6,
            BigPlant = 7,
            SmallFileCabinet = 8,
            Boss = 9,
            PeopleOverTable = 10

        }

        private const int DEFAULT_FURNITURE_HEIGHT = 68;
        //private const int ANIMATION_OBSTACLE_HEIGHT_OFFSET = 

        private const int GREY_DESK_TEXTURE_POS_X = 21;
        private const int GREY_DESK_TEXTURE_POS_Y = 50;
        private const int GREY_DESK_SPRITE_WIDTH = 60;
        private const int GREY_DESK_SPRITE_HEIGHT = 36;

        private const int TRIPPLE_SEAT_TEXTURE_POS_X = 91;
        private const int TRIPPLE_SEAT_TEXTURE_POS_Y = 53;
        private const int TRIPPLE_SEAT_SPRITE_WIDTH = 66;
        private const int TRIPPLE_SEAT_SPRITE_HEIGHT = 33;

        private const int WATER_TOWER_TEXTURE_POS_X = 175;
        private const int WATER_TOWER_TEXTURE_POS_Y = 22;
        private const int WATER_TOWER_SPRITE_WIDTH = 24;
        private const int WATER_TOWER_SPRITE_HEIGHT = 64;

        private const int SMALL_BROWN_DESK_TEXTURE_POS_X = 214;
        private const int SMALL_BROWN_DESK_TEXTURE_POS_Y = 55;
        private const int SMALL_BROWN_DESK_SPRITE_WIDTH = 22;
        private const int SMALL_BROWN_DESK_SPRITE_HEIGHT = 31;

        private const int GREY_FILE_CABINET_TEXTURE_POS_X = 252;
        private const int GREY_FILE_CABINET_TEXTURE_POS_Y = 18;
        private const int GREY_FILE_CABINET_SPRITE_WIDTH = 31;
        private const int GREY_FILE_CABINET_SPRITE_HEIGHT = 68;

        private const int BROWN_FILE_CABINET_TEXTURE_POS_X = 293;
        private const int BROWN_FILE_CABINET_TEXTURE_POS_Y = 18;
        private const int BROWN_FILE_CABINET_SPRITE_WIDTH = 30;
        private const int BROWN_FILE_CABINET_SPRITE_HEIGHT = 68;

        private const int YELLOW_CHAIR_TEXTURE_POS_X = 340;
        private const int YELLOW_CHAIR_TEXTURE_POS_Y = 61;
        private const int YELLOW_CHAIR_SPRITE_WIDTH = 19;
        private const int YELLOW_CHAIR_SPRITE_HEIGHT = 25;

        private const int BIG_PLANT_TEXTURE_POS_X = 368;
        private const int BIG_PLANT_TEXTURE_POS_Y = 21;
        private const int BIG_PLANT_SPRITE_WIDTH = 35;
        private const int BIG_PLANT_SPRITE_HEIGHT = 66;

        private const int SMALL_FILE_CABINET_TEXTURE_POS_X = 419;
        private const int SMALL_FILE_CABINET_TEXTURE_POS_Y = 37;
        private const int SMALL_FILE_CABINET_SPRITE_WIDTH = 31;
        private const int SMALL_FILE_CABINET_SPRITE_HEIGHT = 49;

        private const int BOSS_ONE_TEXTURE_POS_X = 204;
        private const int BOSS_ONE_TEXTURE_POS_Y = 110;
        private const int BOSS_ONE_SPRITE_WIDTH = 21;
        private const int BOSS_ONE_SPRITE_HEIGHT = 53;

        private const int BOSS_TWO_TEXTURE_POS_X = 235;
        private const int BOSS_TWO_TEXTURE_POS_Y = 110;
        private const int BOSS_TWO_SPRITE_WIDTH = 35;
        private const int BOSS_TWO_SPRITE_HEIGHT = 53;

        private const int BOSS_THREE_TEXTURE_POS_X = 279;
        private const int BOSS_THREE_TEXTURE_POS_Y = 111;
        private const int BOSS_THREE_SPRITE_WIDTH = 34;
        private const int BOSS_THREE_SPRITE_HEIGHT = 51;

        private const int PEOPLE_TALKING_OVER_TABLE_ONE_TEXTURE_POS_X = 16;
        private const int PEOPLE_TALKING_OVER_TABLE_ONE_TEXTURE_POS_Y = 111;
        private const int PEOPLE_TALKING_OVER_TABLE_ONE_SPRITE_WIDTH = 83;
        private const int PEOPLE_TALKING_OVER_TABLE_ONE_SPRITE_HEIGHT = 52;

        private const int PEOPLE_TALKING_OVER_TABLE_TWO_TEXTURE_POS_X = 108;
        private const int PEOPLE_TALKING_OVER_TABLE_TWO_TEXTURE_POS_Y = 111;
        private const int PEOPLE_TALKING_OVER_TABLE_TWO_SPRITE_WIDTH = 83;
        private const int PEOPLE_TALKING_OVER_TABLE_TWO_SPRITE_HEIGHT = 52;


        private const int COLLISION_BOX_INSET = 7;

        public override Rectangle CollisionBox
        {
            get
            {
                // inflate to reduce the collision box by couple of pixels

                Rectangle box;
                if (Sprite != null)
                {
                    box = new Rectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), Sprite.Width, Sprite.Height);
                }
                else
                {
                    box = new Rectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), SpriteAnimation.CurrentFrame.Sprite.Width, SpriteAnimation.CurrentFrame.Sprite.Height);
                }

                box.Inflate(-COLLISION_BOX_INSET, -COLLISION_BOX_INSET);
                return box;
            }
        }


        // only getter since it doesn't make sense to change the size of cactus one it has been spawned

        public FurnitureType Type { get; }
        public Sprite Sprite { get; private set; }
        private SpriteAnimation SpriteAnimation { get; set; }
        private Sprite _bossSpriteA;
        private Sprite _bossSpriteB;
        private Sprite _bossSpriteC;

        private Sprite _peopleOverTableSpriteA;
        private Sprite _peopleOverTableSpriteB;

        // this constructor will take parameters and chain call the base constructor and pass the variables to base constructor 
        public OfficeFurniture(Texture2D spriteSheet, FurnitureType type, TRex trex, Vector2 position) : base(trex, position)
        {

            Type = type;
            _bossSpriteA = new Sprite(spriteSheet, BOSS_ONE_TEXTURE_POS_X, BOSS_ONE_TEXTURE_POS_Y, BOSS_ONE_SPRITE_WIDTH, BOSS_ONE_SPRITE_HEIGHT); ;
            _bossSpriteB = new Sprite(spriteSheet, BOSS_TWO_TEXTURE_POS_X, BOSS_TWO_TEXTURE_POS_Y, BOSS_TWO_SPRITE_WIDTH, BOSS_TWO_SPRITE_HEIGHT);
            _bossSpriteC = new Sprite(spriteSheet, BOSS_THREE_TEXTURE_POS_X, BOSS_THREE_TEXTURE_POS_Y, BOSS_THREE_SPRITE_WIDTH, BOSS_THREE_SPRITE_HEIGHT);

            _peopleOverTableSpriteA = new Sprite(spriteSheet, PEOPLE_TALKING_OVER_TABLE_ONE_TEXTURE_POS_X, PEOPLE_TALKING_OVER_TABLE_ONE_TEXTURE_POS_Y,
                PEOPLE_TALKING_OVER_TABLE_ONE_SPRITE_WIDTH, PEOPLE_TALKING_OVER_TABLE_ONE_SPRITE_HEIGHT); ;
            _peopleOverTableSpriteB = new Sprite(spriteSheet, PEOPLE_TALKING_OVER_TABLE_TWO_TEXTURE_POS_X, PEOPLE_TALKING_OVER_TABLE_TWO_TEXTURE_POS_Y,
                PEOPLE_TALKING_OVER_TABLE_TWO_SPRITE_WIDTH, PEOPLE_TALKING_OVER_TABLE_TWO_SPRITE_HEIGHT);

            Sprite = GenerateSprite(spriteSheet);
            SpriteAnimation = GenerateSpriteAnimation();

            if (Sprite != null)
            {
                Position = new Vector2(position.X, position.Y + (DEFAULT_FURNITURE_HEIGHT - Sprite.Height));

            }
            else // it is a sprite animation, not sprite
            {
                Position = new Vector2(position.X, position.Y + (DEFAULT_FURNITURE_HEIGHT - SpriteAnimation.CurrentFrame.Sprite.Height));
            }


        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            SpriteAnimation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // simple. we just need to draw a sprite
            if (Sprite != null)
                Sprite.Draw(spriteBatch, Position);
            SpriteAnimation.Draw(spriteBatch, Position);
        }

        private Sprite GenerateSprite(Texture2D spriteSheet)
        {

            Sprite sprite = null;


            if (Type == FurnitureType.GreyDesk)
            {
                sprite = new Sprite
                (spriteSheet,
                    GREY_DESK_TEXTURE_POS_X,
                    GREY_DESK_TEXTURE_POS_Y,
                    GREY_DESK_SPRITE_WIDTH,
                    GREY_DESK_SPRITE_HEIGHT
                );
            }
            else if (Type == FurnitureType.TrippleSeat)
            {
                sprite = new Sprite
                (spriteSheet,
                    TRIPPLE_SEAT_TEXTURE_POS_X,
                    TRIPPLE_SEAT_TEXTURE_POS_Y,
                    TRIPPLE_SEAT_SPRITE_WIDTH,
                    TRIPPLE_SEAT_SPRITE_HEIGHT
                );
            }
            else if (Type == FurnitureType.WaterTower)
            {
                sprite = new Sprite
                (spriteSheet,
                    WATER_TOWER_TEXTURE_POS_X,
                    WATER_TOWER_TEXTURE_POS_Y,
                    WATER_TOWER_SPRITE_WIDTH,
                    WATER_TOWER_SPRITE_HEIGHT
                );
            }

            else if (Type == FurnitureType.SmallBrownDesk)
            {
                sprite = new Sprite
                (spriteSheet,
                    SMALL_BROWN_DESK_TEXTURE_POS_X,
                    SMALL_BROWN_DESK_TEXTURE_POS_Y,
                    SMALL_BROWN_DESK_SPRITE_WIDTH,
                    SMALL_BROWN_DESK_SPRITE_HEIGHT
                );
            }
            else if (Type == FurnitureType.GreyFileCabinet)
            {
                sprite = new Sprite
                (spriteSheet,
                    GREY_FILE_CABINET_TEXTURE_POS_X,
                    GREY_FILE_CABINET_TEXTURE_POS_Y,
                    GREY_FILE_CABINET_SPRITE_WIDTH,
                    GREY_FILE_CABINET_SPRITE_HEIGHT
                );
            }

            else if (Type == FurnitureType.BrownFileCabinet)
            {
                sprite = new Sprite
                (spriteSheet,
                    BROWN_FILE_CABINET_TEXTURE_POS_X,
                    BROWN_FILE_CABINET_TEXTURE_POS_Y,
                    BROWN_FILE_CABINET_SPRITE_WIDTH,
                    BROWN_FILE_CABINET_SPRITE_HEIGHT
                );
            }

            else if (Type == FurnitureType.YellowChair)
            {
                sprite = new Sprite
                (spriteSheet,
                    YELLOW_CHAIR_TEXTURE_POS_X,
                    YELLOW_CHAIR_TEXTURE_POS_Y,
                    YELLOW_CHAIR_SPRITE_WIDTH,
                    YELLOW_CHAIR_SPRITE_HEIGHT
                );
            }

            else if (Type == FurnitureType.BigPlant)
            {
                sprite = new Sprite
                (spriteSheet,
                    BIG_PLANT_TEXTURE_POS_X,
                    BIG_PLANT_TEXTURE_POS_Y,
                    BIG_PLANT_SPRITE_WIDTH,
                    BIG_PLANT_SPRITE_HEIGHT
                );
            }

            else if (Type == FurnitureType.SmallFileCabinet)
            {
                sprite = new Sprite
                (spriteSheet,
                    SMALL_FILE_CABINET_TEXTURE_POS_X,
                    SMALL_FILE_CABINET_TEXTURE_POS_Y,
                    SMALL_FILE_CABINET_SPRITE_WIDTH,
                    SMALL_FILE_CABINET_SPRITE_HEIGHT
                );
            }

            return sprite;
        }

        private SpriteAnimation GenerateSpriteAnimation()
        {

            SpriteAnimation = new SpriteAnimation();

            if (Type == FurnitureType.Boss)
            {
                CreateBossAnimation();
            }
            else if (Type == FurnitureType.PeopleOverTable)
            {
                CreatePeopleOverTableAnimation();
            }

            return SpriteAnimation;
        }

        private void CreateBossAnimation()
        {

            SpriteAnimation.ShouldLoop = true;

            // the first frame would start at 0 seconds. _spriteA was initiated in constructor
            SpriteAnimation.AddFrame(_bossSpriteA, 0);
            // second frame is 1/20f._SpriteB was initiated in constructor.
            SpriteAnimation.AddFrame(_bossSpriteB, 0.5f);
            // thrid frame.
            SpriteAnimation.AddFrame(_bossSpriteC, 0.5f * 2);

            // another frame to indicate the end of the animation or in other word, how long animation should last
            SpriteAnimation.AddFrame(_bossSpriteC, 0.5f * 3);
            SpriteAnimation.ShouldLoop = true;
            SpriteAnimation.Play();

        }

        private void CreatePeopleOverTableAnimation()
        {

            SpriteAnimation.ShouldLoop = true;

            // the first frame would start at 0 seconds. _spriteA was initiated in constructor
            SpriteAnimation.AddFrame(_peopleOverTableSpriteA, 0);
            // second frame is 1/20f._SpriteB was initiated in constructor.
            SpriteAnimation.AddFrame(_peopleOverTableSpriteB, 0.5f);

            // another frame to indicate the end of the animation or in other word, how long animation should last
            SpriteAnimation.AddFrame(_peopleOverTableSpriteB, 0.5f * 2);
            SpriteAnimation.ShouldLoop = true;
            SpriteAnimation.Play();

        }

    }
}


    
