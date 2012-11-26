using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace w0rms.Rendering
{
    class Text : Drawable
    {
        public string ToDraw;
        public SpriteFont Font;

        public Text(string str = "")
        {
            ToDraw = str;
            Font = Resources<SpriteFont>.Get("font");
        }

        public Text(SpriteFont font, string str = "")
        {
            Font = font;
            ToDraw = str;
        }

        public override void Draw()
        {
            TheGame.spriteBatch.DrawString(Font, ToDraw, Position, DrawColor, Rotation, Origin, Scale, DrawEffects, 0);
        }
    }
}