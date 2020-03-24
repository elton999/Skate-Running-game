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
            this.Reset();
            this.SetRoutine();
            this.RiggiBody();
        }

        int[] _BossLevels = { 3, 6 };

        public bool isBossLevel(int level)
        {
            if (this._BossLevels.Contains(level)) return true;
            return false;
        }

        public void Reset()
        {
            this.timer = 0;
            this.Position = new Vector2(-35 * this.Scale, 112 * this.Scale);
        }

        private void RiggiBody()
        {
            this.Body = new Rectangle(new Point((int)this.Position.X, (int)this.Position.Y + (8 * this.Scale)), new Point(70, 42));
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

            bool x_overlaps = ((player_body.Location.X < x_position && width_position > x_position) || (x_width > player_body.Location.X && x_width < width_position));
            bool y_overlaps = ((player_body.Location.Y < y_position && height_position > y_position) || (y_height > player_body.Location.Y && y_height < height_position));

            return x_overlaps && y_overlaps;
        }

        public void Update(GameTime gameTime)
        {
            this.Move();
            this.RiggiBody();

            game.Map.Collision(this.Body, this.Position, this.game.Map.LinePosition(this.Position.Y));
            if (game.Map.CollisionItem(game.Map.CollisionPosition, true) == "hit_box")
            {
                this.NextMovimente();
                game.Map.DestroyHitBox(game.Map.CollisionPosition);
                game.Map.CollisionPosition = Vector2.Zero;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Sprite, this.Position, null, Color.White, 0, new Vector2(0, 0), this.Scale, SpriteEffects.None, 0f);
            DrawRiggidBody(spriteBatch, game.GraphicsDevice);
        }


        #region routines

        private void SetRoutine()
        {
            switch (game.Level)
            {
                case 3:
                    this.addRoutineLevel3();
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
                if (this.currently == ActionController.move.RIGHT && this.Position.X / this.Scale < 230) this.Position = new Vector2(this.Position.X + (this.Scale * this.pull_x), this.Position.Y);
                else if (this.currently == ActionController.move.LEFT && this.Position.X / this.Scale > -35) this.Position = new Vector2(this.Position.X - (this.Scale * this.pull_x), this.Position.Y);
                else if (this.currently == ActionController.move.UP && this.Position.Y / this.Scale > 47) this.Position = new Vector2(this.Position.X, this.Position.Y - (this.Scale * this.pull_x));
                else if (this.currently == ActionController.move.BOTTOM && this.Position.Y / this.Scale < 112) this.Position = new Vector2(this.Position.X, this.Position.Y + (this.Scale * this.pull_x));
            }
        }

        public void NextMovimente()
        {
           if (this.CurrentMoviment + 1 < this.movimentes.Count)
           {
                this.CurrentMoviment++;
           }
        }

        #region level 3
        protected void addRoutineLevel3 ()
        {
            this.movimentes.Add(new ActionController(ActionController.move.NONE));
            this.movimentes.Add(new ActionController(ActionController.move.RIGHT));
            this.movimentes.Add(new ActionController(ActionController.move.UP));
            this.movimentes.Add(new ActionController(ActionController.move.LEFT));
            this.movimentes.Add(new ActionController(ActionController.move.BOTTOM));
            this.movimentes.Add(new ActionController(ActionController.move.LEFT));
            this.movimentes.Add(new ActionController(ActionController.move.RIGHT));
        }
        #endregion

        #region level 7
        protected void addRoutineLevel7()
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
