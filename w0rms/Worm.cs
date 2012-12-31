using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace w0rms
{
    class Worm : Entity
    {
        public string Name = "stan";
        public float Health = 100.0f;
        public AnimatedSprite Sprite;

        public short Team = 0; //0 indexed team number (0 = team1, 1 = team2)

        public Worm(string naam)
        {
            this.Name = naam;
            Sprite = new AnimatedSprite(0, 1);
            Sprite.Load("werm", 3, 150, 32, 32);
        }

        public override void Update(TimeSpan ts)
        {
            Sprite.UpdateFrame((float)ts.TotalMilliseconds);
            Sprite.Position = Position;
        }

        public override void Draw()
        {
            Sprite.Draw();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}