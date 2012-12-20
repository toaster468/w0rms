using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using w0rms.Rendering;

namespace w0rms
{
    class Terrain
    {
        double seed = 7515;
        public Texture2D TheLevel;

        public Terrain(int width, int height)
        {
            seed = TheGame.Rng.NextDouble() * TheGame.Rng.Next(0, 100);
            TheLevel = GenerateNoiseMap(256, 256, 4);
        }

        private Texture2D GenerateNoiseMap(int width, int height, int octaves)
        {
            Texture2D noiseTexture;
            var data = new float[width * height];

            /// track min and max noise value. Used to normalize the result to the 0 to 1.0 range.
            var min = float.MaxValue;
            var max = float.MinValue;

            /// rebuild the permutation table to get a different noise pattern.
            /// Leave this out if you want to play with changing the number of octaves while
            /// maintaining the same overall pattern.
            Noise2d.Reseed();

            var frequency = 0.5f;
            var amplitude = 1f;
            var persistence = 0.25f;

            for (var octave = 0; octave < octaves; octave++)
            {
                /// parallel loop - easy and fast.
                Parallel.For(0
                    , width * height
                    , (offset) =>
                    {
                        var i = offset % width;
                        var j = offset / width;
                        var noise = Noise2d.Noise(i * frequency * 1f / width, j * frequency * 1f / height);
                        noise = data[j * width + i] += noise * amplitude;

                        min = Math.Min(min, noise);
                        max = Math.Max(max, noise);
                    }
                );

                frequency *= 2;
                amplitude /= 2;
            }

            noiseTexture = new Texture2D(TheGame.Shazam, width, height, false, SurfaceFormat.Color);

            var colors = data.Select(
                (f) =>
                {
                    var norm = (f - min) / (max - min);
                    return new Color(norm, norm, norm, 1);
                }
            ).ToArray();

            noiseTexture.SetData(colors);
            return noiseTexture;
        }

        public static Texture2D GenerateLevel(int width, int height, double seed)
        {
            Texture2D boop = new Texture2D(TheGame.Shazam, width, height, false, SurfaceFormat.Color);
            /*float[,] hi = new float[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    hi[x, y] = SimplexNoise.GetNoise(x, y, seed);
                }
            }*/
            Console.WriteLine("starting generation");
            /*Color[] hi = new Color[width * height];
            for (int i = 0; i < (width * height); i++)
            {
                float value = SimplexNoise.GetNoise((double)i / 3, (double)i % 3, seed);
                hi[i] = new Color(value, value, value);
            }
            boop.SetData<Color>(hi);*/

            Console.WriteLine("done c:");
            Stream stream = File.OpenWrite(@"C:/hi.jpg");

            boop.SaveAsJpeg(stream, width, height);
            stream.Dispose();

            boop.Dispose();
            return boop;
        }
    }
}