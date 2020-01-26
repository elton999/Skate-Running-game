using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace CareerOpportunities.Controller
{
    public class Input
    {
        public bool UsingGamePad;
        public enum Button {
            RIGHT,
            LEFT,
            UP,
            DOWN,
            JUMP,
            FIRE,
            CONFIRM,
            ESC,
        }

        public enum GamePadButton
        {
            NONE,
            // RIGHT_THUMB_BUTTON,
            // LEFT_THUMB_BUTTON,
            // RIGHT_THUMB_STICKS,
            // LEFT_THUMB_STICKS,
            RIGHT_DPAD,
            LEFT_DPAD,
            UP_DPAD,
            DOWN_DPAD,
            A,
            B,
            X,
            Y,
            // RIGHT_SHOULDER_BUTTON,
            // LEFT_SHOULDER_BUTTON,
            // RIGHT_TRIGGER_BUTTON,
            // LEFT_TRIGGER_BUTTON,
            BACK_BUTTON,
            START_BUTTON,
            
        }
        private List<bool> ButtonsPressed;
        private List<Keys> KeyButtonsStatus = new List<Keys>();

        public Input()
        {
            this.ButtonsPressed = new List<bool>();
            for (int i = 0; i < 8; i ++) {
                this.ButtonsPressed.Add(false);
            }
            this.SetAllKey();
        }

        private void SetAllKey()
        {
            // keyboard
            this.KeyButtonsStatus.Add(Keys.Right);
            this.KeyButtonsStatus.Add(Keys.Left);
            this.KeyButtonsStatus.Add(Keys.Up);
            this.KeyButtonsStatus.Add(Keys.Down);

            this.KeyButtonsStatus.Add(Keys.Z);
            this.KeyButtonsStatus.Add(Keys.X);
            this.KeyButtonsStatus.Add(Keys.Enter);
            this.KeyButtonsStatus.Add(Keys.Escape);
        }


        public bool KeyPress(Input.Button Button, Input.GamePadButton gamePadButton = Input.GamePadButton.NONE)
        {
            this.KeyUp(Button, gamePadButton);
            if (!this.ButtonsPressed[(int)Button]) this.KeyDown(Button, gamePadButton);
            else return false;
            return this.ButtonsPressed[(int)Button];
        }

        public bool KeyDown(Input.Button Button, Input.GamePadButton gamePadButton = Input.GamePadButton.NONE)
        {
            if (Keyboard.GetState().GetPressedKeys().Length > 0) this.UsingGamePad = false;
            if (GamePad.GetState(PlayerIndex.One).IsConnected) this.UsingGamePad = true;

            if (Keyboard.GetState().IsKeyDown(this.KeyButtonsStatus[(int)Button]) || this.GamePadStatus(Button, gamePadButton)) this.ButtonsPressed[(int)Button] = true;
            this.KeyUp(Button);

            return this.ButtonsPressed[(int)Button];
        }

        public bool KeyUp(Input.Button Button, Input.GamePadButton gamePadButton = Input.GamePadButton.NONE)
        {
            if ((Keyboard.GetState().IsKeyUp(this.KeyButtonsStatus[(int)Button]) && !this.UsingGamePad)
             || (!this.GamePadStatus(Button, gamePadButton) && this.UsingGamePad))
                this.ButtonsPressed[(int)Button] = false;
            return !this.ButtonsPressed[(int)Button];
        }


        private bool GamePadStatus(Input.Button Button, Input.GamePadButton gamePadButton)
        {
            GamePadState gamepadInput = GamePad.GetState(PlayerIndex.One);

            //has no alternative button to gamepad
            if (gamePadButton == Input.GamePadButton.NONE)
            {
                switch (Button)
                {
                    case Input.Button.LEFT:
                        if (gamepadInput.DPad.Left == ButtonState.Pressed || gamepadInput.ThumbSticks.Left.X < -0.5f) return true;
                        else return false;
                    case Input.Button.RIGHT:
                        if (gamepadInput.DPad.Right == ButtonState.Pressed || gamepadInput.ThumbSticks.Left.X > 0.5f) return true;
                        else return false;
                    case Input.Button.UP:
                        if (gamepadInput.DPad.Up == ButtonState.Pressed || gamepadInput.ThumbSticks.Left.Y > 0.5f) return true;
                        else return false;
                    case Input.Button.DOWN:
                        if (gamepadInput.DPad.Down == ButtonState.Pressed || gamepadInput.ThumbSticks.Left.Y < -0.5f) return true;
                        else return false;
                    case Input.Button.JUMP:
                        if (gamepadInput.Buttons.B == ButtonState.Pressed) return true;
                        else return false;
                    case Input.Button.FIRE:
                        if (gamepadInput.Buttons.X == ButtonState.Pressed) return true;
                        else return false;
                    case Input.Button.CONFIRM:
                        if (gamepadInput.Buttons.A == ButtonState.Pressed) return true;
                        else return false;
                    case Input.Button.ESC:
                        if (gamepadInput.Buttons.Back == ButtonState.Pressed) return true;
                        else return false;
                    default:
                        return false;
                }
            } else {
                switch (gamePadButton)
                {
                    // DPAD
                    case Input.GamePadButton.LEFT_DPAD:
                        if (gamepadInput.DPad.Left == ButtonState.Pressed) return true;
                        else return false;
                    case Input.GamePadButton.RIGHT_DPAD:
                        if (gamepadInput.DPad.Right == ButtonState.Pressed) return true;
                        else return false;
                    case Input.GamePadButton.UP_DPAD:
                        if (gamepadInput.DPad.Up == ButtonState.Pressed) return true;
                        else return false;
                    case Input.GamePadButton.DOWN_DPAD:
                        if (gamepadInput.DPad.Down == ButtonState.Pressed) return true;
                        else return false;

                    // BUTTONS
                    case Input.GamePadButton.A:
                        if (gamepadInput.Buttons.A == ButtonState.Pressed) return true;
                        else return false;
                    case Input.GamePadButton.B:
                        if (gamepadInput.Buttons.B == ButtonState.Pressed) return true;
                        else return false;
                    case Input.GamePadButton.X:
                        if (gamepadInput.Buttons.X == ButtonState.Pressed) return true;
                        else return false;
                    case Input.GamePadButton.Y:
                        if (gamepadInput.Buttons.Y == ButtonState.Pressed) return true;
                        else return false;

                    // OPTIONS
                    case Input.GamePadButton.BACK_BUTTON:
                        if (gamepadInput.Buttons.Back == ButtonState.Pressed) return true;
                        else return false;
                    case Input.GamePadButton.START_BUTTON:
                        if (gamepadInput.Buttons.Start == ButtonState.Pressed) return true;
                        else return false;
                    default:
                        return false;
                }
            }
            
        }

    }
}
