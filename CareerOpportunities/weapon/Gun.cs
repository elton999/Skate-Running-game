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

        public void Update(GameTime gameTime, Level.Render map)
        {
            List<Bullet> list_bullet = new List<Bullet>();
            this.Body = new Rectangle(new Point(0, 0), new Point(4, 3));
            bool collision = false;
            for (int i = 0; i < this.bullets.Count; i++)
            {
                this.bullets[i].Update(gameTime);
                collision = map.Collision(this.Body, this.bullets[i].Position, map.LinePosition(this.bullets[i].Position.Y), false);
                if (this.bullets[i].Position.X < this.Screem.X && !collision) list_bullet.Add(this.bullets[i]);
                if (collision)
                {
                    map.CollisionItem(map.CollisionPosition, false, true);
                    map.CollisionPosition = new Vector2(0, 0);
                }
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
