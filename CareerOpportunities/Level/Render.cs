﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CareerOpportunities.Level
{
    public class Render : GameObject
    {
        // {R:172 G:50 B:50 A:255} 
        Texture2D BoxTexture;
        Texture2D BoxShadow;
        Texture2D TileMap;
        Texture2D CoinTexture;
        Texture2D HeartTexure;
        Texture2D RampTexture;
        Texture2D Ground;

        Vector2[] PositionGround;

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
        Color HeartsColor = Color.Blue;
        Color RampColor = Color.Green;
        int[] LinesBox;

        int stopFramesNum = 0;
        int CurrentStopFramesNum = 1;

        public Render(int scale, int BufferHeight, int start)
        {
            this.scale = scale;
            this.start = start;
            this.currentPositionX = start;
            this.BufferHeight = BufferHeight;

            this.PositionGround = new Vector2[] {
                    new Vector2(0,0),
                    new Vector2(start,0)
            };

            this.LinesBox = new int[] {
                this.BufferHeight - ((33 * scale)) + (scale),
                this.BufferHeight - ((32 * scale) * 2) + (10 * scale),
                this.BufferHeight - ((32 * scale) * 3) + (20 * scale),
                this.BufferHeight - ((32 * scale) * 4) + (30 * scale),
                this.BufferHeight - ((32 * scale) * 5) + (40 * scale),
            };
        }

        public void setGround(Texture2D Ground)
        {
            this.Ground = Ground;
        }

        public void setBoxTexture(Texture2D box)
        {
            this.BoxTexture = box;
        }

        public void setBoxShadow(Texture2D boxShadow)
        {
            this.BoxShadow = boxShadow;
        }

        public void setHeartTexture(Texture2D heart)
        {
            this.HeartTexure = heart;
        }

        public void setRampTexture(Texture2D ramp)
        {
            this.RampTexture = ramp;
        }

        public void setCoinTexture(Texture2D coin, string jsonFile)
        {
            this.CoinTexture = coin;
            this.setJsonFile(jsonFile);
            this.setSprite(this.CoinTexture);
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
                    for (int y = this.TileMap.Height - 1; y >= 0 ; y--)
                    {
                        this.MapColors[x, this.TileMap.Height - 1 - y] = colors1D[x + y * this.TileMap.Width];
                    }
                }
            }

            this.PositionTile();
        }

        private void PositionTile()
        {
            List<Vector2> termsList = new List<Vector2>();
            List<Color> termsListColors = new List<Color>();
            for (int x = 0; x < this.TileMap.Width; x++)
            {
                for (int y = this.TileMap.Height - 1; y >= 0; y--)
                {
                    if (this.MapColors[x, y] == this.BoxColor)
                    {
                        termsList.Add(new Vector2(x, y));
                        termsListColors.Add(this.BoxColor);
                    }
                    else if (this.MapColors[x, y] != Color.Black)
                    {
                        termsList.Add(new Vector2(x, y));
                        termsListColors.Add(this.MapColors[x,y]);
                    }
                }
            }
            this.SpritesColors = termsListColors.ToArray();
            this.positionBoxs = termsList.ToArray();
        }

        public int LinePosition(float Y)
        {
            int line = 0;
            if (Y > this.LinesBox[4] && Y < this.LinesBox[3]) line = 4;
            else if (Y > this.LinesBox[3] && Y < this.LinesBox[2]) line = 3;
            else if (Y > this.LinesBox[2] && Y < this.LinesBox[1]) line = 2;
            else if (Y > this.LinesBox[1] && Y < this.LinesBox[0]) line = 1;
            else if (Y > this.LinesBox[0]) line = 0;
            return line;
        }

        public bool Collision(Rectangle body, Vector2 position, int line, bool item = true)
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
                            //if (this.MapColors[x, y] != this.BoxColor) this.CollisionItem(new Vector2(x, y));
                            if(!item && this.MapColors[x, y] == this.BoxColor) this.CollisionPosition = new Vector2(x, y);
                            else if (item) this.CollisionPosition = new Vector2(x, y);
                            if (this.MapColors[x, y] == this.BoxColor) any_collision = true;
                        }

                    }
                }
            }
            return any_collision;
        }

        public string CollisionItem(Vector2 position, bool item = false)
        {
            Color ReturnColor = this.MapColors[(int)position.X, (int)position.Y];
            string ReturnItem = "";
            

            for (int i = 0; i < this.positionBoxs.Length; i++)
            {
                if (this.positionBoxs[i] == position)
                {
                    if (this.BoxColor != ReturnColor && Color.Black != ReturnColor && item)
                    {
                        if (ReturnColor == this.HeartsColor) ReturnItem = "heart";
                        if (ReturnColor == this.CoinColor) ReturnItem = "coin";
                        if (ReturnColor == this.RampColor) ReturnItem = "ramp";
                        if (this.RampColor != ReturnColor)
                        {
                            this.SpritesColors[i] = Color.Black; // if (this.CoinColor == this.SpritesColors[i])
                            this.MapColors[(int)position.X, (int)position.Y] = Color.Black;
                        }
                    }
                    else if (!item)
                    {
                        if (ReturnColor == this.BoxColor) ReturnItem = "box";
                        this.SpritesColors[i] = Color.Black; // if (this.CoinColor == this.SpritesColors[i])
                        this.MapColors[(int)position.X, (int)position.Y] = Color.Black;
                    }
                   
                }
            }

            return ReturnItem;
        }

        public bool Finished()
        {
            if (-this.currentPositionX + (this.start * this.scale) > (this.TileMap.Width * 32) * this.scale) return true;
            return false;
        }

        public void Update(GameTime gameTime, int velocity)
        {
            this.play(gameTime, "round");
            if ( CurrentStopFramesNum > stopFramesNum )
            {
                float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
                int velocityCurrent = (int)(velocity * delta * this.scale);
                this.currentPositionX -= velocityCurrent;

                for (int i = 0; i < this.PositionGround.Length; i++) {
                    if (this.PositionGround[i].X <= -(start - velocityCurrent)) this.PositionGround[i] = new Vector2(start, 0);
                    else this.PositionGround[i] = new Vector2(this.PositionGround[i].X - velocityCurrent, this.PositionGround[i].Y);
                }
            }
            else CurrentStopFramesNum += 1;
        }


        public void DrawGround(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Ground, this.PositionGround[0], null, Color.White, 0, new Vector2(0, 0), (scale / 5f), SpriteEffects.None, 0f);
            spriteBatch.Draw(Ground, this.PositionGround[1], null, Color.White, 0, new Vector2(0, 0), (scale / 5f), SpriteEffects.None, 0f);
        }

        public void Layers(SpriteBatch spriteBatch, int layer, bool front)
        {
            if (front)
            {
                for (int i_layer = layer != 0 ? layer - 1: layer; i_layer >= 0; i_layer--) this.drawLayer(i_layer, spriteBatch);
            }
            else
            {
                for (int i_layer = 4; i_layer >= layer; i_layer--) this.drawLayer(i_layer, spriteBatch);
            }
        }

        private void drawLayer(int i_layer, SpriteBatch spriteBatch)
        {
            for (int i = this.positionBoxs.Length - 1; i >= 0; i--)
            {
                if (i_layer == (int)this.positionBoxs[i].Y) this.DrawBoxShadow(spriteBatch, i);
            }

            for (int i = this.positionBoxs.Length - 1; i >= 0; i--)
            {
                if (i_layer == (int)this.positionBoxs[i].Y) this.Draw(spriteBatch, i);
            }
        }

        public void DrawBoxShadow(SpriteBatch spriteBatch, int i)
        {
            Texture2D sprite = this.BoxShadow;
            Vector2 position = new Vector2((this.positionBoxs[i].X * (this.scale * 25)) + (this.currentPositionX), this.LinesBox[(int)this.positionBoxs[i].Y]);
            if (SpritesColors[i] == Color.Red)
            {
               spriteBatch.Draw(sprite, position, new Rectangle(new Point(0, 0), new Point(44 * 5, 43 * 5)), Color.White, 0, new Vector2(0, 0), this.scale / 5f, SpriteEffects.None, 0f);
            }
        }

        public void Draw(SpriteBatch spriteBatch, int i)
        {
            Texture2D sprite = this.BoxTexture;
            if (SpritesColors[i] == this.CoinColor) sprite = this.CoinTexture;
            if (SpritesColors[i] == this.HeartsColor) sprite = this.HeartTexure;
            if (SpritesColors[i] == this.RampColor) sprite = this.RampTexture;


            Vector2 position = new Vector2((this.positionBoxs[i].X * (this.scale * 25)) + (this.currentPositionX), this.LinesBox[(int)this.positionBoxs[i].Y]);

            if (SpritesColors[i] == Color.Red)
            {
                spriteBatch.Draw(sprite, position, new Rectangle(new Point(0, 0), new Point(this.tileWidth * 5, 33 * 5)), Color.White, 0, new Vector2(0, 0), this.scale/5f, SpriteEffects.None, 0f);
            } else if (SpritesColors[i] == this.HeartsColor)
            {
                spriteBatch.Draw(sprite, position, new Rectangle(new Point(0, 0), new Point(this.tileWidth, 33)), Color.White, 0, new Vector2(0, 0), this.scale, SpriteEffects.None, 0f);
            } else if (SpritesColors[i] == this.RampColor)
            {
                spriteBatch.Draw(sprite, position, new Rectangle(new Point(0, 0), new Point(22, 30)), Color.White, 0, new Vector2(0, 0), this.scale, SpriteEffects.None, 0f);
            }

            if (SpritesColors[i] == this.CoinColor)
            {
                this.sizeMutiply = 5;
                this.DrawAnimation(spriteBatch, position, scale);
            }
        }


    }
}
