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

        int start;
        int currentPositionX;
        Color[,] MapColors;
        Vector2[] positionBoxs;
        int scale;
        int BufferHeight;
        int tileWidth = 34;

        Color BoxColor = Color.Red;

        int[] LinesBox;

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

        public void setTileMap(Texture2D map)
        {
            this.TileMap = map;
            this.readMap();
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
            for (int x = this.TileMap.Width - 1; x >= 0; x--)
            {
                for (int y = this.TileMap.Height - 1; y >= 0; y--)
                {
                    if (this.MapColors[x, y] == this.BoxColor)
                    {
                        termsList.Add(new Vector2((x) * (this.scale * 25), y));
                    }
                }
            }

            this.positionBoxs = termsList.ToArray();
        }

        public void Update(int velocity)
        {
            this.currentPositionX -= velocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for(int i = 0; i < this.positionBoxs.Length; i++)
            {
                Vector2 position = new Vector2(this.positionBoxs[i].X + (this.currentPositionX * this.scale), this.LinesBox[(int)this.positionBoxs[i].Y]);
                spriteBatch.Draw(this.BoxTexture, position, new Rectangle(new Point(0, 0), new Point(this.tileWidth, 33)), Color.White, 0, new Vector2(0, 0), this.scale, SpriteEffects.None, 0f);
            }
        }


    }
}
