using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Reflection;
using System.IO;


namespace CareerOpportunities
{
   
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D Background;
        Texture2D Character;

        Texture2D loadingScreen;
        Texture2D GameOverScreen;

        public int Level;
        public enum GameStatus { WIN, LOSE, PLAY, PAUSE, MENU }
        public GameStatus status;

        public bool LoadingLevel;
        public bool LoadingMenu;

        bool escReleased;

        MenuManagement MainMenu;
        HeartManagement Hearts;
        CoinManagement Coins;
        PlayerController Player;
        Level.Render Map;

        PauseMenuManagement PauseMenu;
        PauseMenuManagement GameOverMenu;

        SpriteFont font3;

        bool debug;
        public string path;

        int scale;
        
        public Game1()
        {
            scale = 3;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 240 * scale;
            graphics.PreferredBackBufferHeight = 135 * scale;
            //graphics.ToggleFullScreen();
            Content.RootDirectory = "Content";
            this.LoadingLevel = false;
            this.LoadingMenu = true;
            this.escReleased = true;
            this.path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            debug = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.status   = GameStatus.MENU;
            spriteBatch   = new SpriteBatch(GraphicsDevice);
            loadingScreen = Content.Load<Texture2D>("sprites/loading");
            this.font3 = Content.Load<SpriteFont>("pressstart3");
        }

        
        protected override void UnloadContent()
        {
        }
        
        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();
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
                    Map.Update(gameTime, 130);
                    Player.Update(gameTime, Map, Hearts, Coins);
                    if (Hearts.NumberOfhearts == 0)
                    {
                        //this.LoadingLevel = true;
                        this.status = GameStatus.LOSE;
                        this.GameOverMenu.ItemSelected = PauseMenuManagement.MenuStatus.NONE;
                        this.escReleased = false;
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) && this.escReleased)
                    {
                        this.status = GameStatus.PAUSE;
                        this.PauseMenu.ItemSelected = PauseMenuManagement.MenuStatus.NONE;
                        this.escReleased = false;
                    }
                    if (Keyboard.GetState().IsKeyUp(Keys.Escape)) this.escReleased = true;
                }

                if (Map.Finished()) this.status = GameStatus.WIN;


            }

            if (this.isMainMenuReady())
            {
                if (this.status == GameStatus.PAUSE)
                {   
                    this.PauseMenu.Update(gameTime);
                    if (this.PauseMenu.ItemSelected == PauseMenuManagement.MenuStatus.RESUME)
                    {
                        this.status = GameStatus.PLAY;
                        this.escReleased = true;
                    }
                    else if (this.PauseMenu.ItemSelected == PauseMenuManagement.MenuStatus.EXIT)
                    {
                        this.status = GameStatus.MENU;
                        this.MainMenu.ItemSelected = MenuManagement.MenuItens.NONE;
                    }
                }

                if (this.status == GameStatus.LOSE)
                {
                    this.GameOverMenu.Update(gameTime);
                    if (this.GameOverMenu.ItemSelected == PauseMenuManagement.MenuStatus.RESUME)
                    {
                        this.status = GameStatus.PLAY;
                        this.LoadingLevel = true;
                        this.escReleased = true;
                    }
                    else if (this.GameOverMenu.ItemSelected == PauseMenuManagement.MenuStatus.EXIT)
                    {
                        this.status = GameStatus.MENU;
                        this.MainMenu.ItemSelected = MenuManagement.MenuItens.NONE;
                    }
                }

                if (this.status == GameStatus.MENU)
                {
                    MainMenu.Update(gameTime);
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

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, Matrix.CreateTranslation(0,0,0));

            if ((!this.isLevelReady() && !this.isMainMenuReady()) || (!this.isLevelReady() && this.isMainMenuReady() && this.status == GameStatus.PLAY)) {
                spriteBatch.Draw(this.loadingScreen, new Vector2(0, 0), null, Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
            }

            this.DrawPlay();
            this.DrawMainMenu();
            this.DrawPauseMenu();
            this.DrawGameOverMenu();


            spriteBatch.End();

            base.Draw(gameTime);
        }


        public void DrawPlay()
        {
            if (this.isLevelReady() && this.status == GameStatus.PLAY)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                spriteBatch.Draw(Background, new Vector2(0, 0), null, Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
                Map.Layer1(spriteBatch, Player.CurrentVerticalLine);
                Player.Draw(spriteBatch);
                Map.Layer0(spriteBatch, Player.CurrentVerticalLine);
                // HUD
                Hearts.Draw(spriteBatch);
                Coins.Draw(spriteBatch);
                spriteBatch.Draw(this.Character, new Vector2(3 * this.scale, 3 * this.scale), new Rectangle(new Point(0, 0), new Point(27, 27)), Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
            }
        }

        public void LoadLevel()
        {
            Character = Content.Load<Texture2D>("sprites/jim_hud");
            Background = Content.Load<Texture2D>("prototype/esteira");
            Hearts = new HeartManagement(Content.Load<Texture2D>("sprites/heart"));
            Hearts.Scale = scale;
            Coins = new CoinManagement(Content.Load<Texture2D>("sprites/coin-hud"), this.font3, this.scale);
            Player = new PlayerController(Content.Load<Texture2D>("prototype/Jim"), scale, graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth, this.path + "/Content/prototype/jim.json");
            Map = new Level.Render(scale, graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth);
            Map.jsonContent = null;
            Map.setBoxTexture(Content.Load<Texture2D>("prototype/box"));
            Map.setCoinTexture(Content.Load<Texture2D>("sprites/coin-animation"), this.path +"/Content/sprites/coin.json");
            Map.setHeartTexture(Content.Load<Texture2D>("prototype/heart"));
            Map.setTileMap(Content.Load<Texture2D>("prototype/prototype_level"));

            // start game
            this.status = GameStatus.PLAY;
        }

        public void LoadMenu()
        {
            this.MainMenu = new MenuManagement(Content.Load<Texture2D>("sprites/main_menu"), this.font3, this.scale);
            this.PauseMenu = new PauseMenuManagement(Content.Load<Texture2D>("sprites/pause_menu"), this.scale);
            this.GameOverMenu = new PauseMenuManagement(Content.Load<Texture2D>("sprites/pause_menu"), this.scale);
            this.GameOverMenu.gameOver = true;
            this.GameOverScreen = Content.Load<Texture2D>("sprites/game_over");
            this.status = GameStatus.MENU;
        }

        public bool isLevelReady()
        {
            if (Background != null && Hearts != null && Player != null && Map != null && Coins != null){
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
