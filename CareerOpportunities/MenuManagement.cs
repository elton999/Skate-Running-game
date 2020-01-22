﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace CareerOpportunities
{
    public class MenuManagement : GameObject
    {
        public enum MenuItens { START, OPTIONS, CREDITS, EXIT, NONE };
        public MenuItens ItemOver;
        public MenuItens ItemSelected;

        public SpriteFont Font;

        public string[] MenuItensString;

        public MenuManagement(Texture2D sprite, SpriteFont Font, int scale)
        {
            this.Sprite = sprite;
            this.Scale = scale;
            this.ItemOver = MenuItens.START;
            this.ItemSelected = MenuItens.NONE;
            this.Position = new Vector2(102*this.Scale, 72*this.Scale);
            this.Font = Font;
            this.MenuItensString = new string[] {
                "START",
                "OPTIONS",
                "CREDITS",
                "EXIT" };
        }


        public void Update(GameTime gameTime, Controller.Input input)
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

            // if (Keyboard.GetState().IsKeyUp(Keys.Down) && Keyboard.GetState().IsKeyUp(Keys.Up)) released = true;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.Sprite.Width / 36; i++)
            {
                Vector2 position = new Vector2(this.Position.X, this.Position.Y + ((i * 9) * this.Scale));
                this.Body = new Rectangle(new Point(i * 36, 0), new Point(36, 6));

                this.SpriteColor = Color.Gray;
                if ((MenuItens)i == this.ItemOver) this.SpriteColor = Color.White;
                //spriteBatch.Draw(this.Sprite, position, this.Body, this.SpriteColor, 0, new Vector2(0, 0), this.Scale, SpriteEffects.None, 0f);
                spriteBatch.DrawString(this.Font, this.MenuItensString[i], position, this.SpriteColor);

            }
        }
    }
}
