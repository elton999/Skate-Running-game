using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CareerOpportunities;

namespace CareerOpportunities
{
   
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        Level.Render Map;
        PlayerController Player;
        Texture2D Background;

        bool debug;

        int scale;
        
        public Game1()
        {
            scale = 3;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 240 * scale;
            graphics.PreferredBackBufferHeight = 135 * scale;
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

            Player = new PlayerController(Content.Load<Texture2D>("prototype/Jim"), scale, graphics.PreferredBackBufferHeight);

            Map = new Level.Render(scale, graphics.PreferredBackBufferHeight, 0);//graphics.PreferredBackBufferWidth);
            Map.setBoxTexture(Content.Load<Texture2D>("prototype/box"));
            Map.setTileMap(Content.Load<Texture2D>("prototype/prototype_level"));
        }

        
        protected override void UnloadContent()
        {
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Map.Update(0);
            Player.Update(Map);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            spriteBatch.Draw(Background, new Vector2(0,0), null, Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0f);

            Map.Draw(spriteBatch);
            Player.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
