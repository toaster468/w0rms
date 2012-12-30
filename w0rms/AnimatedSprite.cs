using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using w0rms.Rendering;

namespace w0rms
{
    class AnimatedSprite : Sprite
    {
        public AnimatedSprite(Vector2 origin, float rotation, float scale)
        {
            this.Rotation = rotation;
            this.Scale = scale;
            this.Origin = origin;
        }

        public void Load(string asset, int frameCount, int interval, int spriteHeight, int spriteWidth)
        {
            SetTexture("werm2");
            this.spriteHeight = 48;
            this.spriteWidth = 48;
            this.totalFrames = frameCount;
            this.interval = interval;
        }

        public void UpdateFrame(float elapsed)
        {
            SourceRectangle = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);
            timer += elapsed;
            Rotation += 0.001f;

            if (timer > interval)
            {
                //Show the next frame
                currentFrame++;
                //Reset the timer
                timer = 0f;
            }

            if (currentFrame == totalFrames)
            {
                currentFrame = 0;
            }
        }

        public override Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                   (int)Position.X,
                   (int)Position.Y,
                   (int)spriteWidth,
                   (int)spriteHeight);
            }
        }

        public override void Draw()
        {
            //TheGame.spriteBatch.Draw(MyTexture, Position, SourceRectangle, Color.White, 0, Origin, Scale, SpriteEffects.None, 0);
            TheGame.spriteBatch.Draw(MyTexture, Position, SourceRectangle, Color.White);
            C3.XNA.Primitives2D.DrawRectangle(TheGame.spriteBatch, Bounds, Color.Red);
        }
    }
}