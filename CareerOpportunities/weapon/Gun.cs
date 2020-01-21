using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CareerOpportunities.weapon
{
    public class Gun : GameObject
    {

        private List<Bullet> bullets;
        public Vector2 Screem;

        public Gun(int Scale, Texture2D Sprite)
        {
            this.bullets = new List<Bullet>();
            this.Scale = Scale;
            this.Sprite = Sprite;
        }

        public void Update(GameTime gameTime)
        {
            List<Bullet> list_bullet = new List<Bullet>();
            for (int i = 0; i < this.bullets.Count; i++)
            {
                this.bullets[i].Update(gameTime);
                if (this.bullets[i].Position.X < this.Screem.X) list_bullet.Add(this.bullets[i]);
            }
            this.bullets = list_bullet;
        }

        public void Fire(Vector2 Position)
        {
            this.bullets.Add(new Bullet(this.Sprite, this.Scale, Position));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.bullets.Count; i++)
            {
                spriteBatch.Draw(this.bullets[i].Sprite, this.bullets[i].Position, null, Color.White, 0, new Vector2(0, 0), this.Scale, this.bullets[i].spriteEffect, 0f);
            }
        }

    }
}
