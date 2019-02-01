using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BobsOnTheJob
{
    class AnimationManager
    {
        // Fields
        private Animation animation;
        private float timer;

        // Properties
        public Vector2 Position { get; set; }


        // Constructor
        public AnimationManager(Animation animation)
        {
            this.animation = animation;
        }

        

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(animation.Texture, Position, 
                new Rectangle(animation.CurrentFrame * animation.FrameWidth, 0, 
                                animation.FrameWidth, animation.FrameHeight), 
                Color.White);
        }


        // Start Animation from the beginning
        public void Play(Animation animation)
        {
            if(this.animation == animation)
            {
                return;
            }

            this.animation = animation;
            animation.CurrentFrame = 0;

            // Restart timer
            timer = 0;                     
        }


        // Halt Animation
        public void Stop()
        {
            // Reset timer
            timer = 0f;                     
            animation.CurrentFrame = 0;
        }

        public virtual void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds; // Increment timer

            if (timer > animation.FrameSpeed) // Track frame speed
            {
                timer = 0f;
                animation.CurrentFrame++;

                if(animation.CurrentFrame > animation.FrameCount) // Loop animation
                {
                    animation.CurrentFrame = 0;
                }
            }
        }
    }
}
