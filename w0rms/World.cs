﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace w0rms
{
    class World
    {
        public List<Entity> Ents;

        public World()
        {
            Ents = new List<Entity>();
        }

        public void AddEntity(Entity ent)
        {
            Ents.Add(ent);
            ent.MyWorld = this;
        }

        public void Update(TimeSpan ts)
        {
            for (int i = 0; i < Ents.Count; i++)
            {
                Ents[i].Update(ts);
            }
        }

        public void Render()
        {
            for (int i = 0; i < Ents.Count; i++)
            {
                Ents[i].Draw();
            }
        }
    }
}