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
        private int BufferWidth;
        private int[] Lines;
        private int PreviousVerticalLine;


        public PlayerController (Texture2D sprite, int scale, int BufferHeight, int BufferWidth)
        {
            this.sprite = sprite;
            this.scale = scale;

            this.CurrentVerticalLine = 0;
            this.PreviousVerticalLine = 1;
            this.PlayerVerticalVelocity = (22 * scale) / 6.5f;
            this.PlayerHorizontalVelocity = 3 * this.scale;
            this.BufferWidth = BufferWidth;

            int linePosition = (32 * scale);
            Lines = new int[] {
                BufferHeight - linePosition  + (-5 * scale),
                BufferHeight - (linePosition *2) + (5 * scale),
                BufferHeight - (linePosition * 3) + (11 * scale),
                BufferHeight - (linePosition * 4) + (24 * scale),
            };

            canMoveVertical = true;
            this.Position = new Vector2(0, Lines[CurrentVerticalLine]);
            this.Body = new Rectangle(new Point(16 * this.scale, 21 * this.scale), new Point(8 * this.scale, 0));
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

        public void Update(Level.Render map)
        {

            //map.Collision(this.Body, this.Position, this.CurrentVerticalLine);
            this.PlayAnimation();
            int pull = 2;

            map.Collision(this.Body, this.Position, this.CurrentVerticalLine);
            if (this.Position.X - (pull * this.scale) > 0 && !map.Collision(
                this.Body,
                new Vector2(this.Position.X - (pull * this.scale), this.Position.Y),
                this.CurrentVerticalLine
                )) this.Position = new Vector2(this.Position.X - (pull * this.scale), this.Position.Y);

            if (canMoveVertical)
            {
                PreviousVerticalLine = CurrentVerticalLine;

                if (Keyboard.GetState().IsKeyDown(Keys.Up) && CurrentVerticalLine < 3)
                {
                    if (!map.Collision(this.Body, this.Position, this.CurrentVerticalLine + 1))
                    {
                        CurrentVerticalLine += 1;
                        canMoveVertical = false;
                    }
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Down) && CurrentVerticalLine > 0)
                {
                    if (!map.Collision(this.Body, this.Position, this.CurrentVerticalLine - 1))
                    {
                        CurrentVerticalLine -= 1;
                        canMoveVertical = false;
                    }
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                if (Position.X + PlayerHorizontalVelocity < this.BufferWidth - (this.scale * 64)) Position = new Vector2(Position.X + PlayerHorizontalVelocity, Position.Y);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                if (Position.X - PlayerHorizontalVelocity > 0) Position = new Vector2(Position.X - PlayerHorizontalVelocity, Position.Y);
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.sprite, this.Position, new Rectangle(new Point(0, 0), new Point(32, 32)), Color.White, 0, new Vector2(0, 0), this.scale, SpriteEffects.None, 0f);
        }

    }
}
