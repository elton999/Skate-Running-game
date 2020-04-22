using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace CareerOpportunities.weapon
{
    public class Gun : GameObject
    {

        private List<Bullet> bullets;
        private Game1 game;
        public Vector2 Screem;
        public Texture2D Light;
        SoundEffect FireSFX;
        SoundEffect HitSFX;

        public Gun(Game1 game, CameraManagement camera)
        {
            this.game    = game;
            this.bullets = new List<Bullet>();
            this.Scale   = game.scale;

            setSprite(game.Content.Load<Texture2D>("sprites/bullet"));
            setJsonFile(this.game.path + "/Content/sprites/bullet.json");

            this.FireSFX = FireSFX = game.Content.Load<SoundEffect>("Sound/sfx_weapon_shotgun1");
            this.HitSFX = game.Content.Load<SoundEffect>("Sound/sfx_exp_short_hard12");
        }

        public void Update(GameTime gameTime)
        {
            List<Bullet> list_bullet = new List<Bullet>();
            bool collision = false;

            for (int i = 0; i < this.bullets.Count; i++)
            {
                this.bullets[i].Update(gameTime);
                collision = game.Map.Collision(this.bullets[i].Body, this.bullets[i].Position, game.Map.LinePosition(this.bullets[i].Position.Y + (this.Scale * 7)), false);
                if (this.bullets[i].Position.X < this.Screem.X && !collision) list_bullet.Add(this.bullets[i]);
                if (collision)
                {
                    game.Map.CollisionItem(game.Map.CollisionPosition, false, true);
                    game.Map.CollisionPosition = new Vector2(0, 0);
                    game.camera.TimeShake = 5;
                    this.HitSFX.Play();
                }
            }
            this.play(gameTime, "idle", AnimationDirection.LOOP);
            this.bullets = list_bullet;
        }

        public void Fire(Vector2 Position)
        {
            this.bullets.Add(new Bullet(this.Sprite, this.Scale, new Vector2(Position.X, Position.Y - (10 * this.Scale))));
            this.FireSFX.Play();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            for (int i = 0; i < this.bullets.Count; i++)
            {
                this.DrawAnimation(spriteBatch, this.bullets[i].Position, this.Scale);
#if DEBUG
                this.bullets[i].DrawRiggidBody(spriteBatch, game.GraphicsDevice);
#endif
            }
        }

    }
}
