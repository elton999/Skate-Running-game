using System;
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
    public class CoinManagement : GameObject
    {
        public int numbCoins = 0;
        public SpriteFont font;
        public ContentManager Content;
        public List<Hud.Score> CoinPlusList = new List<Hud.Score>();

        public CoinManagement(Texture2D sprite, SpriteFont font, int scale)
        {
            this.Sprite = sprite;
            this.font = font;
            this.Scale = scale;

            Point size = new Point(8, 8);
            Point position = new Point(0, 0);
            this.Position = new Vector2((3*this.Scale) + (30 * this.Scale), 13 * this.Scale);
            this.Body = new Rectangle(position, size);
        }


        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < this.CoinPlusList.Count(); i++)
            {
                if (!this.CoinPlusList[i].CanDetroy) this.CoinPlusList[i].Update(gameTime);
            }
        }


        public void add(Vector2 Position, int numb = 1)
        {
            this.numbCoins += numb;
            this.CoinPlusList.Add(new Hud.Score(Content, this.Scale, Position));
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            string CollectedCoins = "000";
            if (numbCoins < 10) CollectedCoins = "00" + numbCoins.ToString();
            else if (numbCoins < 100) CollectedCoins = "0" + numbCoins.ToString();
            else CollectedCoins = numbCoins.ToString();

            spriteBatch.Draw(this.Sprite, this.Position, this.Body, Color.White, 0, new Vector2(0, 0), this.Scale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(this.font, CollectedCoins, new Vector2(this.Position.X + (9 * this.Scale), this.Position.Y), Color.White);

            for (int i = 0; i < this.CoinPlusList.Count(); i++)
            {
                if (!this.CoinPlusList[i].CanDetroy) this.CoinPlusList[i].Draw(spriteBatch);
            }
        }

    }
}
