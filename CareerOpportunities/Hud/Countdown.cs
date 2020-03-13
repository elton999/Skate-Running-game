using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CareerOpportunities.Hud
{
    public class Countdown : GameObject
    {

        private enum animationStates { COUNTDOWN, NONE };
        private animationStates AnimationCurrent;

        public bool isCountdown
        {
            get => this.AnimationCurrent == animationStates.COUNTDOWN;
            set
            {
                if (value) this.AnimationCurrent = animationStates.COUNTDOWN;
                else this.AnimationCurrent = animationStates.NONE;
            }
        }


        public void Update(GameTime gameTime)
        {
            if (this.AnimationCurrent == animationStates.COUNTDOWN)
            {
                this.play(gameTime, "Count");
                if (this.lastFrame) this.isCountdown = false;
            }

        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.AnimationCurrent != animationStates.NONE)
            {
                this.DrawAnimation(spriteBatch, Position, Scale);
            }
        }

    }
}
