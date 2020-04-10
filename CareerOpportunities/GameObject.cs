using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CareerOpportunities
{
    public class GameObject : Animation
    {
        public Vector2 Position;
        public Rectangle Body;
        public Texture2D Sprite;
        public Color SpriteColor;
        public int Scale;
        public SpriteEffects spriteEffect = SpriteEffects.None;


        public void PostionToCenter(Vector2 ScreenSize, Vector2 SpriteSize)
        {
            float screemX = ScreenSize.X / 2;
            float screemY = ScreenSize.Y / 2;

            this.Position = new Vector2(screemX - (SpriteSize.X * this.Scale / 2), screemY - (SpriteSize.Y * this.Scale / 2));
        }

#if DEBUG
        public void DrawRiggidBody(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            Texture2D rect = new Texture2D( graphicsDevice, this.Body.Size.X, this.Body.Size.Y);

            Color[] data = new Color[this.Body.Size.X * this.Body.Size.Y];

            float X = 1;
            float Y = 1;

            for (int i = 0; i < data.Length; ++i)
            {
                if (Y == 1 || Y == this.Body.Size.Y || X == 1 || X == this.Body.Size.X) data[i] = Color.LightGreen;

                if (X == this.Body.Size.X)
                {
                    X = 1;
                    Y++;
                }
                else X++;
            }
            rect.SetData(data);
            Vector2 positionbody = new Vector2(this.Body.Location.X, this.Body.Location.Y);

            spriteBatch.Draw(rect, positionbody, null, Color.White, 0, new Vector2(0, 0), this.Scale, SpriteEffects.None, 0f);
        }
#endif
    }
}
