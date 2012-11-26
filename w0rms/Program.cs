using System;

namespace w0rms
{
#if WINDOWS || XBOX

    static class Program
    {
        private static void Main(string[] args)
        {
            using (TheGame game = new TheGame())
                game.Run();
        }
    }

#endif
}