using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CareerOpportunities.weapon
{
    public class Bullet : GameObject
    {

        public float velocity = 120f;

        public Bullet(Texture2D Sprite, int Scale, Vector2 Position)
        {
            this.Sprite = Sprite;
            this.Scale = Scale;
            this.Position = new Vector2(Position.X + (this.Scale * (10 + 26)), Position.Y + (this.Scale * 15));
        }

        public void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.Position = new Vector2(this.Position.X + (Scale * delta * velocity), this.Position.Y);
        }
    }
}
