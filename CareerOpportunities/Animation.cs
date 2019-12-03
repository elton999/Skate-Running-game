using System;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CareerOpportunities
{
   public class Animation { 

        // json file
        private string jsonUrl;
        public JsonTextReader jsonContent;
        private JObject jsonJObject;
        private dynamic json;

        // json info
        private int a_from;
        private int a_to;
        private string tag;
        private string direction;

        // animation
        private Texture2D sprite;
        private Rectangle spriteSourceSize;
        private int frame;
        private int frameCurrent;
        private int frameCount;
        private float maxFrame;
        

        public void setJsonFile(string jsonContent = null)
        {
            if (jsonContent != null)
            {
                jsonUrl = jsonContent;
                this.LoadJson();
            }

        }

        public void setSprite (Texture2D sprite)
        {
            this.sprite = sprite;
        }

        public bool AnimationIsReady(){
            if (this.jsonContent != null && this.jsonJObject != null && this.json != null && this.sprite != null)
            {
                if (this.json.meta != null) return true;
            }
            return false;
        }

        private void LoadJson()
        {
            using (StreamReader stream = new StreamReader(this.jsonUrl))
            {
                jsonContent = new JsonTextReader(stream);
                jsonJObject = JObject.Load(jsonContent);
                this.json = (dynamic)jsonJObject;
            }
        }


        public string getTag(int tag = 0)
        {
            return (string)this.json.meta.frameTags[tag].name;
        }

        public void play(GameTime gameTime, string tag)
        {

            if(tag != this.tag)
            {
                int i = 0;
                while (i < this.json.meta.frameTags.Count)
                {
                    if (tag == (string)this.json.meta.frameTags[i].name)
                    {
                        this.a_from    = (int)this.json.meta.frameTags[i].from;
                        this.a_to      = (int)this.json.meta.frameTags[i].to;
                        this.tag       = (string)this.json.meta.frameTags[i].name;
                        this.direction = (string)this.json.meta.frameTags[i].direction;
                        break;
                    }
                    i++;
                }
            }
            
            this.maxFrame = ((float)(this.json.frames[this.frame].duration) / 1000f * 60f);
            this.frameCount++;

            if (this.frameCount >= this.maxFrame)
            {
                if ((this.frameCurrent + this.a_from) <= this.a_to)
                {
                    frame             = (this.frameCurrent + this.a_from);
                    dynamic frameInfo = this.json.frames[frame];

                    Point size = new Point((int)frameInfo.frame.h, (int)frameInfo.frame.w);
                    Point map  = new Point((int)frameInfo.frame.x, (int)frameInfo.frame.y);

                    this.spriteSourceSize = new Rectangle(map, size);
                    if (this.a_to != (this.frameCurrent + this.a_from)) this.frameCurrent++;
                    else this.frameCurrent = 0;
                }
                else
                {
                    this.frameCurrent = 0;
                }
                this.frameCount = 0;
            }
           
        }

        public void DrawAnimation(SpriteBatch spriteBatch, Vector2 position, int scale)
        {
            // spriteBatch.Draw(this.sprite, position, this.spriteSourceSize, Color.White);
            spriteBatch.Draw(sprite, position, this.spriteSourceSize, Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
        }

        public void DrawInfo(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            if (this.tag != null) spriteBatch.DrawString(spriteFont, "animation: "+this.tag, new Vector2(0,0), Color.White);
            spriteBatch.DrawString(spriteFont, "Current frame: " + (this.frameCurrent + this.a_from), new Vector2(0, 25), Color.White);
        }
    }
}
