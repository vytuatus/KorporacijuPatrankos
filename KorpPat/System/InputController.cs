using KorpPat;
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
       
        private bool _wasRightButtonPressed;
        private bool _wasLeftButtonPressed;
        private bool _wasUpButtonPressed;
        private bool _wasDownButtonPressed;

        private Activity1 _activity1;
        Android.Widget.Button _rightButton;
        Android.Widget.Button _leftButton;
        Android.Widget.Button _upButton;
        Android.Widget.Button _downButton;

        public InputController(TRex trex, Activity1 activity1)
        {
            _activity1 = activity1;
            _trex = trex;
            _rightButton = (Android.Widget.Button) _activity1.FindViewById(KorpPat.Resource.Id.button_right);
            _leftButton = (Android.Widget.Button) _activity1.FindViewById(KorpPat.Resource.Id.button_left);
            _upButton = (Android.Widget.Button) _activity1.FindViewById(KorpPat.Resource.Id.button_up);
            _downButton = (Android.Widget.Button) _activity1.FindViewById(KorpPat.Resource.Id.button_down);
        }

        public void ProcessControls(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            
            
                                 
            // if true, then don't process the inputs since they will be processed by another class
            if (!_isBlocked)
            {

                bool isRightKeyPressed = keyboardState.IsKeyDown(Keys.Right) || _rightButton.Pressed;
                bool wasRightKeyPressed = _previousKeyboardState.IsKeyDown(Keys.Right) || _wasRightButtonPressed;
                bool isLeftKeyPressed = keyboardState.IsKeyDown(Keys.Left) || _leftButton.Pressed;
                bool wasLeftKeyPressed = _previousKeyboardState.IsKeyDown(Keys.Left) || _wasLeftButtonPressed;


                bool isJumpKeyPressed = keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.Space) || _upButton.Pressed;
                bool wasJumpKeyPressed = _previousKeyboardState.IsKeyDown(Keys.Up) || _previousKeyboardState.IsKeyDown(Keys.Space) || _wasUpButtonPressed;

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

                else if (keyboardState.IsKeyDown(Keys.Down) || _downButton.Pressed)
                {
                    // if we are in mid air, then pressing down key should result in trex dropping
                    if (_trex.State == TrexState.Jumping || _trex.State == TrexState.Falling)
                        _trex.Drop();
                    // otherwise if we are on ground, we should simply duck
                    else
                        _trex.Duck();
                }
                // if Trex is ducking and key down is not pressed, then trex needs to get up
                else if (_trex.State == TrexState.Ducking && (!keyboardState.IsKeyDown(Keys.Down) || !_downButton.Pressed))
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
            _wasRightButtonPressed = _rightButton.Pressed;
            _wasLeftButtonPressed = _leftButton.Pressed;
            _wasUpButtonPressed = _upButton.Pressed;
            _wasDownButtonPressed = _downButton.Pressed;

            _isBlocked = false;


        }

       
        public void BlockInputTemporarily()
        {
            _isBlocked = true;
        }
    }
}
