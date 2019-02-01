using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BobsOnTheJob
{
    class Collectable : Sprite
    {
        #region Fields
        protected bool collected;
        protected bool placed;
        #endregion

        #region Properties
        public bool Collected { get { return collected; } set { collected = value; } }
        public bool Placed { get { return placed; } set { placed = value; } }
#endregion

        public Collectable(Texture2D texture, int width, int height, Vector2 position, Color color, float speed, bool willCollide)
            :base (texture, width, height, position, color, speed, willCollide)
        {
            collected = false;
            placed = false;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(collected == false)
            {
                spriteBatch.Draw(texture, Rectangle, Color);
            }
        }
    }
}
