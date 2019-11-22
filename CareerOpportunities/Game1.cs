using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CareerOpportunities
{
   
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        Level.Render Map;
        PlayerController Player;
        Texture2D Background;
        HeartManagement hearts;

        bool debug;

        int scale;
        
        public Game1()
        {
            scale = 6;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 240 * scale;
            graphics.PreferredBackBufferHeight = 135 * scale;
            //graphics.ToggleFullScreen();
            Content.RootDirectory = "Content";
            debug = true;
        }

        protected override void Initialize()
        {
            
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch   = new SpriteBatch(GraphicsDevice);
            Background    = Content.Load<Texture2D>("prototype/esteira");
            hearts        = new HeartManagement(Content.Load<Texture2D>("sprites/heart"));
            hearts.Scale = scale;
            Player        = new PlayerController(Content.Load<Texture2D>("prototype/Jim"), scale, graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth);
            Map           = new Level.Render(scale, graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth);
            Map.setBoxTexture(Content.Load<Texture2D>("prototype/box"));
            Map.setCoinTexture(Content.Load<Texture2D>("sprites/coin"));
            Map.setTileMap(Content.Load<Texture2D>("prototype/prototype_level"));
        }

        
        protected override void UnloadContent()
        {
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Map.Update(gameTime, 60);
            Player.Update(gameTime, Map);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            spriteBatch.Draw(Background, new Vector2(0,0), null, Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0f);

            Map.Layer1(spriteBatch, Player.CurrentVerticalLine);
            Player.Draw(spriteBatch);
            Map.Layer0(spriteBatch, Player.CurrentVerticalLine);

            //HUD
            hearts.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
