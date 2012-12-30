using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace w0rms.Rendering
{
    abstract class Drawable
    {
        public Vector2 Origin, Position;
        public float Scale;
        public float Rotation;
        public Color DrawColor;
        public SpriteEffects DrawEffects;

        public Drawable()
        {
            DrawEffects = SpriteEffects.None;
            Origin = new Vector2(0);
            Position = new Vector2(0);
            Scale = 1.0f;
            DrawColor = new Color(255, 255, 255);
            Rotation = 0;
        }

        public abstract void Draw();
    }
}