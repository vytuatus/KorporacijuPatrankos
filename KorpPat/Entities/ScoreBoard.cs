using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TRexGame.Entities
{
    public class ScoreBoard : IGameEntity
    {
        private const int TEXTURE_COORDS_NUMBERS_X = 655;
        private const int TEXTURE_COORDS_NUMBERS_Y = 0;
        private const int TEXTURE_COORDS_NUMBERS_WIDTH = 10;
        private const int TEXTURE_COORDS_NUMBERS_HEIGHT = 13;

        private const byte NUMBER_DIGITS_TO_DRAW = 5;
        private const int SCORE_MARGIN = 70;

        private const int TEXTURE_COORDS_HI_X = 755;
        private const int TEXTURE_COORDS_HI_Y = 0;
        private const int TEXTURE_COORDS_HI_WIDTH = 20;
        private const int TEXTURE_COORDS_HI_HEIGHT = 13;
        private const int HI_TEXT_MARGIN = 28;
        private const float SCORE_INCREMENT_MULTIPLIER = 0.025f;

        private const float FLASH_ANIMATION_FRAME_LENGTH = 0.2f;
        private const int FLASH_ANIMATION_FLASH_COUNT = 4;

        private const int MAX_SCORE = 99_999;

        private Texture2D _texture;
        private TRex _trex;

        private bool _isPlayingFlashAnimation;
        private float _flashAnimationTime; // store time in secs that the animation has been playing

        private SoundEffect _scoreSfx;
        
        private double _score;
        public double Score { 
            get => _score; 
            set => _score = Math.Max(0, Math.Min(MAX_SCORE, value)); 
        }

        //retrun score rounded down to integer. always return the score
        // if score was 1.7 the display score would be 1
        public int DisplayScore => (int)Math.Floor(Score);
        public int HighScore { get; set; }

        public bool HasHighScore => HighScore > 0;

        public int DrawOrder => 100;

        public Vector2 Position { get; set; }

        public ScoreBoard(Texture2D texture, Vector2 position, TRex trex, SoundEffect scoreSfx)
        {
            _texture = texture;
            Position = position;
            _trex = trex;
            _scoreSfx = scoreSfx;
        }


        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            if (HasHighScore)
            {
                // Draw "HI" text before highscore margin
                spriteBatch.Draw(_texture, new Vector2(Position.X - HI_TEXT_MARGIN, Position.Y),
                    new Rectangle(TEXTURE_COORDS_HI_X, TEXTURE_COORDS_HI_Y, TEXTURE_COORDS_HI_WIDTH, TEXTURE_COORDS_HI_HEIGHT), Color.White);
                // Draw high score
                DrawScore(spriteBatch, HighScore, Position.X);
            }

            // say flashAnimeTime = 0.3 & Frame length = 0.2, so we get 1.5. if we cast to int we get 1. this is odd.
            // if we do modular of 2 on 1, reminder is not zero. Then it means on odd number we draw score
            // and on even number we don't draw score
            if (!_isPlayingFlashAnimation || (int)(_flashAnimationTime / FLASH_ANIMATION_FRAME_LENGTH) % 2 != 0)
            {
                // if we are not playing animation then just show normal score, if we play animation then show score rounded to the next 100 integer
                int score = !_isPlayingFlashAnimation ? DisplayScore : DisplayScore / 100 * 100;
                // Draw score
                DrawScore(spriteBatch, score, Position.X + SCORE_MARGIN);
            }

        }

        // we can reuse this method to draw score and hight score
        private void DrawScore(SpriteBatch spriteBatch, int score, float startPosX)
        {
            int[] scoreDigits = SplitDigits(score);

            float posX = startPosX;

            foreach (int digit in scoreDigits)
            {
                // get area within texture representing the digit
                Rectangle textureCoords = GetDigitTextureBounds(digit);
                // get the position of the screen where to draw that area
                Vector2 screenPos = new Vector2(posX, Position.Y);
                spriteBatch.Draw(_texture, screenPos, textureCoords, Color.White);

                //  we increase posX so that we could draw next digit slightly to the right of the first one
                posX += TEXTURE_COORDS_NUMBERS_WIDTH;
            }
        }

        public void Update(GameTime gameTime)
        {
            int oldScore = DisplayScore;
            Score += _trex.Speed * SCORE_INCREMENT_MULTIPLIER * gameTime.ElapsedGameTime.TotalSeconds;

            // if animation is not currently playing &
            // if Display score 200, the division will result in 2, if oldScore is 199, the division will result in 1
            if (!_isPlayingFlashAnimation && (DisplayScore / 100 != oldScore / 100))
            {
                _isPlayingFlashAnimation = true;
                _flashAnimationTime = 0;
                _scoreSfx.Play(0.8f, 0, 0);
            }

            if (_isPlayingFlashAnimation)
            {
                // increase time that the animation is playing 
                _flashAnimationTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                // we reached the end of the animation time
                if (_flashAnimationTime >= FLASH_ANIMATION_FRAME_LENGTH * FLASH_ANIMATION_FLASH_COUNT * 2)
                {
                    _isPlayingFlashAnimation = false;
                }
            }



        }

        // get an array of seperate digits from a int number
        private int[] SplitDigits(int input)
        {
            // if 'input' is less than NUMBER_DIGITS... than PadLeft will add aditional caracters on the left side
            // in our case we specified to add '0' caracters
            string inputStr = input.ToString().PadLeft(NUMBER_DIGITS_TO_DRAW, '0');
            int[] result = new int[inputStr.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (int)char.GetNumericValue(inputStr[i]);
            }

            return result;
        }

        // pass in a digit and return exact cordinates of the texture that should be rendered to the screen
        private Rectangle GetDigitTextureBounds(int digit)
        {
            if (digit < 0 || digit > 9)
                throw new ArgumentOutOfRangeException(nameof(digit), "The value of digit must be between 0 and 9");
            // So if we get digit 2 ex. then we would multiply with width which would give back texture that is 2 WIDTHS to the right
            int posX = TEXTURE_COORDS_NUMBERS_X + digit * TEXTURE_COORDS_NUMBERS_WIDTH;
            int posY = TEXTURE_COORDS_NUMBERS_Y;

            return new Rectangle(posX, posY, TEXTURE_COORDS_NUMBERS_WIDTH, TEXTURE_COORDS_NUMBERS_HEIGHT);
        }
    }
}
 