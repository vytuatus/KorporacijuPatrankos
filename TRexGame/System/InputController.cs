using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using TRexGame.Entities;

namespace TRexGame.System
{
    public class InputController
    {
        private bool _isBlocked;

        private TRex _trex;
        private KeyboardState _previousKeyboardState;

        public InputController(TRex trex)
        {
            _trex = trex;
        }

        public void ProcessControls(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            // if true, then don't process the inputs since they will be processed by another class
            if (!_isBlocked)
            {

                bool isRightKeyPressed = keyboardState.IsKeyDown(Keys.Right);
                bool wasRightKeyPressed = _previousKeyboardState.IsKeyDown(Keys.Right);
                bool isLeftKeyPressed = keyboardState.IsKeyDown(Keys.Left);
                bool wasLeftKeyPressed = _previousKeyboardState.IsKeyDown(Keys.Left);


                bool isJumpKeyPressed = keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.Space);
                bool wasJumpKeyPressed = _previousKeyboardState.IsKeyDown(Keys.Up) || _previousKeyboardState.IsKeyDown(Keys.Space);

                // if previous keyboard state is not UP and current one is UP, then we jump
                if (!wasJumpKeyPressed && isJumpKeyPressed)
                {
                    // if we do anything but jumping, then jump. If we are currently jumping - we continue to jump
                    if (_trex.State != TrexState.Jumping)
                        _trex.BeginJump();

                }
                // if up key is not pressed anymore and Trex is in jumping state, we can try to cancel the jump
                else if (!isJumpKeyPressed && _trex.State == TrexState.Jumping)
                {
                    _trex.CancelJump();
                }

                else if (keyboardState.IsKeyDown(Keys.Down))
                {
                    // if we are in mid air, then pressing down key should result in trex dropping
                    if (_trex.State == TrexState.Jumping || _trex.State == TrexState.Falling)
                        _trex.Drop();
                    // otherwise if we are on ground, we should simply duck
                    else
                        _trex.Duck();
                }
                // if Trex is ducking and key down is not pressed, then trex needs to get up
                else if (_trex.State == TrexState.Ducking && !keyboardState.IsKeyDown(Keys.Down))
                {
                    _trex.GetUp();
                }

                if (!wasRightKeyPressed && isRightKeyPressed)
                {
                    _trex.MoveRight();
                }
                else if (wasRightKeyPressed && !isRightKeyPressed)
                {
                    _trex.StopMoveRight();
                }

                if (!wasLeftKeyPressed && isLeftKeyPressed)
                {
                    _trex.MoveLeft();
                }
                else if (wasLeftKeyPressed && !isLeftKeyPressed)
                {
                    _trex.StopMoveLeft();
                }


            }

            _previousKeyboardState = keyboardState;

            _isBlocked = false;


        }

       
        public void BlockInputTemporarily()
        {
            _isBlocked = true;
        }
    }
}
