using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CareerOpportunities
{
    public class MenuManagement : GameObject
    {
        public enum MenuItens { START, OPTIONS, CREDITS, EXIT, NONE };
        public MenuItens ItemOver;
        public MenuItens ItemSelected;

        public bool any_button;
        public bool any_button_press;
        public Vector2 PositionAnyButton;

        public SpriteFont Font;

        public string[] MenuItensString;
        private GraphicsDevice GraphicsDevice;

        public MenuManagement(Texture2D sprite, SpriteFont Font, int scale, GraphicsDevice graphicsDevice)
        {
            this.Sprite = sprite;
            this.Scale = scale;
            this.GraphicsDevice = graphicsDevice;
            this.ItemOver = MenuItens.START;
            this.ItemSelected = MenuItens.NONE;
            this.Position = new Vector2(140*this.Scale, 100*this.Scale);
            this.PositionAnyButton = new Vector2(120 * this.Scale,  130 * this.Scale);
            this.Font = Font;
            this.MenuItensString = new string[] {
                "START",
                "OPTIONS",
                "CREDITS",
                "EXIT" };

            this.SetSizeString();
        }



        private float[] MenuItensWidth;
        private void SetSizeString()
        {
            this.MenuItensWidth = new float[this.MenuItensString.Length];
            for (int i = 0; i < this.MenuItensString.Length; i++)
            {
                this.MenuItensWidth[i] = this.Font.MeasureString(this.MenuItensString[i]).X;
            }
        }

        public void Update(GameTime gameTime, Controller.Input input)
        {
            if (this.any_button && !any_button_press)
            {
                if (input.KeyPress(Controller.Input.Button.DOWN))
                {
                    if ((int)this.ItemOver < 3)
                    {
                        int item = (int)this.ItemOver;
                        item += 1;

                        this.ItemOver = (MenuItens)item;
                    }
                }

                if (input.KeyPress(Controller.Input.Button.UP))
                {
                    if ((int)this.ItemOver > 0)
                    {
                        int item = (int)this.ItemOver;
                        item -= 1;

                        this.ItemOver = (MenuItens)item;
                    }
                }

                if (input.KeyPress(Controller.Input.Button.CONFIRM))
                {
                    this.ItemSelected = this.ItemOver;
                }
            } else
            {
                if (input.PressAnyButton())
                {
                    this.any_button_press = true;
                    this.any_button = true;
                }
                else this.any_button_press = false;
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.any_button)
            {
                for (int i = 0; i < this.MenuItensString.Length; i++)
                {
                    Vector2 position = new Vector2((GraphicsDevice.Viewport.Width/2f) - ((this.MenuItensWidth[i]) / 2f), this.Position.Y + ((i * 9) * this.Scale));
                    this.SpriteColor = Color.Gray;
                    if ((MenuItens)i == this.ItemOver) this.SpriteColor = Color.White;
                    spriteBatch.DrawString(this.Font, this.MenuItensString[i], position, this.SpriteColor);
                }
            }
            else
            {
                spriteBatch.DrawString(this.Font, "Press Any Button", this.PositionAnyButton, Color.White);
            }
        }
    }
}
