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

        public Boss(Texture2D Sprite, Level.Render Map, Hud.Countdown Countdown, int Scale, int CurrentLevel)
        {
            this.Sprite       = Sprite;
            this.Scale        = Scale;
            this.CurrentLevel = CurrentLevel;
            this.Map          = Map;
            this.Countdown    = Countdown;
            this.Reset();
            this.SetRoutine();
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

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = this.movimentes.Count - 1; i >= 0; i --)
            {
                if (timer >= this.movimentes[i].time)
                {
                    this.currently = this.movimentes[i].MoveTo;
                    i = -1;
                }
            }

            this.Move();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Sprite, this.Position, null, Color.White, 0, new Vector2(0, 0), this.Scale, SpriteEffects.None, 0f);
        }


        #region routines
        private int CurrentLevel;
        private Level.Render Map;
        private Hud.Countdown Countdown;

        private void SetRoutine()
        {
            switch (CurrentLevel)
            {
                case 3:
                    this.addRoutineLevel3();
                    break;
                case 7:
                    this.addRoutineLevel7();
                    break;
            }
        }

        public void Move()
        {
            if (!Map.isStoped && !Countdown.isCountdown)
            {
                if (this.currently != ActionController.move.NONE)
                {
                    if (this.currently == ActionController.move.RIGHT && this.Position.X / this.Scale < 172) this.Position = new Vector2(this.Position.X + (this.Scale * this.pull_x), this.Position.Y);
                    else if (this.currently == ActionController.move.LEFT && this.Position.X / this.Scale > -35) this.Position = new Vector2(this.Position.X - (this.Scale * this.pull_x), this.Position.Y);

                    else if (this.currently == ActionController.move.UP && this.Position.Y / this.Scale > 47) this.Position = new Vector2(this.Position.X, this.Position.Y - (this.Scale * this.pull_x));
                    else if (this.currently == ActionController.move.BOTTOM && this.Position.Y / this.Scale < 112) this.Position = new Vector2(this.Position.X, this.Position.Y + (this.Scale * this.pull_x));
                }
            }
        }

        #region level 3
        protected void addRoutineLevel3 ()
        {
            this.movimentes.Add(new ActionController(ActionController.move.RIGHT, 3.0f));
            this.movimentes.Add(new ActionController(ActionController.move.UP,    8.0f));
            this.movimentes.Add(new ActionController(ActionController.move.LEFT,  13.0f));
        }
        #endregion

        #region level 7
        protected void addRoutineLevel7()
        {
            this.movimentes.Add(new ActionController(ActionController.move.RIGHT, 3.0f));
            this.movimentes.Add(new ActionController(ActionController.move.UP,    8.0f));
            this.movimentes.Add(new ActionController(ActionController.move.LEFT,  13.0f));
        }
        #endregion
        #endregion

    }
}
