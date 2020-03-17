using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CareerOpportunities.Hud
{
    public class Countdown : GameObject
    {

        private enum animationStates { COUNTDOWN, GO, NONE };
        private animationStates AnimationCurrent;

        public bool isCountdown
        {
            get => this.AnimationCurrent == animationStates.COUNTDOWN;
        }


        public void Update(GameTime gameTime)
        {
            if (this.AnimationCurrent == animationStates.COUNTDOWN)
            {
                if (this.lastFrame) this.AnimationCurrent = animationStates.GO;
                else this.play(gameTime, "Count");
            } else if (this.AnimationCurrent == animationStates.GO)
            {
                this.play(gameTime, "Go");
                if (this.lastFrame) this.AnimationCurrent = animationStates.NONE;
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
