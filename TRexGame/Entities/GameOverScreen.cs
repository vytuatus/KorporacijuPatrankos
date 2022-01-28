using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using TRexGame.Graphics;

namespace TRexGame.Entities
{
    public class GameOverScreen : IGameEntity
    {

        private const int GAME_OVER_TEXTURE_POS_X = 655;
        private const int GAME_OVER_TEXTURE_POS_Y = 14;
        public const int GAME_OVER_SPRITE_WIDTH = 192;
        private const int GAME_OVER_SPRITE_HEIGHT = 14;

        private const int RESTART_BUTTON_TEXTURE_POS_X = 110;
        private const int RESTART_BUTTON_TEXTURE_POS_Y = 68;
        private const int RESTART_BUTTON_SPRITE_WIDTH = 36;
        private const int RESTART_BUTTON_SPRITE_HEIGHT = 32;

        private Sprite _gameOverTextSprite;
        private Sprite _restartButtonSprite;

        private KeyboardState _previousKeybordState;

        private TRexRunnerGame _game;

        public Vector2 Position { get; set; }

        public Vector2 RestartButtonPosition => Position + new Vector2(GAME_OVER_SPRITE_WIDTH / 2 - RESTART_BUTTON_SPRITE_WIDTH / 2, GAME_OVER_SPRITE_HEIGHT + 20);
        private Rectangle RestartButtonBounds
            => new Rectangle(RestartButtonPosition.ToPoint(), new Point(RESTART_BUTTON_SPRITE_WIDTH, RESTART_BUTTON_SPRITE_HEIGHT));
        
        public bool IsEnabled { get; set; }
        public GameOverScreen(Texture2D spriteSheet, TRexRunnerGame game)
        {
            _gameOverTextSprite = new Sprite(spriteSheet, GAME_OVER_TEXTURE_POS_X, GAME_OVER_TEXTURE_POS_Y, GAME_OVER_SPRITE_WIDTH, GAME_OVER_SPRITE_HEIGHT);
            _restartButtonSprite = new Sprite(spriteSheet, RESTART_BUTTON_TEXTURE_POS_X, RESTART_BUTTON_TEXTURE_POS_Y, RESTART_BUTTON_SPRITE_WIDTH, RESTART_BUTTON_SPRITE_HEIGHT);
            _game = game;
        }

        public int DrawOrder => 100;

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // we draw only when trex die
            if (!IsEnabled)
                return;
            _gameOverTextSprite.Draw(spriteBatch, Position);
            _restartButtonSprite.Draw(spriteBatch, RestartButtonPosition);
        }

        public void Update(GameTime gameTime)
        {
            if (!IsEnabled)
                return;

            MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();

            bool isKeyPressed = keyboardState.IsKeyDown(Keys.Space) || keyboardState.IsKeyDown(Keys.Up);
            bool wasKeyPressed = _previousKeybordState.IsKeyDown(Keys.Space) || _previousKeybordState.IsKeyDown(Keys.Up);

            // if key up was previously pressed but now it is pressed then also restart. to fix bug when we run into obstacle while holding space bar
            if ((RestartButtonBounds.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed)
                || (wasKeyPressed && !isKeyPressed))
            {
                _game.Replay();
            }
            _previousKeybordState = keyboardState;
        }
    }
}
