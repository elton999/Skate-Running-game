using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using CareerOpportunities.weapon;
using CareerOpportunities.Controller;
using CareerOpportunities.Routine;
using System.Reflection;
using System.IO;


namespace CareerOpportunities
{
   
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D Character;
        Gun Weapon;
        Input InputGK;

        Texture2D loadingScreen;
        Texture2D GameOverScreen;

        public int Level;
        public enum GameStatus { WIN, LOSE, PLAY, PAUSE, MENU }
        public GameStatus status;

        public bool LoadingLevel;
        public bool LoadingMenu;

        MenuManagement MainMenu;
        HeartManagement Hearts;
        CoinManagement Coins;
        PlayerController Player;
        Boss Boss;
        Level.Render Map;

        PauseMenuManagement PauseMenu;
        PauseMenuManagement GameOverMenu;

        CameraManagement camera;

        SpriteFont font3;

        // Effect
        private Effect ghostLightShader;
        private Texture2D background;
        private Texture2D lightmap;
        private Texture2D ghost;
        private RenderTarget2D lightmapLayer;
        private RenderTarget2D sceneLightLayer;
        private RenderTarget2D ghostLayer;
        private RenderTarget2D ghostLightLayer;
        private BlendState lightBlend;

        bool debug;
        public string path;

        int scale;
        
        public Game1()
        {
            this.scale = 3;
            this.Level = 1;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 288 * scale;
            graphics.PreferredBackBufferHeight = 162 * scale;
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            // graphics.ToggleFullScreen();
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
        }

        protected override void LoadContent()
        {
            spriteBatch   = new SpriteBatch(GraphicsDevice);

            this.status = GameStatus.MENU;
            loadingScreen = Content.Load<Texture2D>("sprites/loading");
            this.font3 = Content.Load<SpriteFont>("pressstart3");

            this.lightBlend = new BlendState
            {
                ColorSourceBlend = Blend.Zero,
                ColorDestinationBlend = Blend.SourceColor
            };

            this.ghostLightLayer = new RenderTarget2D(this.GraphicsDevice, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
            this.ghostLayer = new RenderTarget2D(this.GraphicsDevice, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
            this.lightmapLayer = new RenderTarget2D(this.GraphicsDevice, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
            this.sceneLightLayer = new RenderTarget2D(this.GraphicsDevice, this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
        }

        
        protected override void UnloadContent()
        {
        }
        
        protected override void Update(GameTime gameTime)
        {

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
                    Vector2 screemSize = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                    this.camera.Update(gameTime, Player.Position, screemSize);
                    this.Map.Update(gameTime, 130);
                    this.Level = Map.CurrentlyLevel;
                    this.Player.Update(gameTime, this.InputGK, Map, Hearts, Coins, camera, Weapon);
                    this.Weapon.Update(gameTime, Map);
                    this.Boss.Update(gameTime);

                    if (Hearts.NumberOfhearts == 0)
                    {
                        if (Map.isStoped()) {
                            //this.LoadingLevel = true;
                            this.status = GameStatus.LOSE;
                            this.GameOverMenu.ItemSelected = PauseMenuManagement.MenuStatus.NONE;
                        }

                    }

                    if (this.InputGK.KeyPress(Input.Button.ESC))
                    {
                        this.status = GameStatus.PAUSE;
                        this.PauseMenu.ItemSelected = PauseMenuManagement.MenuStatus.NONE;
                    }
                }
                if (Map.Finished())
                {
                    this.Level = Map.NextLevel();   
                    this.status = GameStatus.WIN;
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
                        this.status = GameStatus.MENU;
                        this.MainMenu.ItemSelected = MenuManagement.MenuItens.NONE;
                    }
                }

                if (this.status == GameStatus.LOSE)
                {
                    this.GameOverMenu.Update(gameTime, this.InputGK);
                    if (this.GameOverMenu.ItemSelected == PauseMenuManagement.MenuStatus.RESUME)
                    {
                        this.status = GameStatus.PLAY;
                        this.LoadingLevel = true;
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
                    if (MainMenu.ItemSelected == MenuManagement.MenuItens.EXIT) Exit();
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Start Game
            this.DrawPlay();
            // end Game

            // Menu and Game HUD
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            if ((!this.isLevelReady() && !this.isMainMenuReady()) || (!this.isLevelReady() && this.isMainMenuReady() && this.status == GameStatus.PLAY) || (this.status == GameStatus.WIN) )
            {
                spriteBatch.Draw(this.loadingScreen, new Vector2(0, 0), null, Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
            }

            // HUD
            this.DrawHUDPlay();

            this.DrawMainMenu();
            this.DrawPauseMenu();
            this.DrawGameOverMenu();

            spriteBatch.End();
            // end menu and HUD

            base.Draw(gameTime);
        }


        public void DrawPlay()
        {
            if (this.isLevelReady() && this.status == GameStatus.PLAY)
            {

                //Effect
                GraphicsDevice.SetRenderTarget(this.ghostLightLayer);
                GraphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, null);
                Boss.Draw(spriteBatch);
                Map.Layers(spriteBatch, Player.CurrentVerticalLine, true, false);
                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, null, null, null, null);
                Player.Draw(spriteBatch);
                spriteBatch.End();
                GraphicsDevice.SetRenderTarget(null);

                //final

                // Render scene
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.transformMatrix());
                GraphicsDevice.Clear(Color.CornflowerBlue);
                Map.DrawGround(spriteBatch);
                Map.Layers(spriteBatch, Player.CurrentVerticalLine, false, false);
                Player.Draw(spriteBatch);
                //Boss.Draw(spriteBatch);
                //Map.Layers(spriteBatch, Player.CurrentVerticalLine, true);
                spriteBatch.Draw((Texture2D)this.ghostLightLayer, Vector2.Zero, Color.White);
                spriteBatch.End();

                Weapon.Draw(spriteBatch, camera);
            }
        }

        public void DrawHUDPlay()
        {
            if (this.isLevelReady() && this.status == GameStatus.PLAY)
            {
                Hearts.Draw(spriteBatch);
                Coins.Draw(spriteBatch);
                spriteBatch.Draw(this.Character, new Vector2(3 * this.scale, 3 * this.scale), new Rectangle(new Point(0, 0), new Point(27, 27)), Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
            }
        }


        // Load all assets to the level
        public void LoadLevel()
        {
            Character = Content.Load<Texture2D>("sprites/jim_hud");
            Hearts = new HeartManagement(Content.Load<Texture2D>("sprites/heart"));
            Hearts.Scale = scale;
            Coins = new CoinManagement(Content.Load<Texture2D>("sprites/coin-hud"), this.font3, this.scale);
            Player = new PlayerController(Content.Load<Texture2D>("prototype/Jim"), scale, graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth, this.path + "/Content/prototype/jim.json");
            Map = new Level.Render(scale, graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth);
            Map.setLevel(this.Level);
            Map.jsonContent = null;
            Map.setGround(Content.Load<Texture2D>("prototype/esteira-prototype"));
            Map.setBoxTexture(Content.Load<Texture2D>("prototype/box_2"), Content.Load<Texture2D>("prototype/box_4_2"), Content.Load<Texture2D>("prototype/box_2"), Content.Load<Texture2D>("prototype/box_4_2"));
            Map.setBoxShadow(Content.Load<Texture2D>("prototype/box_2_shadows"));
            Map.setCoinTexture(Content.Load<Texture2D>("sprites/coin"), this.path +"/Content/sprites/coin.json");
            Map.setHeartTexture(Content.Load<Texture2D>("prototype/heart"));
            Map.setRampTexture(Content.Load<Texture2D>("prototype/rampa"));
            Map.setTileMap(Content.Load<Texture2D>("Maps/level_" + this.Level));
            Boss = new Boss(Content.Load<Texture2D>("prototype/boss_1"), this.scale);
            Weapon = new Gun(this.scale, Content.Load<Texture2D>("sprites/bullet"), Content.Load<Texture2D>("Effects/light"), this.path + "/Content/sprites/bullet.json");
            Weapon.Screem = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            // start game
            this.status = GameStatus.PLAY;
        }

        public void LoadMenu()
        {
            this.MainMenu = new MenuManagement(Content.Load<Texture2D>("sprites/main_menu"), this.font3, this.scale);
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
            if (Hearts != null && Player != null && Map != null && Coins != null && Weapon != null && Weapon.AnimationIsReady() && Boss != null){
                if (Map.AnimationIsReady())return true;
            }
            return false;
        }

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
                this.PauseMenu.Draw(spriteBatch);
            }
        }

        public void DrawGameOverMenu()
        {
            if (this.isMainMenuReady() && this.status == GameStatus.LOSE)
            {
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Draw(this.GameOverScreen, new Vector2(0, 0), null, Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
                this.GameOverMenu.Draw(spriteBatch);
            }
        }

        public bool isMainMenuReady()
        {
            if (this.MainMenu != null && this.PauseMenu != null) return true;
            return false;
        }
    }
}
