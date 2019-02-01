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
    class Button : Component
    {
        #region Fields

        private MouseState currentMouse;
        private SpriteFont font;
        private bool isHovering;
        private bool visible;
        private Color penColor;
        private int width;
        private int height;
        private string text;
        private Vector2 textPosition;
        private MouseState previousMouse;
        private Texture2D texture;
        private Texture2D unpressedTexture;
        private Texture2D pressedTexture;
        private Color color;

#endregion

        #region Properties
        public event EventHandler Click;
        public event EventHandler OnMouseDown;
        public event EventHandler OnMouseUp;
        public bool DoesHoverFunction;
        public bool Clicked { get; private set; }   
        public Color PenColor { get; set; } 
        public Vector2 Position { get; set; }
        public int Width { get { return width; } set { width = value; } }
        public int Height { get { return height; } set { height = value; } }
        public bool IsHovering { get { return isHovering; } }
        public bool Visible { get { return visible; }set { visible = value; } }
        public Rectangle Rectangle { get { return new Rectangle((int)Position.X, (int)Position.Y, Width, Height); } }
        public Texture2D PressedTexture { get { return pressedTexture; }}
        public Texture2D UnpressedTexture { get { return unpressedTexture; } }
        public Texture2D CurrentTexture { get { return texture; } set { texture = value; } }
        public Color Color { get { return color; } set { color = value; } }
        public string Text { get { return text; }set { text = value; } }
        public SpriteFont Font { get { return font; } }
        public Vector2 TextPosition { get { return textPosition; }set { textPosition = value; } }
        #endregion

        #region Methods
        // Constructor
        public Button(Texture2D texture, Texture2D pressedTexture, SpriteFont font, string text, int width, int height, Vector2 position, Color color, Color penColor, bool visible, bool doesHoverFunction)
        {
            this.texture = texture;
            this.unpressedTexture = texture;
            this.pressedTexture = pressedTexture;
            this.font = font;
            this.text = text;
            isHovering = false;
            this.penColor = penColor;
            this.color = color;
            this.width = width;
            this.height = height;
            Position = position;
            DoesHoverFunction = doesHoverFunction;
            this.visible = visible;
        }

        // Draw Method
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (DoesHoverFunction)
            {
                if (isHovering) // Changes texture on hover
                    color = Color.LightGray;
                else
                    color = Color.White;
            }

            if (Visible)
            {
                spriteBatch.Draw(texture, Rectangle, color); // Draws

                if (!string.IsNullOrEmpty(Text) && this.textPosition == Vector2.Zero) // Positions text
                {
                    float x = (Rectangle.X + (Rectangle.Width / 2)) - (font.MeasureString(text).X / 2);
                    float y = (Rectangle.Y + (Rectangle.Height / 2)) - (font.MeasureString(text).Y / 2);

                    textPosition = new Vector2(x, y);

                    spriteBatch.DrawString(font, text, textPosition, penColor);
                }
                else
                {
                    spriteBatch.DrawString(font, text, textPosition, penColor);
                }
            }
        }

        // Update Method
        public override void Update(GameTime gameTime)
        {
            previousMouse = currentMouse; // Current and previous state
            currentMouse = Mouse.GetState(); // functions to recieve only one mouse click at a time

            Rectangle mouseRectangle = new Rectangle(currentMouse.X, currentMouse.Y, 1, 1); // Rectangle of the mouse

            isHovering = false;
            
            if (mouseRectangle.Intersects(Rectangle))
            {
                isHovering = true;

                if (currentMouse.LeftButton == ButtonState.Pressed) // Conditions for the mouse being down
                {
                    OnMouseDown?.Invoke(this, new EventArgs());
                }
                if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed) // Conditions for the mouse being clicked
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
            if (currentMouse.LeftButton == ButtonState.Released) // Conditions for the mouse being released
            {
                OnMouseUp?.Invoke(this, new EventArgs());
            }
        }

#endregion
    }
}
