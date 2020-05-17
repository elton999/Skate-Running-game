﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CareerOpportunities
{
    public class HeartManagement : GameObject
    {
        public int NumberOfhearts = 3;
        public int PaddingLeft = 2;

        bool startRemoveItem;
        bool removeItem;


        public HeartManagement(Texture2D sprite)
        {
            Point size = new Point(9,8);
            Point position = new Point(0,0);
            this.Position = new Vector2(2,3);
            this.Body = new Rectangle(position, size);
            this.Sprite = sprite;
        }


        public ContentManager Content;
        public List<Hud.Score> HeartPlusList = new List<Hud.Score>();

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < this.HeartPlusList.Count(); i++)
            {
                if (!this.HeartPlusList[i].CanDetroy) this.HeartPlusList[i].Update(gameTime);
            }
        }

        public void remove(int num)
        {
            this.NumberOfhearts -= num;
        }

        public void add(int num, Vector2 Position)
        {
            this.NumberOfhearts += num;
            this.HeartPlusList.Add(new Hud.Score(Content, this.Scale, Position, false));
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.NumberOfhearts; i++)
            {
                int position_x = ((this.PaddingLeft + this.Body.Width) * i) * this.Scale;
                position_x = ((int)this.Position.X * this.Scale + position_x) + (30 * this.Scale);

                Vector2 position = new Vector2(position_x, this.Position.Y * this.Scale);

                spriteBatch.Draw(this.Sprite, position, this.Body, Color.White, 0, new Vector2(0, 0), this.Scale, SpriteEffects.None, 0f);
            }

            for (int i = 0; i < this.HeartPlusList.Count(); i++)
            {
                if (!this.HeartPlusList[i].CanDetroy) this.HeartPlusList[i].Draw(spriteBatch);
            }
        }
            

    }
}
