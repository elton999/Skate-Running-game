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
        float ScaleFloat;
        private static readonly Random getrandom = new Random();

        public Score(ContentManager Content, int Scale, Vector2 Position, bool Coin = true)
        {
            float size = 1f;
            float maxX = 10f;
            lock (getrandom)
            {
                size = getrandom.Next(10) / 10f;
                maxX = getrandom.Next(10) * Scale;

                this.Content = Content;
                this.ScaleFloat = Scale * size;
                if (Coin) this.Sprite = Content.Load<Texture2D>("Sprites/coinPlus");
                else this.Sprite = Content.Load<Texture2D>("Sprites/heartPlus");
                this.InitialPosition = new Vector2(Position.X + maxX, Position.Y);
                this.Position = new Vector2(Position.X + maxX, Position.Y);
            }
        }

        public bool CanDetroy = false;

        public void Update(GameTime gameTime)
        {
            if (this.Position.Y <= this.InitialPosition.Y + (120 * this.ScaleFloat))
            {
                this.Position = new Vector2(this.Position.X, this.Position.Y - (1 * this.ScaleFloat));
                this.Transparent -= 0.01f;
            }
            else this.CanDetroy = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Sprite, this.Position, null, Color.White * this.Transparent, 0, new Vector2(0, 0), this.ScaleFloat, SpriteEffects.None, 0f);
        }

    }
}
