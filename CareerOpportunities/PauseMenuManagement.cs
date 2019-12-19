using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace CareerOpportunities
{
    public class PauseMenuManagement : GameObject
    {

        public enum MenuStatus { RESUME, EXIT, NONE };
        public String[] MenuPause = new String[] { "RESUME (ENTER)", "EXIT (ESC)" };
        public String[] MenuGameOver = new String[] { "CONTINUIE (ENTER)", "EXIT (ESC)" };
        public MenuStatus ItemSelected;
        public bool gameOver;
        public bool Released = false;
        public SpriteFont Font;

        public PauseMenuManagement(Texture2D sprite, int scale)
        {
            this.Sprite = sprite;
            this.Scale = scale;
            this.Position = new Vector2(47*this.Scale, 119*this.Scale);
            this.ItemSelected = MenuStatus.NONE;
            this.SpriteColor = Color.White;
            this.gameOver = false;
        }


        public void Update(GameTime gameTime)
        {
            if(Keyboard.GetState().IsKeyDown(Keys.Escape) && this.Released)
            {
                this.ItemSelected = MenuStatus.EXIT;
                this.Released = false;
            } else if (Keyboard.GetState().IsKeyDown(Keys.Enter) && this.Released)
            {
                this.ItemSelected = MenuStatus.RESUME;
                this.Released = false;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Escape) && Keyboard.GetState().IsKeyUp(Keys.Enter)) this.Released = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //if (this.gameOver) this.Body = new Rectangle(new Point(0, 0), new Point(75, 8));
            //else this.Body = new Rectangle(new Point(75, 0), new Point(75, 8));

            // spriteBatch.Draw(this.Sprite, this.Position, this.Body, this.SpriteColor, 0, new Vector2(0, 0), this.Scale, SpriteEffects.None, 0f);

            //this.Body = new Rectangle(new Point(150, 0), new Point(75, 8));
            Vector2 position_exit = new Vector2(this.Scale * 157, this.Position.Y);
            //spriteBatch.Draw(this.Sprite, position_exit, this.Body, this.SpriteColor, 0, new Vector2(0, 0), this.Scale, SpriteEffects.None, 0f);
            if (this.gameOver)
            {
                spriteBatch.DrawString(this.Font, MenuGameOver[0], this.Position, this.SpriteColor);
                spriteBatch.DrawString(this.Font, MenuGameOver[1], position_exit, this.SpriteColor);

            }
            else
            {
                spriteBatch.DrawString(this.Font, MenuPause[0], this.Position, this.SpriteColor);
                spriteBatch.DrawString(this.Font, MenuPause[1], position_exit, this.SpriteColor);
            }
        }
    }
}
