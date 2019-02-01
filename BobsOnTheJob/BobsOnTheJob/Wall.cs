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
    /// <summary>
    /// a class for an unpassable object that is drawn to the screen
    /// has a position and a sprite that can change depending on the constructor
    /// we can put them in a list later and draw them and then have the player check for collisions against the 
    ///                                                                                                 whole list
    /// </summary>
    class Wall : Sprite
    {
        // fields of the walls

        /// <summary>
        /// walls constructor
        /// take a texture2D and a Rectangle as params
        /// the sprite is in the constructor so we dont need a class for each type of wall
        /// </summary>
        /// <param name="pic"></param>
        /// <param name="pos"></param>
        public Wall(Texture2D texture, int width, int height, Vector2 position, Color color, float speed, bool willCollide) 
            : base(texture, width, height, position, color, speed, willCollide)
        {
            //position = pos;
            // this.texture = texture;
        }

        // public Wall(Sprite sp)
        // 
        //    sprite = sp.Texture;
        //    position = sp.Rectangle;
        // 
    }
}
