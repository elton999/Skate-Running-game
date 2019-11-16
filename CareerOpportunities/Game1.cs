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

        Texture2D PlayerTexture;
        Vector2 PlayerPosition;
        float PlayerVerticalVelocity;
        float PlayerHorizontalVelocity;

        Level.Render Map;

        bool canMoveVertical;
        int CurrentVerticalLine;
        int PreviousVerticalLine;

        int[] Lines;

        Texture2D Background;
        Texture2D Box;

        int scale;
        
        public Game1()
        {
            scale = 3;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 240 * scale;
            graphics.PreferredBackBufferHeight = 135 * scale;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            CurrentVerticalLine = 0;
            PreviousVerticalLine = 1;
            PlayerVerticalVelocity = (22 * scale) / 6.5f;
            PlayerHorizontalVelocity = 5;
            int linePosition = (32 * scale);
            Lines = new int[] {
                graphics.PreferredBackBufferHeight - linePosition  + (-5 * scale),
                graphics.PreferredBackBufferHeight - (linePosition *2) + (5 * scale),
                graphics.PreferredBackBufferHeight - (linePosition * 3) + (11 * scale),
                graphics.PreferredBackBufferHeight - (linePosition * 4) + (24 * scale),
            };

            canMoveVertical = true;
            PlayerPosition = new Vector2(0, Lines[CurrentVerticalLine]);
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch   = new SpriteBatch(GraphicsDevice);
            Background    = Content.Load<Texture2D>("prototype/esteira");
            Box = Content.Load<Texture2D>("prototype/box");
            PlayerTexture = Content.Load<Texture2D>("prototype/Jim");

            Map = new Level.Render(scale, graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth);
            Map.setBoxTexture(Box);
            Map.setTileMap(Content.Load<Texture2D>("prototype/prototype_level"));
        }

        
        protected override void UnloadContent()
        {
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (PlayerPosition.Y == Lines[CurrentVerticalLine]) canMoveVertical = true;
            else
            {
                if ((CurrentVerticalLine < PreviousVerticalLine && Lines[CurrentVerticalLine] < PlayerPosition.Y) || (CurrentVerticalLine > PreviousVerticalLine && Lines[CurrentVerticalLine] > PlayerPosition.Y))
                {
                    PlayerPosition = new Vector2(PlayerPosition.X, Lines[CurrentVerticalLine]);
                }
                else
                {
                    if (PlayerPosition.Y < Lines[CurrentVerticalLine])
                    {
                        PlayerPosition = new Vector2(PlayerPosition.X, PlayerPosition.Y + PlayerVerticalVelocity);
                    }
                    else if (PlayerPosition.Y > Lines[CurrentVerticalLine])
                    {
                        PlayerPosition = new Vector2(PlayerPosition.X, PlayerPosition.Y - PlayerVerticalVelocity);
                    }
                }
                
            }

            if (canMoveVertical)
            {
                PreviousVerticalLine = CurrentVerticalLine;

                if (Keyboard.GetState().IsKeyDown(Keys.Up) && CurrentVerticalLine < 3)
                {
                    CurrentVerticalLine += 1;
                    canMoveVertical = false;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Down) && CurrentVerticalLine > 0)
                {
                    CurrentVerticalLine -= 1;
                    canMoveVertical = false;
                }
            }
            
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                PlayerPosition = new Vector2(PlayerPosition.X + PlayerHorizontalVelocity, PlayerPosition.Y);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                PlayerPosition = new Vector2(PlayerPosition.X - PlayerHorizontalVelocity, PlayerPosition.Y);
            }

            Map.Update(1);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            spriteBatch.Draw(Background, new Vector2(0,0), null, Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
            Map.Draw(spriteBatch);
            spriteBatch.Draw(PlayerTexture, PlayerPosition, new Rectangle(new Point(0, 0), new Point(32, 32)), Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0f);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
