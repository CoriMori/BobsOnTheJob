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
    class Light : Sprite
    {
        Random rng;
        private float nextOpacity;

        public float Opacity;
        public float MaxOpacity;
        public float MinOpacity;

        public Light(Texture2D texture, int width, int height, Vector2 position, Color color, float speed, bool willCollide, Random rng)
           : base(texture, width, height, position, color, speed, willCollide)
        {
            this.texture = texture;
            Opacity = 0.5f;
            nextOpacity = Opacity;
            MaxOpacity = 0.5f;
            MinOpacity = 0.2f;
            this.rng = rng;
        }


        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            if (Math.Abs(Opacity - nextOpacity) <= 0.1) fluctuateOpacity();
            else if (Opacity > nextOpacity) Opacity -= 0.01f;
            else if (Opacity < nextOpacity) Opacity += 0.01f;

            //Width = (int)(Width * Opacity);
            //Height = (int)(Height * Opacity);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Rectangle, Color * Opacity);
        }


        public void fluctuateOpacity()
        {
            float range = (float)rng.NextDouble();
            while(range < MinOpacity || range > MaxOpacity)
                range = (float)rng.NextDouble();
            nextOpacity = range;
        }
    }
}
