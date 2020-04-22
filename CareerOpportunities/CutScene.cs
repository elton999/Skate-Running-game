using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CareerOpportunities
{
    public class CutScene
    {
        private SpriteFont Font;
        //private List<Texture2D> Sprites;
        private Vector2 Size;
        private int Level;

        public CutScene(SpriteFont font, Vector2 size, int level)
        { 

            this.Font = font;
            this.Size = size;
            this.Level = level;
        }
        
        private string Dialogue;
        private void DialogueLevel1()
        {
            this.Dialogue = "The race started!";
        }

        private void DialogueLevel2()
        {
            this.Dialogue = "And you are too late...";
        }
        
        private void SetLevelString()
        {
            this.Dialogue = "LEVEL "+(this.Level - 1);
        }

        private float timeTotal;
        public bool IsFinished {
            get => (this.timeTotal >= 3000);
        }

        private float position_x;
        private void SetPosition()
        {
            this.position_x = this.Font.MeasureString(this.Dialogue).X;
        }

        public void Update(GameTime gameTime)
        {
            if (this.position_x == 0)
            {
                switch (this.Level)
                {
                    case 1:
                        this.DialogueLevel1();
                        break;
                    case 2:
                        this.DialogueLevel2();
                        break;
                    default:
                        this.SetLevelString();
                        break;
                }
                this.SetPosition();
            }

            this.timeTotal += gameTime.ElapsedGameTime.Milliseconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!this.IsFinished)
            {
                spriteBatch.DrawString(this.Font, this.Dialogue, new Vector2((this.Size.X / 2f) - (position_x/2f), this.Size.Y / 2f), Color.White);
            }
        }

    }
}
