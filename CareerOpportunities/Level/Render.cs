using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CareerOpportunities.Level
{
    public class Render
    {
        // {R:172 G:50 B:50 A:255} 
        Texture2D BoxTexture;
        Texture2D TileMap;
        Texture2D CoinTexture;

        int start;
        int currentPositionX;
        Color[,] MapColors;
        Vector2[] positionBoxs;
        Color[] SpritesColors;
        public Vector2 CollisionPosition;
        int scale;
        int BufferHeight;
        int tileWidth = 34;

        Color BoxColor = Color.Red;
        Color CoinColor = Color.Yellow;
        int[] LinesBox;

        int stopFramesNum = 0;
        int CurrentStopFramesNum = 1;

        public Render(int scale, int BufferHeight, int start)
        {
            this.scale = scale;
            this.start = start;
            this.currentPositionX = start;
            this.BufferHeight = BufferHeight;

            this.LinesBox = new int[] {
                this.BufferHeight - ((33 * scale)) + (scale),
                this.BufferHeight - ((32 * scale) * 2) + (10 * scale),
                this.BufferHeight - ((32 * scale) * 3) + (20 * scale),
                this.BufferHeight - ((32 * scale) * 4) + (30 * scale),
            };
        }

        public void setBoxTexture(Texture2D box)
        {
            this.BoxTexture = box;
        }

        public void setCoinTexture(Texture2D coin)
        {
            this.CoinTexture = coin;
        }

        public void setTileMap(Texture2D map)
        {
            this.TileMap = map;
            this.readMap();
        }

        public void StopFor(int frames = 15)
        {
            stopFramesNum = frames;
            CurrentStopFramesNum = 0;
        }

        public bool isStoped()
        {
            if (CurrentStopFramesNum > stopFramesNum) return true;
            else return false;
        }

        public void readMap()
        {
            if (TileMap != null)
            {
                Color[] colors1D = new Color[this.TileMap.Width * this.TileMap.Height];
                this.TileMap.GetData(colors1D);
                this.MapColors = new Color[this.TileMap.Width, this.TileMap.Height];

                for (int x = 0; x < this.TileMap.Width; x++)
                {
                    for (int y = 0; y < this.TileMap.Height; y++)
                    {
                        this.MapColors[x, y] = colors1D[x + y * this.TileMap.Width];
                    }
                }
            }

            this.PositionTile();
        }

        private void PositionTile()
        {
            List<Vector2> termsList = new List<Vector2>();
            List<Color> termsListColors = new List<Color>();
            for (int x = this.TileMap.Width - 1; x >= 0; x--)
            {
                for (int y = this.TileMap.Height - 1; y >= 0; y--)
                {
                    if (this.MapColors[x, y] == this.BoxColor)
                    {
                        termsList.Add(new Vector2(x, y));
                        termsListColors.Add(this.BoxColor);
                    } else if (this.MapColors[x, y] == this.CoinColor)
                    {
                        termsList.Add(new Vector2(x, y));
                        termsListColors.Add(this.CoinColor);
                    }
                }
            }
            this.SpritesColors = termsListColors.ToArray();
            this.positionBoxs = termsList.ToArray();
        }

        public bool Collision(Rectangle body, Vector2 position, int line)
        {
            bool any_collision = false;

            for (int x = 0; x < this.TileMap.Width; x++)
            {
                for (int y = 0; y < this.TileMap.Height; y++)
                {
                    if (this.MapColors[x, y] != Color.Black) {
                        float x_position = ((x) * 25) * this.scale + this.currentPositionX;
                        float x_width = x_position + (40 * this.scale);

                        bool x_overlaps = (((body.X + position.X < x_position) && (body.X + position.X + body.Width > x_position) && (body.X + position.X + body.Width < x_width)) || 
                                           ((body.X + position.X > x_position) && (body.X + position.X + body.Width < x_width)) ||
                                           ((body.X + position.X > x_position) && (body.X + position.X < x_width) && (body.X + position.X + body.Width > x_width)));
                        bool y_overlaps = line == y;

                        if (x_overlaps && y_overlaps)
                        {
                            if (this.MapColors[x, y] != this.BoxColor) this.CollisionItem(new Vector2(x, y));
                            this.CollisionPosition = new Vector2(x, y);
                            if (this.MapColors[x, y] == this.BoxColor) any_collision = true;
                        }

                    }
                }
            }
            return any_collision;
        }

        public void CollisionItem(Vector2 position)
        {
            for (int i = 0; i < this.positionBoxs.Length; i++)
            {
                if (this.positionBoxs[i] == position){
                    this.SpritesColors[i] = Color.Black; // if (this.CoinColor == this.SpritesColors[i])
                    this.MapColors[(int)position.X, (int)position.Y] = Color.Black;
                }
            }
        }

        public bool Finished()
        {
            if (-this.currentPositionX + (this.start * this.scale) > (this.TileMap.Width * 32) * this.scale) return true;
            return false;
        }

        public void Update(GameTime gameTime, int velocity)
        {
            if ( CurrentStopFramesNum > stopFramesNum )
            {
                float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.currentPositionX -= (int)(velocity * delta * this.scale);
            }
            else CurrentStopFramesNum += 1;
        }

        public void Layer0(SpriteBatch spriteBatch, int layer)
        {
            for(int i = 0; i < this.positionBoxs.Length; i++)
            {
                if (layer > (int)this.positionBoxs[i].Y) this.Draw(spriteBatch, i);
            }
        }

        public void Layer1(SpriteBatch spriteBatch, int layer)
        {
            for (int i = 0; i < this.positionBoxs.Length; i++)
            {
                if (layer <= (int)this.positionBoxs[i].Y) this.Draw(spriteBatch, i);
            }
        }

        public void Draw(SpriteBatch spriteBatch, int i)
        {
            Texture2D sprite = this.BoxTexture;
            if (SpritesColors[i] == this.CoinColor) sprite = this.CoinTexture;

            if (SpritesColors[i] != Color.Black)
            {
                Vector2 position = new Vector2((this.positionBoxs[i].X * (this.scale * 25)) + (this.currentPositionX), this.LinesBox[(int)this.positionBoxs[i].Y]);
                spriteBatch.Draw(sprite, position, new Rectangle(new Point(0, 0), new Point(this.tileWidth, 33)), Color.White, 0, new Vector2(0, 0), this.scale, SpriteEffects.None, 0f);
            }
        }


    }
}
