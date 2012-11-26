using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace w0rms
{
    class Team
    {
        public string Name = "navi";
        public short Index = 0;
        public List<Worm> TeamMembers = new List<Worm>() { };

        public Team(string name, short index)
        {
            this.Name = name;
            this.Index = index;
        }

        public void AddWorm(Worm werm)
        {
            TeamMembers.Add(werm);
        }

        public void RemoveWorm(Worm werm)
        {
            TeamMembers.Remove(werm);
        }

        public bool IsInTeam(Worm werm)
        {
            if (TeamMembers.Contains(werm))
                return true;
            else
                return false;
        }

        public override string ToString()
        {
            string s = Name + ": \n";
            foreach (Worm werm in TeamMembers)
            {
                s += "* " + werm.Name + "\n";
            }
            return s;
        }
    }
}