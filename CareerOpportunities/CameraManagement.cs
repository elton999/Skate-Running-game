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
        // Shake
        public float TimeShake;
        private static readonly Random getrandom = new Random();
        public Vector2 InitialPosition;
        private Vector2 ScreemSize;
        private Vector2 Position;
        public Vector2 TargetPosition;
        public float shakeMagnitude = 0.3f;

        // Zoom
        public float Zoom { get; set; }
        public float maxZoom;
        public float TimeZoom;
        private float TimeZoomCurrent;
        private bool ZoomIn;
        private bool ZoomOut;


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
            this.UpdateZoomJump(delta);
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


        public void StartZoomJump()
        {
            this.TimeZoom = 0.4f;
            this.TimeZoomCurrent = this.TimeZoom;
            this.ZoomIn = true;
        }

        private void UpdateZoomJump(float delta)
        {
            this.TimeZoomCurrent -= delta;
            float alpha = MathHelper.ToRadians(this.TimeZoom - 0.2f);

            if (this.TimeZoomCurrent > 0)
            {
                if (this.ZoomIn) this.Zoom += (float)(1 * this.TimeZoomCurrent * Math.Sin(alpha));
                else if (this.ZoomOut) this.Zoom -= (float)(1 * this.TimeZoomCurrent * Math.Sin(alpha));
            }

            if (this.TimeZoomCurrent <= 0)
            {
                if (this.ZoomIn)
                {
                    this.ZoomIn = false;
                    this.ZoomOut = true;
                    this.TimeZoomCurrent = this.TimeZoom;

                } else if (this.ZoomOut) this.ZoomOut = false;
            }
        }

        public Matrix transformMatrix()
        {
            return Matrix.CreateScale(Zoom, Zoom, 1.0f) * Matrix.CreateTranslation(this.Position.X, this.Position.Y, 0);
        }
    }
}
