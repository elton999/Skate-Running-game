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
    public class PlayerController
    {
        public Texture2D sprite;

        public Vector2 Position;
        public Rectangle Body;
        public bool canMoveVertical;
        public int CurrentVerticalLine;
        public float PlayerVerticalVelocity;
        public int PlayerHorizontalVelocity;

        public int scale;
        private int[] Lines;
        private int PreviousVerticalLine;


        public PlayerController (Texture2D sprite, int scale, int BufferHeight)
        {
            this.sprite = sprite;
            this.scale = scale;

            this.CurrentVerticalLine = 0;
            this.PreviousVerticalLine = 1;
            this.PlayerVerticalVelocity = (22 * scale) / 6.5f;
            this.PlayerHorizontalVelocity = 5;

            int linePosition = (32 * scale);
            Lines = new int[] {
                BufferHeight - linePosition  + (-5 * scale),
                BufferHeight - (linePosition *2) + (5 * scale),
                BufferHeight - (linePosition * 3) + (11 * scale),
                BufferHeight - (linePosition * 4) + (24 * scale),
            };

            canMoveVertical = true;
            this.Position = new Vector2(0, Lines[CurrentVerticalLine]);
        }


        public void PlayAnimation()
        {
            if (Position.Y == Lines[CurrentVerticalLine]) canMoveVertical = true;
            else
            {
                if (
                    (CurrentVerticalLine < PreviousVerticalLine && Lines[CurrentVerticalLine] < Position.Y) ||
                    (CurrentVerticalLine > PreviousVerticalLine && Lines[CurrentVerticalLine] > Position.Y))
                {
                    Position = new Vector2(Position.X, Lines[CurrentVerticalLine]);
                }
                else
                {
                    if (Position.Y < Lines[CurrentVerticalLine])
                    {
                        Position = new Vector2(Position.X, Position.Y + PlayerVerticalVelocity);
                    }
                    else if (Position.Y > Lines[CurrentVerticalLine])
                    {
                        Position = new Vector2(Position.X, Position.Y - PlayerVerticalVelocity);
                    }
                }

            }
        }

        public void Update()
        {
            this.PlayAnimation();

            if (canMoveVertical)
            {
                PreviousVerticalLine = CurrentVerticalLine;

                if (Keyboard.GetState().IsKeyDown(Keys.Up) && CurrentVerticalLine < 3)
                {
                    CurrentVerticalLine += 1;
                    canMoveVertical = false;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Down) && CurrentVerticalLine > 0)
                {
                    CurrentVerticalLine -= 1;
                    canMoveVertical = false;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                Position = new Vector2(Position.X + PlayerHorizontalVelocity, Position.Y);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                Position = new Vector2(Position.X - PlayerHorizontalVelocity, Position.Y);
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.sprite, this.Position, new Rectangle(new Point(0, 0), new Point(32, 32)), Color.White, 0, new Vector2(0, 0), this.scale, SpriteEffects.None, 0f);
        }

    }
}
