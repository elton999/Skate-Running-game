using System;
using CareerOpportunities.weapon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CareerOpportunities
{
    public class PlayerController : GameObject
    {
        public bool canMoveVertical;
        public int CurrentVerticalLine;
        public float PlayerVerticalVelocity;
        public float PlayerHorizontalVelocity;
        public float PlayerHorizontalAndVelocity;

        private int BufferWidth;
        private int[] Lines;
        private int PreviousVerticalLine;
        
        private bool isGrounded = true;
        private float JumpTime;
        private float JumpTimeCurrent = 0;

        private int runTimes;


        public PlayerController (Texture2D sprite, int scale, int BufferHeight, int BufferWidth, string jsonFile)
        {
            this.Sprite = sprite;
            this.Scale = scale;

            this.CurrentVerticalLine = 0;
            this.PreviousVerticalLine = 1;
            this.PlayerVerticalVelocity = (22 * scale) / 5.5f;
            this.PlayerHorizontalVelocity = 2 * this.Scale;
            this.PlayerHorizontalAndVelocity = 0.4f * this.Scale;
            this.BufferWidth = BufferWidth;

            int linePosition = (32 * scale);
            Lines = new int[] {
                BufferHeight - linePosition  + (-5 * scale),
                BufferHeight - (linePosition *2) + (5 * scale),
                BufferHeight - (linePosition * 3) + (11 * scale),
                BufferHeight - (linePosition * 4) + (24 * scale),
                BufferHeight - (linePosition * 5) + (35 * scale),
                BufferHeight - (linePosition * 6) + (45 * scale),
            };

            this.setJsonFile(jsonFile);
            this.setSprite(this.Sprite);

            canMoveVertical = true;
            this.Position = new Vector2(0, Lines[CurrentVerticalLine]);
            this.Body = new Rectangle(new Point(16 * this.Scale, 21 * this.Scale), new Point(8 * this.Scale, 0));

            this.runTimes = 0;
        }


        public void PlayAnimation(Level.Render map, GameTime gameTime)
        {
            if (this.isGrounded)
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


            if (map.isStoped())
            {
                if (this.isGrounded)
                {
                    if (this.lastFrame) this.runTimes += 1;

                    if (this.runTimes < 3) this.play(gameTime, "run_idle");
                    else this.play(gameTime, "run");

                    if (this.runTimes == 4) this.runTimes = 0;
                } else this.play(gameTime, "jump");
            } else this.play(gameTime, "hit");
        }

        public void Update(GameTime gameTime, Controller.Input input, Level.Render map, HeartManagement heart, CoinManagement Coins, CameraManagement camera, Gun Weapon)
        {
            float pull = 100f;
            float JumpPull = pull;
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (this.isGrounded)
            {
                if (map.Collision(this.Body, this.Position, this.CurrentVerticalLine))
                {
                    string MapItem = map.CollisionItem(map.CollisionPosition, false);
                    heart.remove(1);
                    map.StopFor(60);
                    camera.TimeShake = 12;
                }
                else
                {
                    string MapItem = map.CollisionItem(map.CollisionPosition, true);
                    switch (MapItem)
                    {
                        case "heart":
                            heart.add(1);
                            break;
                        case "coin":
                            Coins.add(1);
                            break;
                        case "ramp":
                            this.isGrounded = false;
                            JumpPull = pull;
                            camera.StartZoomJump();
                            map.CollisionPosition = new Vector2(0, 0);
                            break;
                    }
                }

                if (map.isStoped())
                {
                    float limit = (BufferWidth / 2) - (100 * this.Scale);
                    if (this.Position.X - (pull * this.Scale * delta) > limit && !map.Collision(
                    this.Body,
                    new Vector2(this.Position.X - (pull * this.Scale * delta), this.Position.Y),
                    this.CurrentVerticalLine
                    )) this.Position = new Vector2(this.Position.X - (pull * this.Scale * delta), this.Position.Y);
                    else if (this.Position.X + (pull * this.Scale * delta) < limit) this.Position = new Vector2(this.Position.X + ((pull / 2) * this.Scale * delta), this.Position.Y);

                    if (canMoveVertical)
                    {
                        PreviousVerticalLine = CurrentVerticalLine;
                        // top and down
                        this.HorizontalMove(input, map);

                        // Jump
                        if (input.KeyPress(Controller.Input.Button.JUMP) && this.isGrounded)
                        {
                            this.isGrounded = false;
                            JumpPull = 140f;
                            camera.StartZoomJump();
                            map.CollisionPosition = new Vector2(0, 0);
                        }
                    }

                    // left and right
                    this.VerticalMove(input, map);

                    if (input.KeyPress(Controller.Input.Button.FIRE))
                    {
                        Weapon.Fire(this.Position);
                    }
                }
            }
            
            this.PlayAnimation(map, gameTime);
            this.Jump(gameTime, delta, pull);
        }

        public void VerticalMove(Controller.Input input, Level.Render map)
        {
            if (input.KeyDown(Controller.Input.Button.RIGHT) && !map.Collision(this.Body, new Vector2(Position.X + PlayerHorizontalVelocity, Position.Y), this.CurrentVerticalLine))
            {
                if (input.KeyDown(Controller.Input.Button.DOWN) || input.KeyDown(Controller.Input.Button.UP))
                {
                    if (Position.X + PlayerHorizontalAndVelocity < this.BufferWidth - (this.Scale * 32)) Position = new Vector2(Position.X + PlayerHorizontalAndVelocity, Position.Y);
                }
                else
                {
                    if (Position.X + PlayerHorizontalVelocity < this.BufferWidth - (this.Scale * 32)) Position = new Vector2(Position.X + PlayerHorizontalVelocity, Position.Y);
                }
            }
            if (input.KeyDown(Controller.Input.Button.LEFT))
            {
                if (input.KeyDown(Controller.Input.Button.DOWN) || input.KeyDown(Controller.Input.Button.UP))
                {
                    if (Position.X - PlayerHorizontalAndVelocity > 0) Position = new Vector2(Position.X - PlayerHorizontalAndVelocity, Position.Y);
                }
                else
                {
                    if (Position.X - PlayerHorizontalVelocity > 0) Position = new Vector2(Position.X - PlayerHorizontalVelocity, Position.Y);
                }
               
            }
        }

        public void HorizontalMove(Controller.Input input, Level.Render map)
        {
            if (input.KeyDown(Controller.Input.Button.UP) && CurrentVerticalLine < 4)
            {
                if (!map.Collision(this.Body, this.Position, this.CurrentVerticalLine + 1))
                {
                    CurrentVerticalLine += 1;
                    canMoveVertical = false;
                }
            }
            if (input.KeyDown(Controller.Input.Button.DOWN) && CurrentVerticalLine > 0)
            {
                if (!map.Collision(this.Body, this.Position, this.CurrentVerticalLine - 1))
                {
                    CurrentVerticalLine -= 1;
                    canMoveVertical = false;
                }
            }
        }

        public void Jump( GameTime gameTime, float delta, float pull)
        {
            if (!this.isGrounded)
            {
                if (this.JumpTimeCurrent <= this.JumpTime && this.Position.Y > Lines[CurrentVerticalLine + 1]) this.Position = new Vector2(this.Position.X, this.Position.Y - (pull * this.Scale * delta));
                else
                {
                    this.JumpTime = 0.2f*60f;
                    if (this.JumpTimeCurrent <= this.JumpTime) this.JumpTimeCurrent++;
                    else
                    {
                        if (this.Position.Y < Lines[CurrentVerticalLine]) this.Position = new Vector2(this.Position.X, this.Position.Y + ((pull / 2 ) * this.Scale * delta));
                        else
                        {
                            this.Position = new Vector2(this.Position.X, Lines[CurrentVerticalLine]);
                            this.isGrounded = true;
                            this.JumpTimeCurrent = 0;
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(this.Sprite, this.Position, new Rectangle(new Point(0, 0), new Point(32, 32)), Color.White, 0, new Vector2(0, 0), this.Scale, SpriteEffects.None, 0f);
            this.DrawAnimation(spriteBatch, this.Position, this.Scale);
        }

    }
}
