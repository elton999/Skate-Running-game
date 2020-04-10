using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CareerOpportunities
{
    public class Transition
    {

        ContentManager Content;
        Texture2D Sprite;
        float start;
        int scale;

        public Transition(ContentManager content, float start, int scale)
        {
            this.Content = content;
            this.Sprite = this.Content.Load<Texture2D>("Effects/transition");
            this.start = start;
            this.scale = scale;
            this.Position = new Vector2( -(this.Sprite.Width * 23 * this.scale / 2), 0);
        }


        private bool Show = false;
        public bool animation = false;

        public void HideScreem()
        {
            this.Show = false;
        }

        public void ShowScreem()
        {
            this.Show = true;
        }

        public void Hide()
        {
            this.Position = new Vector2(-(this.Sprite.Width * 23 * this.scale / 2), 0);
            this.Show = false;
        }

        public bool IsHide
        {
            get => this.Position.X == -(this.Sprite.Width * 23 * this.scale / 2);
        }

        Vector2 Position;

        public void Update(GameTime gameTime)
        {
            animation = false;
            if (this.Show)
            {
                if (this.Position.X < start*this.scale)
                {
                    this.Position.X = this.Position.X + 5* this.scale;
                    animation = true;
                } else this.Position = new Vector2(start * this.scale, 0);
            }
            else
            {
                if (this.Position.X > -(this.Sprite.Width * 23 * this.scale / 2))
                {
                    this.Position.X = this.Position.X - 5 * this.scale;
                    animation = true;
                } else this.Position = new Vector2(-(this.Sprite.Width * 23 * this.scale / 2), 0);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Sprite, this.Position, null, Color.White, 0, new Vector2(0, 0), this.scale * 23, SpriteEffects.None, 0f);
        }

    }
}
