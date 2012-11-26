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

        public void SetTexture(Texture2D text)
        {
            MyTexture = text;
            SpriteBounds = new Rectangle(0, 0, text.Width, text.Height);
        }

        public override void Draw()
        {
            TheGame.spriteBatch.Draw(MyTexture, Position, SpriteBounds, DrawColor, Rotation, Origin, Scale, DrawEffects, 0);
        }
    }
}