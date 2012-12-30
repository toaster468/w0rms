using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace w0rms.Rendering
{
    class Sprite : Drawable
    {
        public Texture2D MyTexture
        {
            get;
            private set;
        }

        public float timer = 0f;
        public float interval = 100f;
        public int currentFrame = 0;
        public int spriteWidth = 48;
        public int spriteHeight = 48;
        public int totalFrames = 6;
        public Rectangle SourceRectangle;
        public int Height;
        public int Width;

        public Rectangle SpriteBounds;

        public Sprite()
        {
            MyTexture = null;
            SpriteBounds = new Rectangle(0, 0, 0, 0);
        }

        public Sprite(Texture2D texture)
        {
            SetTexture(texture);
        }

        public Sprite(string file)
        {
            SetTexture(file);
        }

        public void Center()
        {
            Origin = new Vector2(SpriteBounds.Width / 2, SpriteBounds.Height / 2);
        }

        public void SetTexture(string file)
        {
            SetTexture(Resources<Texture2D>.Get(file));
        }

        public virtual void SetTexture(Texture2D text)
        {
            MyTexture = text;
            SpriteBounds = new Rectangle(0, 0, text.Width, text.Height);
            Width = text.Width;
            Height = text.Height;
        }

        public override void Draw()
        {
            TheGame.spriteBatch.Draw(MyTexture, Position, SpriteBounds, DrawColor, Rotation, Origin, Scale, DrawEffects, 0);
        }

        public void Draw(Texture2D text, Vector2 position, Rectangle srcrec)
        {
            TheGame.spriteBatch.Draw(text, position, srcrec, DrawColor, Rotation, Origin, Scale, DrawEffects, 0);
        }

        //collision shit c:

        public bool CollidesWith(Sprite other)
        {
            // Default behavior uses per-pixel collision detection
            return CollidesWith(other, true);
        }

        public bool CollidesWith(Sprite other, bool calcPerPixel)
        {
            // Get dimensions of texture
            int widthOther = other.MyTexture.Width;
            int heightOther = other.MyTexture.Height;
            int widthMe = MyTexture.Width;
            int heightMe = MyTexture.Height;

            if (calcPerPixel &&
                ((Math.Min(widthOther, heightOther) > 100) ||
                (Math.Min(widthMe, heightMe) > 100)))
            {
                return Bounds.Intersects(other.Bounds) //check for AABB collision first
                    && PerPixelCollision(this, other);
            }

            return Bounds.Intersects(other.Bounds);
        }

        private static bool PerPixelCollision(Sprite a, Sprite b)
        {
            // Get Color data of each Texture
            Color[] bitsA = new Color[a.MyTexture.Width * a.MyTexture.Height];
            a.MyTexture.GetData(bitsA);
            Color[] bitsB = new Color[b.MyTexture.Width * b.MyTexture.Height];
            b.MyTexture.GetData(bitsB);

            // Calculate the intersecting rectangle
            int x1 = Math.Max(a.Bounds.X, b.Bounds.X);
            int x2 = Math.Min(a.Bounds.X + a.Bounds.Width, b.Bounds.X + b.Bounds.Width);

            int y1 = Math.Max(a.Bounds.Y, b.Bounds.Y);
            int y2 = Math.Min(a.Bounds.Y + a.Bounds.Height, b.Bounds.Y + b.Bounds.Height);

            // For each pixel in the intersecting rectangle
            for (int y = y1; y < y2; ++y)
            {
                for (int x = x1; x < x2; ++x)
                {
                    // Get the color from each texture
                    Color Ca = bitsA[(x - a.Bounds.X) + (y - a.Bounds.Y) * a.Width];
                    Color Cb = bitsB[(x - b.Bounds.X) + (y - b.Bounds.Y) * b.Width];

                    if (Ca.A != 0 && Cb.A != 0) // If both colors are not transparent (the alpha channel is not 0), then there is a collision
                    {
                        return true;
                    }
                }
            }
            // If no collision occurred by now, we're clear.
            return false;
        }

        private Rectangle bounds = Rectangle.Empty;

        public virtual Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                   (int)Position.X,
                   (int)Position.Y,
                   (int)Width,
                   (int)Height);
            }
        }
    }
}