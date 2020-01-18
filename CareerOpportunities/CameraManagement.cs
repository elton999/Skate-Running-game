using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace CareerOpportunities
{
    public class CameraManagement
    {

        public float TimeShake;
        private static readonly Random getrandom = new Random();
        public Vector2 InitialPosition;
        private Vector2 ScreemSize;
        private Vector2 Position;
        public Vector2 TargetPosition;
        public float shakeMagnitude = 0.3f;
        public float Zoom { get; set; }

        public CameraManagement()
        {
            this.Position = new Vector2(0, 0);
            this.TargetPosition = new Vector2(0, 0);
            this.Zoom = 1f;
            this.InitialPosition = new Vector2(0, 0);
        }

        public void Update(GameTime gameTime, Vector2 targetPosition, Vector2 screemSize)
        {
            this.ScreemSize = screemSize;
            this.TargetPosition = targetPosition;
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.UpdateShake(delta);
        }

        private void UpdateShake(float delta)
        {
            if (this.TimeShake > 0)
            {
                lock (getrandom)
                {
                    int randomX = getrandom.Next(5);
                    int randomY = getrandom.Next(5);

                    this.Position = new Vector2(
                        randomX + this.InitialPosition.X * this.shakeMagnitude,
                        randomY + this.InitialPosition.Y * this.shakeMagnitude
                    );

                    this.TimeShake -= 1;
                }
            }
            else
            {
                this.Position = new Vector2(0,0);
            }
            
        }

        public Matrix transformMatrix()
        {
            return Matrix.CreateScale(Zoom, Zoom, 1.0f) * Matrix.CreateTranslation(this.Position.X, this.Position.Y, 0);
        }
    }
}
