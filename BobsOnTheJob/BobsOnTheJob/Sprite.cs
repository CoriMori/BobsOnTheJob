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
    class Sprite
    {
        #region Fields
        protected Texture2D texture;
        protected KeyboardState kb;
        protected Vector2 position;
        protected Color color;
        protected float speed;
        protected int width;
        protected int height;
        protected bool willCollide;
        #endregion

        #region Properties
        // Properties
        public Vector2 Position { get { return position; } set { position = value; } }
        public Vector2 Velocity;
        public Color Color { get { return color; } set { color = value; } }
        public float Speed { get { return speed; } set { speed = value; } }
        public Input Input;
        public int Width { get { return width; } set { width = value; } }
        public int Height { get { return height; } set { height = value; } }
        public bool WillCollide { get { return willCollide; } set { willCollide = value; } }
        public Rectangle Rectangle { get { return new Rectangle((int)Position.X, (int)Position.Y, Width, Height); } }
        public Texture2D Texture { get { return texture; } set { texture = value; } }
        #endregion

        #region Methods
        // Constructor just for Texture [Any animations would have to be hardcoded]
        public Sprite(Texture2D texture, int width, int height, Vector2 position, Color color, float speed, bool willCollide)
        {
            this.texture = texture;

            if (this is Player)
            {
                Player temp = (Player)this;
                temp.Width = temp.FrameWidth;
            }      
            else if (width <= 0) width = texture.Width;
            else this.width = width;

            if (this is Player)
            {
                Player temp = (Player)this;
                temp.Height = temp.FrameHeight;
            }
            else if (height <= 0) height = texture.Height;
            else this.height = height;

            this.willCollide = willCollide;

            if (color == null) this.color = Color.White;
            else this.color = color;

            this.position = position;
            this.speed = speed;
        }

        public virtual void Update(GameTime gameTime, List<Sprite> sprites)
        {
        }


        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Rectangle, Color); // changed from position to rectangle
        }
        #endregion

        #region Collision  

        public bool IsTouchingLeft(Sprite sprite)
        {
            return this.Rectangle.Right + this.Velocity.X > sprite.Rectangle.Left &&
                   this.Rectangle.Left < sprite.Rectangle.Left &&
                   this.Rectangle.Bottom > sprite.Rectangle.Top &&
                   this.Rectangle.Top < sprite.Rectangle.Bottom;
        }

        public bool IsTouchingRight(Sprite sprite)
        {
            return this.Rectangle.Left + this.Velocity.X < sprite.Rectangle.Right &&
                   this.Rectangle.Right > sprite.Rectangle.Right &&
                   this.Rectangle.Bottom > sprite.Rectangle.Top &&
                   this.Rectangle.Top < sprite.Rectangle.Bottom;
        }

        public bool IsTouchingTop(Sprite sprite)
        {
            return this.Rectangle.Bottom + this.Velocity.Y > sprite.Rectangle.Top &&
                   this.Rectangle.Top < sprite.Rectangle.Top &&
                   this.Rectangle.Right > sprite.Rectangle.Left &&
                   this.Rectangle.Left < sprite.Rectangle.Right;
        }

        public bool IsTouchingBottom(Sprite sprite)
        {
            return this.Rectangle.Top + this.Velocity.Y < sprite.Rectangle.Bottom &&
                   this.Rectangle.Bottom > sprite.Rectangle.Bottom &&
                   this.Rectangle.Right > sprite.Rectangle.Left &&
                   this.Rectangle.Left < sprite.Rectangle.Right;
        }

        #endregion
    }
}
