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
    // Player's possible animations
    enum AnimationState
    {
        Pickup,
        Place,
        Downwards,
        Upwards,
        HorizontalRight,
        HorizontalLeft,
        Idle,
    }
    class Player : Sprite
    {
        #region Other Fields
        public List<Collectable> inventory;
        public AnimationState animState;
        private bool alive;

        private Game1 game;
        private List<Sprite> gameSprites;
        private Vector2 bobReachLocation;
        private Vector2 holdDimensions;
        #endregion

        #region Animation Fields/Properties
        // Constants for "source" rectangle (inside the image)
        private int FrameCount = 12;       // The number of frames in the animation
        public int FrameHeight = 64;     // The height of a single frame
        public int FrameWidth = 48;      // The width of a single frame

        public Collectable currentlyPickingUp;
        private bool movedToHands = false;
        // Animation Timing
        public int CurrentFrame;              // current animation frame
        private double timeCounter;     // The amount of time that has passed
        private double fps;             // The speed of the animation
        private double timePerFrame;    // The amount of time (in fractional seconds) per frame
        #endregion

        #region Other Properties
        // Properties 
        public AnimationState AnimState { get { return animState; } set { animState = value; } }
        public Vector2 BobReachLocation { get { return bobReachLocation; } }
        public bool Alive { get { return alive; } set { alive = value; } }
        #endregion

        #region Constructor
        // Player constructor
        public Player(Texture2D texture, int width, int height, Vector2 position, Color color, float speed, bool willCollide, Game1 game, List<Sprite> sprites)
            : base(texture, width, height, position, color, speed, willCollide)
        {
            this.texture = texture;
            this.game = game;
            gameSprites = sprites;
            inventory = new List<Collectable>();
            animState = AnimationState.Idle;
            alive = true;

            // Frame data
            fps = 20.0;
            timePerFrame = 1.0 / fps;   
        }
#endregion

        #region Update
        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            UpdateAnimationState(gameTime);
            Move();
            kb = Keyboard.GetState();

            foreach (Sprite sprite in sprites) // Loop through all sprites 
            // [In the future, we could add a "willCollideWithPlayer" boolean property if we dont need some sprite colliding]
            {
                if (sprite == this || !sprite.WillCollide) // Shouldnt collide with itself
                {
                    continue;
                }
                if (sprite is Collectable)
                {
                    Collectable temp = (Collectable)sprite;
                    if(!temp.Collected) Collect(temp);
                    continue;
                }
                if ((this.Velocity.X > 0 && this.IsTouchingLeft(sprite)) || // If you're moving along the perpendicular 
                   (this.Velocity.X < 0 && this.IsTouchingRight(sprite))) // plane of the rectangle side you're intersectin with, then collide
                {
                    this.Velocity.X = 0;
                }
                if ((this.Velocity.Y > 0 && this.IsTouchingTop(sprite)) ||
                   (this.Velocity.Y < 0 && this.IsTouchingBottom(sprite)))
                {
                    this.Velocity.Y = 0;
                }
            }

            this.position += this.Velocity; // Move accordingly

            this.Velocity = Vector2.Zero; // Reset movement
        }
#endregion

        // Returns Distance between 2 rectangles
        public double DistanceBetween(Rectangle a, Rectangle b)
        {
            return Math.Sqrt(Math.Pow(a.Center.X - b.Center.X, 2) + Math.Pow(a.Center.Y - b.Center.Y, 2));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawAnimationState(spriteBatch);
        }

        // If player collides with a collectable, add it to the inventory
        public void Collect(Collectable col)
        {
            // Makes sure player is proper position to collect
            if(!col.Collected 
                && DistanceBetween(col.Rectangle, new Rectangle(this.Rectangle.Left, this.Rectangle.Bottom - this.Height / 4, this.Width, this.Height / 4)) < 15 
                && kb.IsKeyDown(Input.Pickup)
                && animState != AnimationState.Pickup)
            {
                animState = AnimationState.Pickup;
                currentlyPickingUp = col; // Placeholder for other calculations
                holdDimensions = new Vector2(currentlyPickingUp.Width, currentlyPickingUp.Height);
                movedToHands = false; // Keeps track of collectable's movement progress
                CurrentFrame = 0; // Makes sure animation starts from beginning
            }
        }

        #region Animation Update/Draw
        // Handles switch statements for animation FSM, the default being the idle animation
        public void UpdateAnimationState(GameTime gameTime)
        {
            // Update some reference locations
            bobReachLocation = new Vector2(this.Position.X + this.Width / 2, this.Position.Y + this.Height / 5);

            // Handle animation timing
            // - Add to the time counter
            // - Check if we have enough "time" to advance the frame
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;
            if (timeCounter >= timePerFrame)
            {
                // Will calculate the planned movement of the collectable when picking it up
                // And do not progress the animation unless told to do so
                if(animState == AnimationState.Pickup && currentlyPickingUp != null)
                {
                    Vector2 toPickupPos = new Vector2(this.Rectangle.Center.X - currentlyPickingUp.Width / 2, this.Rectangle.Bottom - currentlyPickingUp.Height);

                    if (!movedToHands)
                    {
                        // Move collectable to space where it is picked up
                        if (currentlyPickingUp.Position.X != toPickupPos.X)
                            currentlyPickingUp.Position += new Vector2(2 * Math.Abs((toPickupPos.X - currentlyPickingUp.Position.X)) / (toPickupPos.X - currentlyPickingUp.Position.X), 0);
                        currentlyPickingUp.Width--;
                        if (currentlyPickingUp.Position.Y != toPickupPos.Y)
                            currentlyPickingUp.Position += new Vector2(0, 2 * Math.Abs((toPickupPos.Y - currentlyPickingUp.Position.Y)) / (toPickupPos.Y - currentlyPickingUp.Position.Y));
                        currentlyPickingUp.Height--;


                        if (currentlyPickingUp.Position == toPickupPos)
                            movedToHands = true;
                    }
                    else
                    {
                        // Will Move collectable up with the player's hands                      
                        double upIncr = DistanceBetween(currentlyPickingUp.Rectangle, new Rectangle((int)bobReachLocation.X, (int)bobReachLocation.Y, 1, 1));
                        currentlyPickingUp.Position -= new Vector2(0, (float)upIncr / (FrameCount - CurrentFrame));

                        if (DistanceBetween(currentlyPickingUp.Rectangle, new Rectangle((int)bobReachLocation.X, (int)bobReachLocation.Y, 1, 1)) < 5)
                        {
                            currentlyPickingUp.Collected = true;
                            currentlyPickingUp.Width = (int)holdDimensions.X;
                            currentlyPickingUp.Height = (int)holdDimensions.Y;
                            inventory.Add(currentlyPickingUp);
                            gameSprites.Remove(currentlyPickingUp);
                            currentlyPickingUp = null;
                        }

                        CurrentFrame++;
                    }
                }
                else if (animState == AnimationState.Place)
                {

                }
                else CurrentFrame += 1;

                // Check loop progress
                if (CurrentFrame > FrameCount - 1)     
                {
                    // Ends Pickup animation after 1 loop
                    if (animState == AnimationState.Pickup
                        || animState == AnimationState.Place)
                    {
                        animState = AnimationState.Idle;
                        currentlyPickingUp = null;
                    }
                    // Back to 1 (since 0 is the "standing" frame)
                    CurrentFrame = 0;                  
                }
                timeCounter -= timePerFrame;    // Remove the time we "used"
            }

            switch (animState)
            {
                case AnimationState.Idle:
                    if (kb.IsKeyDown(Input.Up))
                        animState = AnimationState.Upwards;
                    else if (kb.IsKeyDown(Input.Left))
                        animState = AnimationState.HorizontalLeft;
                    else if (kb.IsKeyDown(Input.Down))
                        animState = AnimationState.Downwards;
                    else if (kb.IsKeyDown(Input.Right))
                        animState = AnimationState.HorizontalRight;
                    break;

                case AnimationState.Downwards:
                    if (kb.IsKeyUp(Input.Down))
                        animState = AnimationState.Idle;
                    else if (kb.IsKeyDown(Input.Up) || kb.IsKeyDown(Input.Left) || kb.IsKeyDown(Input.Right))
                        animState = AnimationState.Downwards;
                    break;

                case AnimationState.Upwards:
                    if (kb.IsKeyUp(Input.Up))
                        animState = AnimationState.Idle;
                    else if (kb.IsKeyDown(Input.Down)||kb.IsKeyDown(Input.Left)||kb.IsKeyDown(Input.Right))
                        animState = AnimationState.Upwards;
                    break;

                case AnimationState.HorizontalLeft:
                    if (kb.IsKeyUp(Input.Left))
                        animState = AnimationState.Idle;
                    else if (kb.IsKeyDown(Input.Up))
                        animState = AnimationState.Upwards;
                    else if (kb.IsKeyDown(Input.Down))
                        animState = AnimationState.Downwards;
                    else if (kb.IsKeyDown(Input.Left)) { }
                    else if (kb.IsKeyDown(Input.Right))
                        animState = AnimationState.HorizontalRight;
                    break;

                case AnimationState.HorizontalRight:
                    if (kb.IsKeyUp(Input.Right))
                        animState = AnimationState.Idle;
                    else if (kb.IsKeyDown(Input.Up))
                        animState = AnimationState.Upwards;
                    else if (kb.IsKeyDown(Input.Down))
                        animState = AnimationState.Downwards;
                    else if (kb.IsKeyDown(Input.Right)) { }
                    else if (kb.IsKeyDown(Input.Left))
                        animState = AnimationState.HorizontalLeft;
                    break;

                case AnimationState.Pickup:
                    break;

                case AnimationState.Place:
                    break;
            }
        }


        // Draws player based on the current FRAME field
        public void DrawBobIdle(SpriteEffects flipSprite, SpriteBatch spriteBatch, Texture2D spriteSheet)
        {
            spriteBatch.Draw(
                spriteSheet,                    // - The texture to draw
                new Vector2(position.X,         // - The location to draw on the screen
                    position.Y),                
                new Rectangle(                  // - The "source" rectangle
                    CurrentFrame / 4 * FrameWidth,                          //   - This rectangle specifies
                    0,        //	   where "inside" the texture
                    FrameWidth,           //     to get pixels (We don't want to
                    FrameHeight),         //     draw the whole thing)
                color,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                1.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }

        public void DrawBobHorizontal(SpriteEffects flipSprite, SpriteBatch spriteBatch, Texture2D spriteSheet)
        {
            spriteBatch.Draw(
                spriteSheet,                    // - The texture to draw
                new Vector2(position.X, 
                position.Y),                    // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    CurrentFrame * FrameWidth + 4 * FrameWidth,     //   - This rectangle specifies
                    0,           //	   where "inside" the texture
                    FrameWidth,             //     to get pixels (We don't want to
                    FrameHeight),           //     draw the whole thing)
                color,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                1.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }

        public void DrawBobForward(SpriteEffects flipSprite, SpriteBatch spriteBatch, Texture2D spriteSheet)
        {
            spriteBatch.Draw(
                spriteSheet,                    // - The texture to draw
                new Vector2(position.X,
                position.Y),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    CurrentFrame * FrameWidth,     //   - This rectangle specifies
                    FrameHeight * 2,           //	   where "inside" the texture
                    FrameWidth,             //     to get pixels (We don't want to
                    FrameHeight),           //     draw the whole thing)
                color,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                1.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }

        public void DrawBobBackwards(SpriteEffects flipSprite, SpriteBatch spriteBatch, Texture2D spriteSheet)
        {
            spriteBatch.Draw(
                spriteSheet,                    // - The texture to draw
                new Vector2(position.X,
                position.Y),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    CurrentFrame * FrameWidth,     //   - This rectangle specifies
                    FrameHeight,           //	   where "inside" the texture
                    FrameWidth,             //     to get pixels (We don't want to
                    FrameHeight),           //     draw the whole thing)
                color,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                1.0f,                           // - Scale (100% - no change)
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }

        public void DrawBobPickup(SpriteEffects flipSprite, SpriteBatch spriteBatch, Texture2D spriteSheet)
        {
            spriteBatch.Draw(
                spriteSheet,
                new Vector2(position.X,
                position.Y),
                new Rectangle(
                    CurrentFrame * FrameWidth,
                    FrameHeight * 3,
                    FrameWidth,
                    FrameHeight),
                color,
                0,
                Vector2.Zero,
                1.0f,
                flipSprite,
                0);
        }


        public void DrawAnimationState(SpriteBatch spriteBatch)
        {
            switch (animState)
            {
                case AnimationState.Idle:
                    DrawBobIdle(SpriteEffects.None, spriteBatch, texture);
                    break;
                case AnimationState.Downwards:
                    DrawBobForward(SpriteEffects.None, spriteBatch, texture);
                    break;
                case AnimationState.Upwards:
                    DrawBobBackwards(SpriteEffects.None, spriteBatch, texture);
                    break;
                case AnimationState.HorizontalLeft:
                    DrawBobHorizontal(SpriteEffects.FlipHorizontally, spriteBatch, texture);
                    break;
                case AnimationState.HorizontalRight:
                    DrawBobHorizontal(SpriteEffects.None, spriteBatch, texture);
                    break;
                case AnimationState.Pickup:
                    DrawBobPickup(SpriteEffects.None, spriteBatch, texture);
                    break;
            }
        }
#endregion

        #region Movement

        public void Move()
        {           
            kb = Keyboard.GetState();

            if (animState != AnimationState.Pickup)
            {
                if (kb.IsKeyDown(Input.Up))
                {
                    this.Velocity.Y -= speed;
                }
                else if (kb.IsKeyDown(Input.Down))
                {
                    this.Velocity.Y = speed;
                }
                if (kb.IsKeyDown(Input.Left))
                {
                    this.Velocity.X -= speed;
                }
                else if (kb.IsKeyDown(Input.Right))
                {
                    this.Velocity.X += speed;
                }
            }
        }

#endregion
    }
}
