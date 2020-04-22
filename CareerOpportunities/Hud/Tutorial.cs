using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace CareerOpportunities.Hud
{
    public class Tutorial : GameObject
    {

        private SoundEffect SoundConfirme;

        public Tutorial(ContentManager Content, int Scale)
        {
            this.Sprite = Content.Load<Texture2D>("sprites/hud_tutorial_dialogue_box");
            this.Scale = Scale;
            this.SoundConfirme = Content.Load<SoundEffect>("Sound/sfx_sounds_button3");
        }


        public bool ShowTutorial = true;
        public void Update(GameTime gameTime, int Level, Controller.Input input)
        {
            if (this.ShowTutorial)
            {
                switch (Level)
                {
                    case 2:
                        this.DodgeObstacles();
                        break;
                    case 4:
                        this.Fire();
                        break;
                    default:
                        this.ShowTutorial = false;
                        break;
                }

                // exit tutorial dialogue
                if (input.KeyPress(Controller.Input.Button.CONFIRM) || input.KeyPress(Controller.Input.Button.ESC))
                {
                    this.ShowTutorial = false;
                    this.SoundConfirme.Play();
                }
                    
            }
            
        }


        public void Draw(SpriteBatch spriteBatch, float Width, SpriteFont font)
        {
            if (this.ShowTutorial)
            {
                int x = (int)((Width / 2f) - (this.Size.X * this.Scale / 2f));
                int y = 30 * this.Scale;

                spriteBatch.Draw(this.Sprite, new Vector2(x-(2*this.Scale), y-(2 * this.Scale)), new Rectangle(new Point(0, 0), new Point(2, 2)), Color.White, 0, new Vector2(0, 0), this.Scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(this.Sprite, new Rectangle(new Point(x, y-(2* this.Scale)), new Point((int)this.Size.X * this.Scale,  (2 * this.Scale))), new Rectangle(new Point(2, 0), new Point(1, 2)), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0f);
                spriteBatch.Draw(this.Sprite, new Vector2(x - (1 * this.Scale) + (this.Size.X * this.Scale), y - (2 * this.Scale)), new Rectangle(new Point(0, 0), new Point(2, 2)), Color.White, 0, new Vector2(0, 0), this.Scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(this.Sprite, new Rectangle(new Point(x - (2 * this.Scale), y), new Point(2 * this.Scale, (int)this.Size.Y * this.Scale)), new Rectangle(new Point(0, 2), new Point(2, 1)), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0f);

                spriteBatch.Draw(this.Sprite, new Rectangle(new Point(x,y), new Point((int)this.Size.X * this.Scale, (int)this.Size.Y * this.Scale)), new Rectangle(new Point(2, 2), new Point(1, 1)), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0f);

                spriteBatch.Draw(this.Sprite, new Vector2(x - (2 * this.Scale), y + (this.Size.Y * this.Scale)), new Rectangle(new Point(0, 4), new Point(2, 2)), Color.White, 0, new Vector2(0, 0), this.Scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(this.Sprite, new Rectangle(new Point(x, y + (int)(this.Size.Y * this.Scale)), new Point((int)this.Size.X * this.Scale, (2 * this.Scale))), new Rectangle(new Point(2, 4), new Point(1, 2)), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0f);
                spriteBatch.Draw(this.Sprite, new Vector2(x + (this.Size.X * this.Scale), y + (this.Size.Y * this.Scale)), new Rectangle(new Point(4, 4), new Point(2, 2)), Color.White, 0, new Vector2(0, 0), this.Scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(this.Sprite, new Rectangle(new Point(x + (int)(this.Size.X * this.Scale), y), new Point(2 * this.Scale, (int)this.Size.Y * this.Scale)), new Rectangle(new Point(4, 2), new Point(2, 1)), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0f);

                for (int i = 0; i < this.Dialugue.Count; i++)
                {
                    spriteBatch.DrawString(font, this.Dialugue[i], new Vector2( x + (this.Scale * 5), y + (this.Scale * 5)), Color.White, 0, new Vector2(0,0), 1, SpriteEffects.None, 0);
                }
            }
        }


        #region tutorials settings

        private Vector2 Size = new Vector2(150, 70);
        private List<string> Dialugue = new List<string>();

        private void DodgeObstacles()
        {
            this.Dialugue.Add("use [up] or [down] to dodge\nobstacles on the street.\n\nPress [enter] to continue");
        }

        private void Fire()
        {
            this.Dialugue.Add("use [x] to fire to destroy boxs\nand cones.\n\nPress [enter] to continue");
        }

        #endregion


    }
}
