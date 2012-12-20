using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace w0rms
{
    class Worm
    {
        public string Name = "stan";
        public Vector2 Position = new Vector2(0, 0);
        public float Health = 100.0f;
        public short Team = 0; //0 indexed team number (0 = team1, 1 = team2)

        public Worm(string naam)
        {
            this.Name = naam;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}