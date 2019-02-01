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
    class Note : Collectable
    {
        #region Fields
        protected string description;
#endregion

        #region Properties
        public string Description { get { return description; } set { description = value; } }
#endregion

        public Note(Texture2D texture, int width, int height, Vector2 position, Color color, float speed, bool willCollide, string description)
            :base(texture, width, height, position, color, speed, willCollide)
        {
            collected = false;
            this.description = description;
        }
    }
}
