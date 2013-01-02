using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace w0rms
{
    class Entity
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public World MyWorld;

        public Entity()
        {
            Position = new Vector2(0);
            MyWorld = null;
        }

        public virtual void Update(TimeSpan ts) 
        {

        }

        public virtual void Draw() { }

        public virtual void Use(Entity user) { }

        public virtual Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)Position.X - 10, (int)Position.Y - 10, 20, 20);
            }
        }
    }
}