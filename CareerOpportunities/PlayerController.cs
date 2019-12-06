using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace CareerOpportunities
{
    public class PlayerController : GameObject
    {
        public bool canMoveVertical;
        public int CurrentVerticalLine;
        public float PlayerVerticalVelocity;
        public int PlayerHorizontalVelocity;

        private int BufferWidth;
        private int[] Lines;
        private int PreviousVerticalLine;

        private bool removeITem;


        public PlayerController (Texture2D sprite, int scale, int BufferHeight, int BufferWidth)
        {
            this.Sprite = sprite;
            this.Scale = scale;

            this.CurrentVerticalLine = 0;
            this.PreviousVerticalLine = 1;
            this.PlayerVerticalVelocity = (22 * scale) / 6.5f;
            this.PlayerHorizontalVelocity = 3 * this.Scale;
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
            this.Body = new Rectangle(new Point(16 * this.Scale, 21 * this.Scale), new Point(8 * this.Scale, 0));
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

        public void Update(GameTime gameTime, Level.Render map, HeartManagement heart)
        {
            this.PlayAnimation();
            float pull = 100f;
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.removeITem = false;
            // Console.WriteLine(this.Position.X - (pull * this.scale * delta));

            if (map.Collision(this.Body, this.Position, this.CurrentVerticalLine))
            {
                string MapItem = map.CollisionItem(map.CollisionPosition);
                heart.remove(1);
                map.StopFor(35);
            }
            else
            {
                string MapItem = map.CollisionItem(map.CollisionPosition, true);
                switch (MapItem)
                {
                    case "heart":
                        heart.add(1);
                        break;
                }
            }

            if (map.isStoped())
            {
                if (this.Position.X - (pull * this.Scale * delta) > BufferWidth / 2 && !map.Collision(
                this.Body,
                new Vector2(this.Position.X - (pull * this.Scale * delta), this.Position.Y),
                this.CurrentVerticalLine
                )) this.Position = new Vector2(this.Position.X - (pull * this.Scale * delta), this.Position.Y);
                else
                {
                    if (this.Position.X + (pull * this.Scale * delta) < BufferWidth / 2) this.Position = new Vector2(this.Position.X + ((pull / 2) * this.Scale * delta), this.Position.Y);
                }

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

                // left and right
                if (Keyboard.GetState().IsKeyDown(Keys.Right) && !map.Collision(this.Body, new Vector2(Position.X + PlayerHorizontalVelocity, Position.Y), this.CurrentVerticalLine))
                {
                    if (Position.X + PlayerHorizontalVelocity < this.BufferWidth - (this.Scale * 32)) Position = new Vector2(Position.X + PlayerHorizontalVelocity, Position.Y);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    if (Position.X - PlayerHorizontalVelocity > 0) Position = new Vector2(Position.X - PlayerHorizontalVelocity, Position.Y);
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Sprite, this.Position, new Rectangle(new Point(0, 0), new Point(32, 32)), Color.White, 0, new Vector2(0, 0), this.Scale, SpriteEffects.None, 0f);
        }

    }
}
