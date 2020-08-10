using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using CareerOpportunities.weapon;
using CareerOpportunities.Controller;
using CareerOpportunities.Hud;
using CareerOpportunities.Routine;
using System.Reflection;
using System.IO;

namespace CareerOpportunities
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Input InputGK;

        Tutorial Tutorial;

        Texture2D loadingScreen;
        Texture2D GameOverScreen;
        Texture2D Bar;

        SoundEffect SoundTrack;
        SoundEffectInstance BackgroundSoundTrack;

        SoundEffect SoundTrackBoss;
        SoundEffectInstance BackgroundSoundTrackBoss;

        public int Level;
        public enum GameStatus { WIN, LOSE, PLAY, PAUSE, MENU }
        public GameStatus status;

        public bool LoadingLevel;
        public bool LoadingMenu;

        public Transition Transition;

        PauseMenuManagement PauseMenu;
        PauseMenuManagement GameOverMenu;

        CutScene CutScene;

        public CameraManagement camera;

        SpriteFont font3;

        bool debug;
        public string path;

        public int scale;
        private bool mute = false;
        int screemGameHeight = 162;
        int screemGameWidth = 288;
        public Game1()
        {

            this.scale = 3;
            this.Level = 1;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 316 * this.scale;
            graphics.PreferredBackBufferHeight = 178 * this.scale;
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            //Window.AllowUserResizing = true;
            //graphics.ToggleFullScreen();
            Content.RootDirectory = "Content";
            this.LoadingLevel = false;
            this.LoadingMenu = true;
            this.path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            debug = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            camera = new CameraManagement();
            camera.scale = 3;
            this.InputGK = new Input();
            Window.Title = "Skate Running";
        }

        protected override void LoadContent()
        {
            spriteBatch   = new SpriteBatch(GraphicsDevice);

            this.status   = GameStatus.MENU;
            loadingScreen = Content.Load<Texture2D>("sprites/loading");
            this.font3    = Content.Load<SpriteFont>("pressstart3");
            this.Bar      = Content.Load<Texture2D>("sprites/hud_gameover");

            this.SoundTrack = Content.Load<SoundEffect>("Sound/Juhani Junkala [Chiptune Adventures] 2 Stage 2");
            this.BackgroundSoundTrack = this.SoundTrack.CreateInstance();
            this.BackgroundSoundTrack.IsLooped = true;
            this.BackgroundSoundTrack.Volume = 0.4f;

            this.SoundTrackBoss = Content.Load<SoundEffect>("Sound/Juhani Junkala [Chiptune Adventures] 3 Boss Fight");
            this.BackgroundSoundTrackBoss = this.SoundTrackBoss.CreateInstance();
            this.BackgroundSoundTrack.IsLooped = true;
            this.BackgroundSoundTrack.Volume = 0.4f;

            this.Transition = new Transition(Content, 316, this.scale);

            this.backgroundLayer = new RenderTarget2D(this.GraphicsDevice, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
            this.PlayerLayer     = new RenderTarget2D(this.GraphicsDevice, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
            this.lightmapLayer   = new RenderTarget2D(this.GraphicsDevice, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
            this.weaponLayer     = new RenderTarget2D(this.GraphicsDevice, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);

            this.allLayers       = new RenderTarget2D(this.GraphicsDevice, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);

            this.HUDlayer        = new RenderTarget2D(this.GraphicsDevice, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);

        }

        
        protected override void UnloadContent()
        {
        }

#region Update

        private void goToMenu()
        {
            this.Level = 1;
            this.status = GameStatus.MENU;
            this.MainMenu.ItemSelected = MenuManagement.MenuItens.NONE;
        }

        protected override void Update(GameTime gameTime)
        {
            if (this.status != GameStatus.MENU && !mute)
            {
                if (this.Boss != null && this.Boss.isBossLevel(Level))
                {
                    if (this.BackgroundSoundTrackBoss.State == SoundState.Stopped) this.BackgroundSoundTrackBoss.Play();
                    else this.BackgroundSoundTrack.Stop();
                }
                else
                {
                    if (this.BackgroundSoundTrack.State == SoundState.Stopped) this.BackgroundSoundTrack.Play();
                    else this.BackgroundSoundTrackBoss.Stop();
                }
               
            }
            else
            {
                if (this.BackgroundSoundTrackBoss.State == SoundState.Playing) this.BackgroundSoundTrackBoss.Stop();
                if (this.BackgroundSoundTrack.State == SoundState.Playing) this.BackgroundSoundTrack.Stop();
            }

            if (this.LoadingLevel)
            {
                this.LoadingLevel = false;
                this.LoadLevel();
            }

            if (this.LoadingMenu)
            {
                this.LoadingMenu = false;
                this.LoadMenu();
            }

            if (this.isLevelReady())
            {
                if (this.status == GameStatus.PLAY)
                {
                    this.CutScene.Update(gameTime);
                    if (this.CutScene.IsFinished)
                    {
                        this.Transition.Update(gameTime);
                        if (!this.Transition.animation && !this.Tutorial.ShowTutorial) this.Countdown.Update(gameTime);
                        this.Tutorial.Update(gameTime, this.Level, this.InputGK);
                    }

                    Vector2 screemSize = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                    this.camera.Update(gameTime, Player.Position, screemSize);
                    this.Map.Update(gameTime, Player);
                    this.Coins.Update(gameTime);
                    this.Hearts.Update(gameTime);
                    this.Level = Map.CurrentlyLevel;
                    if (this.Level > 1) this.Player.Update(gameTime, this.InputGK, camera);
                    this.Weapon.Update(gameTime);
                    if(this.Boss != null && this.Boss.isBossLevel(this.Level) && !this.Transition.IsHide) this.Boss.Update(gameTime, camera);


                    // cut scene done, next level
                    // boss is not visible
                    if (this.Boss.Position.X - 100 > graphics.PreferredBackBufferWidth)
                    {
                        if (!this.Transition.animation) this.Transition.HideScreem();
                        if (this.Transition.IsHide)
                        {
                            this.Level = Map.NextLevel();
                            if (this.Level <= Map.LastLevel) this.status = GameStatus.WIN;
                        }
                    }
                    

                    if (Hearts.NumberOfhearts < 1)
                    {
                        if (!Map.isStoped) {
                            //this.LoadingLevel = true;
                            this.status = GameStatus.LOSE;
                            this.GameOverMenu.ItemSelected = PauseMenuManagement.MenuStatus.NONE;
                            this.Transition.Hide();
                        }
                    }

                    if (this.InputGK.KeyPress(Input.Button.ESC))
                    {
                        this.status = GameStatus.PAUSE;
                        this.PauseMenu.ItemSelected = PauseMenuManagement.MenuStatus.NONE;
                    }
                }
                if (Map != null && Map.Finished())
                {
                    if (!this.Transition.animation) this.Transition.HideScreem();
                    if (this.Transition.IsHide)
                    {
                        this.Level = Map.NextLevel();
                        if (this.Level <= Map.LastLevel) this.status = GameStatus.WIN;
                        else
                        {
                            this.Level = 1;
                            this.Map = null;
                            this.status = GameStatus.MENU;
                            this.MainMenu.ItemSelected = MenuManagement.MenuItens.CREDITS;
                            this.MainMenu.ItemOver = MenuManagement.MenuItens.CREDITS;
                        }

                    }
                }

            }

            if (this.isMainMenuReady())
            {
                if (this.status == GameStatus.PAUSE)
                {   
                    this.PauseMenu.Update(gameTime, this.InputGK);
                    if (this.PauseMenu.ItemSelected == PauseMenuManagement.MenuStatus.RESUME)
                    {
                        this.status = GameStatus.PLAY;
                    }
                    else if (this.PauseMenu.ItemSelected == PauseMenuManagement.MenuStatus.EXIT)
                    {
                        this.goToMenu();
                    }
                }

                if (this.status == GameStatus.LOSE)
                {
                    this.GameOverMenu.Update(gameTime, this.InputGK);
                    if (this.GameOverMenu.ItemSelected == PauseMenuManagement.MenuStatus.RESUME)
                    {
                        this.status = GameStatus.PLAY;
                        this.LoadingLevel = true;
                        this.MainMenu.ItemSelected = MenuManagement.MenuItens.NONE;
                    }
                    else if (this.GameOverMenu.ItemSelected == PauseMenuManagement.MenuStatus.EXIT)
                    {
                        this.status = GameStatus.MENU;
                        this.MainMenu.ItemSelected = MenuManagement.MenuItens.NONE;
                    }
                }

                if (this.status == GameStatus.WIN)
                {
                    Map = null;
                    Countdown = null;
                    this.LoadingLevel = true;
                }

                if (this.status == GameStatus.MENU)
                {
                    MainMenu.Update(gameTime, this.InputGK);
                    if (MainMenu.ItemSelected == MenuManagement.MenuItens.START)
                    {
                        this.status = GameStatus.PLAY;
                        this.LoadingLevel = true;
                    }
                    if (MainMenu.ItemSelected == MenuManagement.MenuItens.EXIT)
                    {
                        Exit();
                    }
                }
            }
            base.Update(gameTime);
        }
#endregion

#region Draw Player
        protected override void Draw(GameTime gameTime)
        {
            // Start Game
            this.DrawPlay();
            // end Game

            // Loading
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            if ((!this.isLevelReady() && !this.isMainMenuReady()) || (!this.isLevelReady() && this.isMainMenuReady() && this.status == GameStatus.PLAY) || (this.status == GameStatus.WIN) )
            {
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Draw(this.loadingScreen, new Vector2(0, 0), null, Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
            }
            spriteBatch.End();
            // end loading

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            this.DrawMainMenu();
            this.DrawPauseMenu();
            this.DrawGameOverMenu();
            spriteBatch.End();

            base.Draw(gameTime);
        }

#region Draw Player Layers
        // layers
        private RenderTarget2D backgroundLayer;
        private RenderTarget2D PlayerLayer;
        private RenderTarget2D lightmapLayer;
        private RenderTarget2D weaponLayer;
        private RenderTarget2D allLayers;

        public void DrawPlay()
        {
            if (this.isLevelReady() && this.status == GameStatus.PLAY)
            {
                //Effect layer
                GraphicsDevice.SetRenderTarget(this.lightmapLayer);
                GraphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, null);
                Map.Layers(spriteBatch, Player.CurrentVerticalLine, true, false);
                if (this.Boss != null && this.Boss.isBossLevel(this.Level)) Boss.Draw(spriteBatch);
                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, null, null, null, null);
                if (this.Level > 1) Player.Draw(spriteBatch);
                spriteBatch.End();
                GraphicsDevice.SetRenderTarget(null);

                // Render background layer
                GraphicsDevice.SetRenderTarget(this.backgroundLayer);
                GraphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, null);
                Map.DrawGround(spriteBatch);
                Map.Layers(spriteBatch, Player.CurrentVerticalLine, false, false);
                if (this.Level > 1) Player.Draw(spriteBatch);
                spriteBatch.End();
                GraphicsDevice.SetRenderTarget(null);

                // Render weapon layer
                GraphicsDevice.SetRenderTarget(this.weaponLayer);
                GraphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, null);
                Weapon.Draw(spriteBatch);
                spriteBatch.End();
                GraphicsDevice.SetRenderTarget(null);

                // Join all layers
                GraphicsDevice.SetRenderTarget(this.allLayers);
                GraphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.transformMatrix());
                spriteBatch.Draw((Texture2D)this.backgroundLayer, Vector2.Zero, Color.White);
                spriteBatch.Draw((Texture2D)this.lightmapLayer, Vector2.Zero, Color.White);
                spriteBatch.Draw((Texture2D)this.weaponLayer, Vector2.Zero, Color.White);
                spriteBatch.End();
                GraphicsDevice.SetRenderTarget(null);

                this.DrawHUDPlay();

                this.DrawAllLayers();
                
            }
        }
#endregion

#region Draw HUD Layer
        private RenderTarget2D HUDlayer;

        public void DrawHUDPlay()
        {
            if (this.isLevelReady() && this.status == GameStatus.PLAY)
            {
                GraphicsDevice.SetRenderTarget(this.HUDlayer);
                GraphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

                if (!Boss.isBossLevel(this.Level))
                {
                    Hearts.Draw(spriteBatch);
                    Coins.Draw(spriteBatch);
                    spriteBatch.Draw(this.Character, new Vector2(3 * this.scale, 3 * this.scale), new Rectangle(new Point(0, 0), new Point(27, 27)), Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
                } else if (this.Level > 1) Boss.DrawHud(spriteBatch);
                
                if(this.Level > 1)
                {
                    Countdown.PostionToCenter(new Vector2(HUDlayer.Width, HUDlayer.Height), new Vector2(400*0.4f, 400*0.4f));
                    Countdown.Draw(spriteBatch);
                    Tutorial.Draw(spriteBatch, graphics.PreferredBackBufferWidth, font3);
                }

                this.Transition.Draw(spriteBatch);
                if (!this.CutScene.IsFinished) this.CutScene.Draw(spriteBatch);
                spriteBatch.End();
                GraphicsDevice.SetRenderTarget(null);
            }
        }
#endregion

        public void DrawAllLayers()
        {
            float widthCenter = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2;
            float heightCenter = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2;
            // Vector2 PositionScreem = new Vector2(widthCenter - (this.backgroundLayer.Width / 2), heightCenter - (this.backgroundLayer.Height / 2));
            Vector2 PositionScreem = Vector2.Zero;

            // render all layers
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null);
            spriteBatch.Draw((Texture2D)this.allLayers, PositionScreem, Color.White);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            spriteBatch.Draw((Texture2D)this.HUDlayer, PositionScreem, Color.White);
            spriteBatch.End();
        }

#endregion

#region Draw Menu
        public void DrawMainMenu()
        {
            if (this.isMainMenuReady() && this.status == GameStatus.MENU)
            {
                GraphicsDevice.Clear(Color.Black);
                this.MainMenu.Draw(spriteBatch);
            }
        }

        public void DrawPauseMenu()
        {
            if (this.isMainMenuReady() && this.status == GameStatus.PAUSE)
            {
                GraphicsDevice.Clear(Color.Black);
                float x_start = (GraphicsDevice.Viewport.Width - (0.7f * this.GameOverScreen.Width)) / 2f;
                this.PauseMenu.Draw(spriteBatch, x_start);
            }
        }

        public void DrawGameOverMenu()
        {
            if (this.isMainMenuReady() && this.status == GameStatus.LOSE)
            {
                GraphicsDevice.Clear(Color.Black);
                float x_start = (GraphicsDevice.Viewport.Width - (0.7f * this.GameOverScreen.Width)) / 2f;
                spriteBatch.Draw(this.GameOverScreen, new Vector2(x_start, 100), null, Color.White, 0, new Vector2(0, 0), 0.7f, SpriteEffects.None, 0f);
                this.GameOverMenu.Draw(spriteBatch, x_start);

                int y = this.scale * 95;
                float x = ((GraphicsDevice.Viewport.Width - (this.scale * 160)) / 2f) / scale;
                float progress = ((this.Map.FinishedPerCent() / 100) * (150));
                bool show_progress = false;

                spriteBatch.Draw(this.Bar, new Vector2(x * this.scale, y), new Rectangle(new Point(0,0), new Point(5, 14)), Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
                x += 4;
                for (int i = 3; i < 150; i++)
                {
                    x++;
                    if(progress > i)
                    {
                        spriteBatch.Draw(this.Bar, new Vector2(x * this.scale, y), new Rectangle(new Point(4, 0), new Point(1, 14)), Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
                    } else
                    {
                        spriteBatch.Draw(this.Bar, new Vector2(x * this.scale, y), new Rectangle(new Point(5, 0), new Point(1, 14)), Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
                        if (!show_progress)
                        {
                            spriteBatch.Draw(this.Bar, new Vector2((x - 5) * this.scale, y - (13 * this.scale)), new Rectangle(new Point(10, 0), new Point(10, 13)), Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
                            show_progress = true;
                        }
                    }
                       
                }
                x++;
                spriteBatch.Draw(this.Bar, new Vector2(x * this.scale, y), new Rectangle(new Point(5,0), new Point(5, 14)), Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
            }
        }

#endregion

#region Load Content

        public Texture2D Character;
        public Texture2D JimSpriteHud;
        public Texture2D JosieSpriteHud;
        public MenuManagement MainMenu;
        public HeartManagement Hearts;
        public Hud.Countdown Countdown;

        public CoinManagement Coins;
        public PlayerController Player;
        public Boss Boss;
        public Level.Render Map;
        public Gun Weapon;

        // Load all assets to the level
        public void LoadLevel()
        {

            JimSpriteHud = Content.Load<Texture2D>("sprites/jim_hud");
            JosieSpriteHud = Content.Load<Texture2D>("sprites/josie_hud");

            if (MainMenu.CharacterSelected == MenuManagement.Characters.JIM) Character = JimSpriteHud;
            else if (MainMenu.CharacterSelected == MenuManagement.Characters.JOSIE) Character = JosieSpriteHud;

            this.Tutorial = new Tutorial(Content, this.scale);
            this.CutScene = new CutScene(this.font3, new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), this.Level);

            Hearts = new HeartManagement(Content.Load<Texture2D>("sprites/heart"));
            Hearts.Content = Content;
            Hearts.Scale = scale;

            Coins = new CoinManagement(Content.Load<Texture2D>("sprites/coin-hud"), this.font3, this.scale);
            Coins.Content = Content;

            Countdown = new Countdown(this);

            Player = new PlayerController(graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth, this);

            Map = new Level.Render(scale, graphics.PreferredBackBufferHeight, this.screemGameWidth * this.scale);
            Map.setLevel(this.Level);
            Map.jsonContent = null;
            Map.SetBackground(Content, this.Level);
            Map.Sprite = Content.Load<Texture2D>("sprites/tiles");
            Map.setCoinTexture(Content.Load<Texture2D>("sprites/coin"), this.path +"/Content/sprites/coin.json");
            if(this.Level == 1 || this.Level == 2) Map.setTileMap(Content.Load<Texture2D>("Maps/level_1"));
            else Map.setTileMap(Content.Load<Texture2D>("Maps/level_"+(this.Level-1)));
            Map.countdown = this.Countdown;

            Weapon = new Gun(this, this.camera);
            Weapon.Screem = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            Boss = new Boss(this);

            // start game
            this.status = GameStatus.PLAY;
            this.Transition.ShowScreem();


        }

        public void LoadMenu()
        {
            this.MainMenu = new MenuManagement(Content, this.font3, this.scale, GraphicsDevice);
            this.PauseMenu = new PauseMenuManagement(Content.Load<Texture2D>("sprites/pause_menu"), this.scale);
            this.PauseMenu.Font = this.font3;
            this.GameOverMenu = new PauseMenuManagement(Content.Load<Texture2D>("sprites/pause_menu"), this.scale);
            this.GameOverMenu.gameOver = true;
            this.GameOverMenu.Font = this.font3;
            this.GameOverScreen = Content.Load<Texture2D>("sprites/game_over");
            this.status = GameStatus.MENU;
        }

        public bool isLevelReady()
        {
            if (Hearts != null && Player != null && Map != null && Coins != null && Weapon != null && Weapon.AnimationIsReady() && Countdown != null){
                if (Map.AnimationIsReady())return true;
            }
            return false;
        }

        public bool isMainMenuReady()
        {
            if (this.MainMenu != null && this.PauseMenu != null) return true;
            return false;
        }
#endregion

    }
}
