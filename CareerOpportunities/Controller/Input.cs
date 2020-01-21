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
            X,
            Z,
            C,
            ESC,
        }
        private List<bool> ButtonsPressed;

        public Input()
        {
            for (int i = 0; i < (int)Input.Button.ESC; i ++) {
                this.ButtonsPressed.Add(false);
            }
        }


        public bool KeyPress(Input.Button Button)
        {
            return this.ButtonsPressed[(int)Button];
        }

        public bool KeyDown(Input.Button Button)
        {
            return this.ButtonsPressed[(int)Button];
        }

        public bool KeyUp(Input.Button Button)
        {
            return this.ButtonsPressed[(int)Button];
        }
    }
}
