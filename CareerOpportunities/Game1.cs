using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CareerOpportunities
{
   
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D Background;
        Texture2D Character;

        public int Level;
        public enum GameStatus { WIN, LOSE, DIE, PLAY, PAUSE, MENU }
        public GameStatus status;

        public bool LoadingLevel;
        public bool LoadingMenu;

        bool escReleased;

        MenuManagement MainMenu;
        HeartManagement Hearts;
        PlayerController Player;
        Level.Render Map;

        PauseMenuManagement PauseMenu;
        PauseMenuManagement GameOverMenu;

        bool debug;
        

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
                    Map.Update(gameTime, 100);
                    Player.Update(gameTime, Map, Hearts);
                    if (Hearts.NumberOfhearts == 0)
                    {
                        this.LoadingLevel = true;
                        this.status = GameStatus.LOSE;
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            this.DrawPlay();
            this.DrawMainMenu();
            this.DrawPauseMenu();


            spriteBatch.End();

            base.Draw(gameTime);
        }


        public void DrawPlay()
        {
            if (this.isLevelReady() && this.status == GameStatus.PLAY)
            {
                spriteBatch.Draw(Background, new Vector2(0, 0), null, Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
                Map.Layer1(spriteBatch, Player.CurrentVerticalLine);
                Player.Draw(spriteBatch);
                Map.Layer0(spriteBatch, Player.CurrentVerticalLine);
                // HUD
                Hearts.Draw(spriteBatch);
                spriteBatch.Draw(this.Character, new Vector2(3 * this.scale, 3 * this.scale), new Rectangle(new Point(0, 0), new Point(27, 27)), Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
            }
        }

        public void LoadLevel()
        {
            Character = Content.Load<Texture2D>("sprites/jim_hud");
            Background = Content.Load<Texture2D>("prototype/esteira");
            Hearts = new HeartManagement(Content.Load<Texture2D>("sprites/heart"));
            Hearts.Scale = scale;
            Player = new PlayerController(Content.Load<Texture2D>("prototype/Jim"), scale, graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth);
            Map = new Level.Render(scale, graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth);
            Map.setBoxTexture(Content.Load<Texture2D>("prototype/box"));
            Map.setCoinTexture(Content.Load<Texture2D>("sprites/coin-animation"), "Content/sprites/coin.json");
            Map.setTileMap(Content.Load<Texture2D>("prototype/prototype_level"));

            // start game
            this.status = GameStatus.PLAY;
        }

        public void LoadMenu()
        {
            this.MainMenu = new MenuManagement(Content.Load<Texture2D>("sprites/main_menu"), this.scale);
            this.PauseMenu = new PauseMenuManagement(Content.Load<Texture2D>("sprites/pause_menu"), this.scale);
            this.status = GameStatus.MENU;
        }

        public bool isLevelReady()
        {
            if (Background != null && Hearts != null && Player != null && Map != null) return true;
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

        public bool isMainMenuReady()
        {
            if (this.MainMenu != null && this.PauseMenu != null) return true;
            return false;
        }
    }
}
