using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace w0rms
{
    class Resources<TYPE>
    {
        static Dictionary<string, TYPE> loaded = new Dictionary<string, TYPE>();

        public static TYPE Get(string file)
        {
            if (loaded.ContainsKey(file) == false)
            {
                loaded.Add(file, TheGame.MainContentLoader.Load<TYPE>(file));
                Console.WriteLine("loaded resource \"" + file + "\" c:");
            }
            return loaded[file];
        }
    }
}