using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace CareerOpportunities
{
    public class MenuManagement : GameObject
    {
        public enum MenuItens { SELECT_PLAYER, CREDITS, EXIT, START, NONE };
        public MenuItens ItemOver;
        public MenuItens ItemSelected;
        
        public bool any_button;
        public bool any_button_press;
        public Vector2 PositionAnyButton;

        public SpriteFont Font;
        private Texture2D SpriteLogo;

        public string[] MenuItensString;
        private GraphicsDevice GraphicsDevice;
        private SoundEffect MusicMenu;
        private SoundEffect SoundConfirme;
        public SoundEffectInstance soundInstance;

        public MenuManagement(ContentManager content, SpriteFont Font, int scale, GraphicsDevice graphicsDevice)
        {
            this.Sprite = content.Load<Texture2D>("sprites/main_menu");
            this.SpriteSelectedCharacterHud = content.Load<Texture2D>("sprites/hud_select_character");
            this.SpritesSelectPlayerTextHud = content.Load<Texture2D>("sprites/select_player");
            this.SpriteLogo = content.Load<Texture2D>("sprites/skate_running_logo");
            this.MusicMenu = content.Load<SoundEffect>("Sound/Into the Depths");
            this.SoundConfirme = content.Load<SoundEffect>("Sound/sfx_sounds_button3");
            this.Scale = scale;
            this.GraphicsDevice = graphicsDevice;
            this.ItemOver = MenuItens.SELECT_PLAYER;
            this.ItemSelected = MenuItens.NONE;
            this.Position = new Vector2(140*this.Scale, 110*this.Scale);
            this.PositionAnyButton = new Vector2(120 * this.Scale,  130 * this.Scale);
            this.Font = Font;
            this.MenuItensString = new string[] {
                "START",
                //"OPTIONS",
                "CREDITS",
                "EXIT"
            };

            this.SetSizeString();
            this.SetSizeStringCredits();
            this.SetSizesStringMenuButtons();

            this.soundInstance = this.MusicMenu.CreateInstance();
            this.soundInstance.IsLooped = true;
        }
        
        private float[] MenuItensWidth;
        private void SetSizeString()
        {
            this.MenuItensWidth = new float[this.MenuItensString.Length];
            for (int i = 0; i < this.MenuItensString.Length; i++)
            {
                this.MenuItensWidth[i] = this.Font.MeasureString(this.MenuItensString[i]).X;
            }
        }

        private float[] CreditsWidth;
        private string[] CreditsItensString = new string[] {
                "PRODUCED BY",
                "Elton Silva",
                "SPECIAL THANKS",
                "Mom and Dad",
                "Mayra Carvalho",
                "www.eltonsilva.site",
                "[ESC] Back to main menu"
            };
        private void SetSizeStringCredits()
        {
            this.CreditsWidth = new float[this.CreditsItensString.Length];
            for (int i = 0; i < this.CreditsItensString.Length; i++)
                this.CreditsWidth[i] = this.Font.MeasureString(this.CreditsItensString[i]).X;
        }
        
        public void Update(GameTime gameTime, Controller.Input input)
        {
            if (this.soundInstance.State == SoundState.Stopped)
            {
                this.soundInstance.Play();
            }

            if (this.any_button && !any_button_press)
            {
                if (this.ItemSelected != MenuItens.SELECT_PLAYER)
                {
                    if (input.KeyPress(Controller.Input.Button.DOWN))
                    {
                        if ((int)this.ItemOver < 2)
                        {
                            int item = (int)this.ItemOver;
                            item += 1;

                            this.ItemOver = (MenuItens)item;
                            this.SoundConfirme.Play();
                        }
                    }

                    if (input.KeyPress(Controller.Input.Button.UP))
                    {
                        if ((int)this.ItemOver > 0)
                        {
                            int item = (int)this.ItemOver;
                            item -= 1;

                            this.ItemOver = (MenuItens)item;
                            this.SoundConfirme.Play();
                        }
                    }

                    if (input.KeyPress(Controller.Input.Button.CONFIRM) && this.ItemSelected == MenuItens.NONE)
                    {
                        this.ItemSelected = this.ItemOver;
                        this.SoundConfirme.Play();
                    }

                    if (input.KeyPress(Controller.Input.Button.ESC) && this.ItemSelected == MenuItens.CREDITS)
                    {
                        this.ItemSelected = MenuItens.NONE;
                        this.ItemOver = MenuItens.SELECT_PLAYER;
                        this.SoundConfirme.Play();
                    }
                }
                else
                {
                    if (input.KeyPress(Controller.Input.Button.RIGHT))
                    {
                        if ((int)this.CharacterOver == 0)
                        {
                            int item = (int)this.CharacterOver;
                            item += 1;

                            this.CharacterOver = (Characters)item;
                            this.SoundConfirme.Play();
                        }
                    }

                    if (input.KeyPress(Controller.Input.Button.LEFT))
                    {
                        if ((int)this.CharacterOver == 1)
                        {
                            int item = (int)this.CharacterOver;
                            item -= 1;

                            this.CharacterOver = (Characters)item;
                            this.SoundConfirme.Play();
                        }
                    }

                    if (input.KeyPress(Controller.Input.Button.CONFIRM))
                    {
                        this.CharacterSelected = this.CharacterOver;
                        this.ItemSelected = MenuItens.START;
                        this.SoundConfirme.Play();
                        this.soundInstance.Stop();
                    }

                    if (input.KeyPress(Controller.Input.Button.ESC))
                    {
                        this.ItemSelected = MenuItens.NONE;
                        this.ItemOver = MenuItens.SELECT_PLAYER;
                        this.SoundConfirme.Play();
                    }
                }
            } else
            {
                if (input.PressAnyButton())
                {
                    this.any_button_press = true;
                    this.any_button = true;
                    this.SoundConfirme.Play();
                }
                else this.any_button_press = false;
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.ItemSelected == MenuItens.NONE)
            {

                spriteBatch.Draw(this.SpriteLogo, new Vector2(((GraphicsDevice.Viewport.Width / 2f) - (this.SpriteLogo.Width / 2f)), 0), null, Color.White, 0, new Vector2(0,0), 1f, SpriteEffects.None, 0 );

                if (this.any_button)
                {
                    for (int i = 0; i < this.MenuItensString.Length; i++)
                    {
                        Vector2 position = new Vector2((GraphicsDevice.Viewport.Width / 2f) - ((this.MenuItensWidth[i]) / 2f), this.Position.Y + ((i * 10) * this.Scale));
                        this.SpriteColor = Color.Gray;
                        if ((MenuItens)i == this.ItemOver) this.SpriteColor = Color.White;
                        spriteBatch.DrawString(this.Font, this.MenuItensString[i], position, this.SpriteColor);
                    }
                }
                else spriteBatch.DrawString(this.Font, "Press Any Button", this.PositionAnyButton, Color.White);
            }

            this.DrawCredits(spriteBatch);
            this.DrawSelectPlayer(spriteBatch);
        }
        
        private float[] MenuButtonsWidth;
        private string[] MenuButtonsString = new string[]
        {
            "[ENTER] Confirm",
            "[ESC] Back to main menu"
        };
        private void SetSizesStringMenuButtons()
        {
            this.MenuButtonsWidth = new float[this.MenuButtonsString.Length];
            for (int i = 0; i < this.MenuButtonsString.Length; i++)
                this.MenuButtonsWidth[i] = this.Font.MeasureString(this.MenuButtonsString[i]).X;
        }

        public enum Characters { JIM, JOSIE, NONE };
        public Characters CharacterOver = Characters.JIM;
        public Characters CharacterSelected = Characters.NONE;
        public Texture2D SpriteSelectedCharacterHud;
        public Texture2D SpritesSelectPlayerTextHud;

        public void DrawSelectPlayer(SpriteBatch spriteBatch)
        {
            if (this.ItemSelected == MenuItens.SELECT_PLAYER)
            {
                float ScaleHud = 6f;
                Vector2 PositionJim = new Vector2((GraphicsDevice.Viewport.Width / 4f) + (20 * ScaleHud) - (18 / 2f * ScaleHud), (GraphicsDevice.Viewport.Height / 2f) - (40 / 2f * ScaleHud));
                Vector2 PositionJosie = new Vector2((GraphicsDevice.Viewport.Width / 2f) + (GraphicsDevice.Viewport.Width / 4f) - (20 * ScaleHud) - (18 / 2f * ScaleHud), (GraphicsDevice.Viewport.Height / 2f) - (40 / 2f * ScaleHud));

                if(this.CharacterOver == Characters.JIM)
                    spriteBatch.Draw(this.SpriteSelectedCharacterHud, PositionJim, new Rectangle(new Point(0, 0), new Point(18, 40)), Color.White, 0f, Vector2.Zero, ScaleHud, SpriteEffects.None, 0f);
                else if (this.CharacterOver == Characters.JOSIE)
                    spriteBatch.Draw(this.SpriteSelectedCharacterHud, PositionJosie, new Rectangle(new Point(0, 0), new Point(18, 40)), Color.White, 0f, Vector2.Zero, ScaleHud, SpriteEffects.None, 0f);

                spriteBatch.Draw(this.SpriteSelectedCharacterHud, PositionJim, new Rectangle(new Point(0, 40), new Point(18, 40)), Color.White, 0f, Vector2.Zero, ScaleHud, SpriteEffects.None, 0f);
                spriteBatch.Draw(this.SpriteSelectedCharacterHud, PositionJosie, new Rectangle(new Point(18, 40), new Point(18, 40)), Color.White, 0f, Vector2.Zero, ScaleHud, SpriteEffects.None, 0f);

                Vector2 position;
                position = new Vector2( 40 * this.Scale , 150 * this.Scale);
                spriteBatch.DrawString(this.Font, this.MenuButtonsString[0], position, Color.White);

                position = new Vector2(170 * this.Scale, 150 * this.Scale);
                spriteBatch.DrawString(this.Font, this.MenuButtonsString[1], position, Color.White);

                position = new Vector2((GraphicsDevice.Viewport.Width / 2f) - (this.SpritesSelectPlayerTextHud.Width * 0.7f / 2f), 15 * this.Scale);
                spriteBatch.Draw(this.SpritesSelectPlayerTextHud, position, null, Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);
            }
        }

        public void DrawCredits(SpriteBatch spriteBatch)
        {
            if (this.ItemSelected == MenuItens.CREDITS)
            {
                Vector2 position = new Vector2((GraphicsDevice.Viewport.Width / 2f) - ((this.CreditsWidth[0]) / 2f), 30 *this.Scale);
                spriteBatch.DrawString(this.Font, this.CreditsItensString[0], position, this.SpriteColor);

                position = new Vector2((GraphicsDevice.Viewport.Width / 2f) - ((this.CreditsWidth[1]) / 2f), 40 * this.Scale);
                spriteBatch.DrawString(this.Font, this.CreditsItensString[1], position, Color.White);

                position = new Vector2((GraphicsDevice.Viewport.Width / 2f) - ((this.CreditsWidth[2]) / 2f), 60 * this.Scale);
                spriteBatch.DrawString(this.Font, this.CreditsItensString[2], position, this.SpriteColor);

                position = new Vector2((GraphicsDevice.Viewport.Width / 2f) - ((this.CreditsWidth[3]) / 2f), 70 * this.Scale);
                spriteBatch.DrawString(this.Font, this.CreditsItensString[3], position, Color.White);

                position = new Vector2((GraphicsDevice.Viewport.Width / 2f) - ((this.CreditsWidth[4]) / 2f), 80 * this.Scale);
                spriteBatch.DrawString(this.Font, this.CreditsItensString[4], position, Color.White);

                position = new Vector2((GraphicsDevice.Viewport.Width / 2f) - ((this.CreditsWidth[5]) / 2f), 110 * this.Scale);
                spriteBatch.DrawString(this.Font, this.CreditsItensString[5], position, Color.White);

                position = new Vector2((GraphicsDevice.Viewport.Width / 2f) - ((this.CreditsWidth[6]) / 2f), 150 * this.Scale);
                spriteBatch.DrawString(this.Font, this.CreditsItensString[6], position, Color.White);
            }
        }
    }
}
