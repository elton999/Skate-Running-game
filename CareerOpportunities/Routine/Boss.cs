using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CareerOpportunities.Routine
{
    public class Boss : GameObject
    {
        public List<ActionController> movimentes = new List<ActionController>();
        public float timer;
        public ActionController.move currently;
        public int line;
        public float pull_x = 2;

        public Game1 game;

        public Boss(Game1 game)
        {
            this.game = game;
            this.Scale = this.game.scale;
            this.Sprite = game.Content.Load<Texture2D>("prototype/boss_1");
            this.BossHUD = game.Content.Load<Texture2D>("sprites/boss_hud");
            this.Reset();
            this.SetRoutine();
            this.RiggiBody();
            this.setSprite(this.Sprite);
            this.setJsonFile(@"Content/prototype/boss_1.json");
        }

        int[] _BossLevels = { 5, 6 };

        public bool isBossLevel(int level)
        {
            if (this._BossLevels.Contains(level)) return true;
            return false;
        }

        public void Reset()
        {
            this.timer = 0;
            this.Position = new Vector2(-60 * this.Scale, 112 * this.Scale);
        }

        private void RiggiBody()
        {
            this.Body = new Rectangle(new Point((int)this.Position.X, (int)this.Position.Y + (8 * this.Scale)), new Point(22,22));
        }

        public bool Collision(Rectangle player_body)
        {
            // Player collision
            float width_position = (player_body.Size.X * this.Scale) + player_body.Location.X;
            float height_position = (player_body.Size.Y * this.Scale) + player_body.Location.Y;

            // Boss Collision
            float x_position = Body.Location.X;
            float y_position = Body.Location.Y;

            float x_width = x_position + ((float)Body.Size.X * this.Scale);
            float y_height = y_position + ((float)Body.Size.Y * this.Scale);

            bool x_overlaps = ((player_body.Location.X < x_position && width_position > x_position) || 
                (player_body.Location.X > x_position && width_position < x_width) || 
                (x_width > player_body.Location.X && x_width < width_position));

            bool y_overlaps = ((player_body.Location.Y < y_position && height_position > y_position) ||
                (player_body.Location.Y > y_position && height_position < y_height) ||
                (y_height > player_body.Location.Y && y_height < height_position));

            return x_overlaps && y_overlaps;
        }

        int healthy = 18;
        public void Update(GameTime gameTime, CameraManagement camera)
        {
            this.Move();
            this.RiggiBody();

            game.Map.Collision(this.Body, this.Position, this.game.Map.LinePosition(this.Position.Y));
            switch (game.Map.CollisionItem(game.Map.CollisionPosition, true, false, true))
            {
                case "hit_box":
                    this.NextMovimente();
                    game.Map.DestroyHitBox(game.Map.CollisionPosition);
                    game.Map.CollisionPosition = Vector2.Zero;
                    break;
                case "box":
                    healthy--;
                    camera.TimeShake = 10;
                    break;
                
            }

            float pull = 220f;
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (game.Map.isStoped)
            {
                this.Position = new Vector2(this.Position.X + (pull * this.Scale * delta), this.Position.Y);
                this.CurrentAnimation = AnimationStatus.RUN;
            }

            if (!game.Map.isStoped && healthy < 1)
            {
                this.Position.X -= (pull * this.Scale * delta);
            }

            this.PlayAnimation(gameTime);
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 position = new Vector2(this.Position.X - (15*this.Scale), this.Position.Y - (35*this.Scale));
            this.DrawAnimation(spriteBatch, position, this.Scale * 2);
            //spriteBatch.Draw(this.Sprite, this.Position, null, Color.White, 0, new Vector2(0, 0), this.Scale * 2, SpriteEffects.None, 0f);

#if DEBUG
            DrawRiggidBody(spriteBatch, game.GraphicsDevice);
#endif
        }


        private Texture2D BossHUD;
        public void DrawHud(SpriteBatch spriteBatch)
        {
            if (this.Position.X > -50)
            {
                spriteBatch.Draw(this.BossHUD, new Vector2(3 * this.Scale, 3 * this.Scale), new Rectangle(new Point(0, 0), new Point(29, 27)), Color.White, 0, new Vector2(0, 0), this.Scale, SpriteEffects.None, 0f);
                Vector2 position_bar = new Vector2((3 + 27) * this.Scale, 3 * this.Scale);
                for (int i = 0; i < 18; i++)
                {
                    if (i < healthy) spriteBatch.Draw(this.BossHUD, position_bar, new Rectangle(new Point(30, 0), new Point(4, 27)), Color.White, 0, new Vector2(0, 0), this.Scale, SpriteEffects.None, 0f);
                    else spriteBatch.Draw(this.BossHUD, position_bar, new Rectangle(new Point(34, 0), new Point(4, 27)), Color.White, 0, new Vector2(0, 0), this.Scale, SpriteEffects.None, 0f);
                    position_bar = new Vector2((3 + 27 + (i * 4)) * this.Scale, 3 * this.Scale);
                }
                spriteBatch.Draw(this.BossHUD, position_bar, new Rectangle(new Point(38, 0), new Point(5, 27)), Color.White, 0, new Vector2(0, 0), this.Scale, SpriteEffects.None, 0f);
            }
        }


        #region routines

        private void SetRoutine()
        {
            switch (game.Level)
            {
                case 5:
                    this.addRoutineLevel5();
                    break;
                case 7:
                    this.addRoutineLevel7();
                    break;
            }
        }


        private int CurrentMoviment = 0;
        protected void Move()
        {
            this.currently = this.movimentes[CurrentMoviment].MoveTo;

            if (!game.Map.isStoped && !game.Countdown.isCountdown && this.currently != ActionController.move.NONE)
            {
                if (this.currently == ActionController.move.RIGHT && this.Position.X / this.Scale < 230)
                {
                    this.Position = new Vector2(this.Position.X + (this.Scale * this.pull_x), this.Position.Y);
                    this.CurrentAnimation = AnimationStatus.RUN;
                }
                else if (this.currently == ActionController.move.LEFT && this.Position.X / this.Scale > 0)
                {
                    this.Position = new Vector2(this.Position.X - (this.Scale * this.pull_x), this.Position.Y);
                    this.CurrentAnimation = AnimationStatus.TIRED;
                }
                else if (this.currently == ActionController.move.UP && this.Position.Y / this.Scale > 90)
                {
                    this.Position = new Vector2(this.Position.X, this.Position.Y - (this.Scale * this.pull_x));
                    this.CurrentAnimation = AnimationStatus.RUN;
                }
                else if (this.currently == ActionController.move.BOTTOM && this.Position.Y / this.Scale < 112)
                {
                    this.Position = new Vector2(this.Position.X, this.Position.Y + (this.Scale * this.pull_x));
                    this.CurrentAnimation = AnimationStatus.RUN;
                }
                else this.CurrentAnimation = AnimationStatus.IDLE;
            } else this.CurrentAnimation = AnimationStatus.IDLE;
        }

        public void NextMovimente()
        {
           if (this.CurrentMoviment + 1 < this.movimentes.Count)
           {
                this.CurrentMoviment++;
           }
        }

        private enum AnimationStatus
        {
            IDLE,
            RUN,
            TIRED,
            HIT,
        }
        private AnimationStatus CurrentAnimation = AnimationStatus.IDLE;
        private void PlayAnimation(GameTime gameTime)
        {
            if (healthy > 0)
            {
                if (this.CurrentAnimation == AnimationStatus.RUN) this.play(gameTime, "run", AnimationDirection.LOOP);
                else if (this.CurrentAnimation == AnimationStatus.TIRED) this.play(gameTime, "tired", AnimationDirection.LOOP);
                else this.play(gameTime, "idle", AnimationDirection.LOOP);
            }
            else this.play(gameTime, "hit", AnimationDirection.LOOP);
            
        }

        #region level 3
        protected void addRoutineLevel5 ()
        {
            this.movimentes.Add(new ActionController(ActionController.move.NONE));
            this.movimentes.Add(new ActionController(ActionController.move.RIGHT));
            this.movimentes.Add(new ActionController(ActionController.move.UP));
            this.movimentes.Add(new ActionController(ActionController.move.LEFT));
            this.movimentes.Add(new ActionController(ActionController.move.BOTTOM));
            this.movimentes.Add(new ActionController(ActionController.move.LEFT));
            this.movimentes.Add(new ActionController(ActionController.move.RIGHT));
            this.movimentes.Add(new ActionController(ActionController.move.BOTTOM));
            this.movimentes.Add(new ActionController(ActionController.move.LEFT));
            this.movimentes.Add(new ActionController(ActionController.move.RIGHT));
            this.movimentes.Add(new ActionController(ActionController.move.UP));
            this.movimentes.Add(new ActionController(ActionController.move.LEFT));
            this.movimentes.Add(new ActionController(ActionController.move.RIGHT));
        }
        #endregion

        #region level 7
        protected void addRoutineLevel7 ()
        {
            this.movimentes.Add(new ActionController(ActionController.move.NONE));
            this.movimentes.Add(new ActionController(ActionController.move.RIGHT));
            this.movimentes.Add(new ActionController(ActionController.move.UP));
            this.movimentes.Add(new ActionController(ActionController.move.LEFT));
        }
        #endregion
        #endregion

    }
}
