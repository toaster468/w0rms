using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace w0rms
{
    interface GameState
    {
        void Start();

        void End();

        void Update(TimeSpan dt);

        void Render();
    }
}