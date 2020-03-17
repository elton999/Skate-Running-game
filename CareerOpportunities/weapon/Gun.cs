using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CareerOpportunities.weapon
{
    public class Gun : GameObject
    {

        private List<Bullet> bullets;
        public Vector2 Screem;
        public Texture2D Light;
        public CameraManagement camera;

        public Gun(int Scale, Texture2D Sprite, Texture2D Light, CameraManagement camera, string jsonFile)
        {
            this.bullets = new List<Bullet>();
            this.Scale = Scale;
            this.camera = camera;
            this.setSprite(Sprite);
            this.setJsonFile(jsonFile);
            this.Light = Light;
        }

        public void Update(GameTime gameTime, Level.Render map)
        {
            List<Bullet> list_bullet = new List<Bullet>();
            this.Body = new Rectangle(new Point(5, 5), new Point(9, 9));
            bool collision = false;
            for (int i = 0; i < this.bullets.Count; i++)
            {
                this.bullets[i].Update(gameTime);
                collision = map.Collision(this.Body, new Vector2( this.bullets[i].Position.X, this.bullets[i].Position.Y ), map.LinePosition(this.bullets[i].Position.Y + (this.Scale * 7)), false);
                if (this.bullets[i].Position.X < this.Screem.X && !collision) list_bullet.Add(this.bullets[i]);
                if (collision)
                {
                    map.CollisionItem(map.CollisionPosition, false, true);
                    map.CollisionPosition = new Vector2(0, 0);
                    camera.TimeShake = 5;
                }
            }
            this.play(gameTime, "idle", AnimationDirection.LOOP);
            this.bullets = list_bullet;
        }

        public void Fire(Vector2 Position)
        {
            this.bullets.Add(new Bullet(this.Sprite, this.Scale, new Vector2(Position.X, Position.Y - (10 * this.Scale))));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            for (int i = 0; i < this.bullets.Count; i++)
            {
                this.DrawAnimation(spriteBatch, this.bullets[i].Position, this.Scale);
            }
        }

    }
}
