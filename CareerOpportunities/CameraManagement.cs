using System;
using Microsoft.Xna.Framework;

namespace CareerOpportunities
{
    public class CameraManagement
    {
        public float scale;

        // Shake
        public  float   TimeShake;
        private static  readonly Random getrandom = new Random();
        public  Vector2 InitialPosition;
        private Vector2 ScreemSize;
        private Vector2 Position;
        public  Vector2 TargetPosition;
        public  float   shakeMagnitude = 0.05f;

        // Zoom
        public  float Zoom { get; set; }
        public  float maxZoom;
        public  float TimeZoom;
        private bool  ZoomIn;
        private bool  ZoomOut;


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
            this.UpdateZoomJump(gameTime);
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
            else this.Position = new Vector2(0,0);
        }

        private float t;
        static  float d           = 600f;
        static  float zoomPower   = 0.1f;
        static  float zoomDefault = 1.0f;

        private float GetZoom
        {
            get => zoomDefault + zoomDefault + zoomPower - (float)(1 + Math.Sqrt((double)((t + d) * (d - t))) / (d / zoomPower)) ;
        }

        public void StartZoomJump()
        {
            this.Zoom = 1;
            this.t = 0;
            this.ZoomIn = true;
        }
        
        private void UpdateZoomJump(GameTime gameTime)
        {
            if (this.ZoomIn)
            {
                if (d > t)
                {
                    t += (float)gameTime.ElapsedGameTime.Milliseconds;
                    this.Zoom = this.GetZoom;
                } else
                {
                    this.ZoomIn  = false;
                    this.ZoomOut = true;
                    this.Zoom    = zoomDefault + zoomPower;
                }
            }
            else if (this.ZoomOut)
            {
                if (t > 0)
                {
                    t -= (float)gameTime.ElapsedGameTime.Milliseconds;
                    this.Zoom = this.GetZoom;
                } else
                {
                    this.ZoomIn  = false;
                    this.ZoomOut = false;
                    this.Zoom    = zoomDefault;
                }
            }

            if (this.Zoom.ToString() == "NaN") this.Zoom = 1.1f;
        }

        public Matrix transformMatrix()
        {
            return Matrix.CreateScale(Zoom, Zoom, 1.0f) * Matrix.CreateRotationZ(0f) * Matrix.CreateTranslation(this.Position.X, this.Position.Y * scale, 0);
        }
    }
}
