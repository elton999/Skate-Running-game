using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CareerOpportunities.Hud
{
    public class Score : GameObject
    {

        ContentManager Content;
        Vector2 InitialPosition;
        float Transparent = 1;

        public Score(ContentManager Content, int Scale, Vector2 Position, bool Coin = true)
        {
            this.Content = Content;
            this.Scale = Scale;
            if(Coin) this.Sprite = Content.Load<Texture2D>("Sprites/coinPlus");
            else this.Sprite = Content.Load<Texture2D>("Sprites/heartPlus");
            this.InitialPosition = Position;
            this.Position = Position;
        }

        public bool CanDetroy = false;

        public void Update(GameTime gameTime)
        {
            if (this.InitialPosition.Y - this.Position.X < 120 * this.Scale)
            {
                this.Position = new Vector2(this.Position.X, this.Position.Y - (1 * this.Scale));
                if (this.InitialPosition.Y - this.Position.X > 80 * this.Scale) this.Transparent -= 0.1f;
            }
            else this.CanDetroy = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Sprite, this.Position, null, Color.White * this.Transparent, 0, new Vector2(0, 0), this.Scale, SpriteEffects.None, 0f);
        }

    }
}
