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
    }
}
