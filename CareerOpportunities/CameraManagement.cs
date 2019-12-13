using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace CareerOpportunities
{
    public class CameraManagement
    {

        public float TimeShake;
        public Vector2 maxShake;

        public CameraManagement()
        {
        }

        public void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void UpdateShake(int delta)
        { 
        
        }

        private bool IsShake()
        {

            return false;
        }
    }
}
