using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace CareerOpportunities
{
    public class PauseMenuManagement : GameObject
    {

        public enum MenuStatus { RESUME, EXIT, NONE };
        public String[] MenuPause = new String[] { "RESUME (ENTER)", "EXIT (ESC)" };
        public String[] MenuGameOver = new String[] { "CONTINUIE (ENTER)", "EXIT (ESC)" };
        public MenuStatus ItemSelected;
        public bool gameOver;
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


        public void Update(GameTime gameTime, Controller.Input input)
        {
            if (input.KeyPress(Controller.Input.Button.ESC))this.ItemSelected = MenuStatus.EXIT;
            else if (input.KeyPress(Controller.Input.Button.CONFIRM)) this.ItemSelected = MenuStatus.RESUME;
        }

        public void Draw(SpriteBatch spriteBatch, float x_start)
        {
            Vector2 position_exit = new Vector2((this.Scale * 157) + x_start, this.Position.Y);
            Vector2 position_first_btn = new Vector2(this.Position.X + x_start, this.Position.Y);

            if (this.gameOver)
            {
                spriteBatch.DrawString(this.Font, MenuGameOver[0], position_first_btn, this.SpriteColor);
                spriteBatch.DrawString(this.Font, MenuGameOver[1], position_exit, this.SpriteColor);
            }
            else
            {
                spriteBatch.DrawString(this.Font, MenuPause[0], position_first_btn, this.SpriteColor);
                spriteBatch.DrawString(this.Font, MenuPause[1], position_exit, this.SpriteColor);
            }
        }
    }
}
