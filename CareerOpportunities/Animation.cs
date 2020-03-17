using System;
using System.IO;
using System.Collections.Generic;
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
        
        // public bool lastFrame;

        public float sizeMutiply = 1f;
        

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
            if (this.json != null && this.sprite != null)
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
                // destroy
                jsonContent = null;
                jsonJObject = null;
                stream.Dispose();
                stream.DiscardBufferedData();
                stream.Close();
            }
        }


        public string getTag(int tag = 0)
        {
            return (string)this.json.meta.frameTags[tag].name;
        }

        #region animations
        // animation
        private Texture2D sprite;
        private Rectangle spriteSourceSize;
        private int frame;
        private int frameCurrent;
        private float frameCount;
        private List<float> maxFrame;
        private AnimationDirection direction;
        public enum AnimationDirection { FORWARD, LOOP, PING_PONG }
        private bool checkedFirstframe;

        public bool lastFrame
        {
            get {
                if (this.maxFrame != null)
                {
                    if (this.maxFrame[this.maxFrame.Count -1] < this.frameCount && this.tag != null) return true;
                }
                return false;
            }
        }

        public void play(GameTime gameTime, string tag, AnimationDirection aDirection = AnimationDirection.FORWARD)
        {
            if (tag != this.tag)
            {
                int i = 0;
                while (i < this.json.meta.frameTags.Count)
                {
                    if (tag == (string)this.json.meta.frameTags[i].name)
                    {
                        this.a_from       = (int)this.json.meta.frameTags[i].from;
                        this.a_to         = (int)this.json.meta.frameTags[i].to;
                        this.tag          = (string)this.json.meta.frameTags[i].name;
                        this.direction = aDirection;
                        this.frameCurrent = 0;
                        this.frameCount = 0;
                        this.maxFrame = new List<float>();
                        this.checkedFirstframe = false;
                        break;
                    }
                    i++;
                }

                i = 0;
                while (i + this.a_from <= this.a_to)
                {
                    int i_frame = this.a_from + i;
                    int last_frame = i - 1;

                    if (i > 0) this.maxFrame.Add(((float)this.json.frames[i_frame].duration) + this.maxFrame[last_frame]);
                    else this.maxFrame.Add((float)this.json.frames[i_frame].duration);

                    i++;
                }
            }

            float delta = (float)gameTime.ElapsedGameTime.Milliseconds;
            //this.maxFrame = ((float)(this.json.frames[this.frame].duration) / 1000f);
            this.frameCount += delta;

            if (this.frameCount >= this.maxFrame[this.frameCurrent] || (this.frameCurrent == 0 && !checkedFirstframe))
            {
                if (this.a_to > (this.frameCurrent + this.a_from) && checkedFirstframe) this.frameCurrent++;
                else if (this.direction == AnimationDirection.LOOP)
                {
                    this.frameCurrent = 0;
                    this.frameCount = 0;
                    this.checkedFirstframe = false;
                }

                frame = (int)(this.frameCurrent + this.a_from);
                dynamic frameInfo = this.json.frames[frame];

                Point size = new Point((int)(frameInfo.frame.h * this.sizeMutiply), (int)(frameInfo.frame.w * this.sizeMutiply));
                Point map = new Point((int)(frameInfo.frame.x * this.sizeMutiply), (int)(frameInfo.frame.y * this.sizeMutiply));
                this.spriteSourceSize = new Rectangle(map, size);

                   
                this.checkedFirstframe = true;
            }
           
        }
        #endregion

        public void DrawAnimation(SpriteBatch spriteBatch, Vector2 position, float scale)
        {
            // spriteBatch.Draw(this.sprite, position, this.spriteSourceSize, Color.White);
            spriteBatch.Draw(sprite, position, this.spriteSourceSize, Color.White, 0, new Vector2(0, 0), scale / this.sizeMutiply, SpriteEffects.None, 0f);
        }

        public void DrawInfo(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            if (this.tag != null) spriteBatch.DrawString(spriteFont, "animation: "+this.tag, new Vector2(0,0), Color.White);
            spriteBatch.DrawString(spriteFont, "Current frame: " + (this.frameCurrent + this.a_from), new Vector2(0, 25), Color.White);
        }
    }
}
