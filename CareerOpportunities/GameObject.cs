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
    }
}
