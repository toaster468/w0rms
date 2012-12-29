using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

//using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using w0rms.Rendering;
using xColor = Microsoft.Xna.Framework.Color;

namespace w0rms
{
    class Terrain
    {
        double seed = 7515;
        public Sprite TheLevel;
        public Pen bpen;

        public Terrain(Texture2D texture)
        {
            TheLevel = new Sprite(texture);
        }

        public Terrain(int width, int height)
        {
            seed = TheGame.Rng.NextDouble() * TheGame.Rng.Next(0, 100);
            bpen = new Pen(Brushes.Black);
            bpen.Color = System.Drawing.Color.FromArgb(0, 0, 0);
            bpen.Width = 2.0f;

            var bmp = GenerateLevelBitmap(GenerateSlice(500, 12), 500);
            TheLevel = new Sprite(GetTexture2DFromBitmap(TheGame.graphics.GraphicsDevice, bmp));
        }

        public static Texture2D GetTexture2DFromBitmap(GraphicsDevice device, Bitmap bitmap)
        {
            Texture2D tex = new Texture2D(device, bitmap.Width, bitmap.Height, false, SurfaceFormat.Color);

            BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            int bufferSize = data.Height * data.Stride;

            //create data buffer
            byte[] bytes = new byte[bufferSize];

            // copy bitmap data into buffer
            Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);

            // copy our buffer to the texture
            tex.SetData(bytes);

            // unlock the bitmap data
            bitmap.UnlockBits(data);

            return tex;
        }

        public float[] GenerateSlice(int width, int octaves)
        {
            var data = new float[width];

            var min = float.MaxValue;
            var max = float.MinValue;

            Noise2d.Reseed();

            var frequency = 0.5f;
            var amplitude = 1f;
            var persistence = 0.25f;

            for (var octave = 0; octave < octaves; octave++)
            {
                Parallel.For(0
                    , width * 1
                    , (offset) =>
                    {
                        var i = offset % width;
                        var j = offset / width;
                        var noise = Noise2d.Noise(i * frequency * 1f / width, j * frequency * 1f / 1);
                        noise = data[j * width + i] += noise * amplitude;

                        min = Math.Min(min, noise);
                        max = Math.Max(max, noise);
                    }
                );

                frequency *= 2;
                amplitude /= 2;
            }

            var colors = data.Select(
                (f) =>
                {
                    var norm = (f - min) / (max - min);
                    return norm;
                }
            ).ToArray();

            return colors;
        }

        public Bitmap GenerateLevelBitmap(float[] heightmap, int width)
        {
            Bitmap b = new Bitmap(500, 250);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                //g.FillRectangle(Brushes.Transparent, 0, 0, 2000, 1000);

                for (int i = 1; i < 500; i += 1)
                {
                    float newValuePrev = scaleRange(heightmap[i - 1], 0.0f, 1.0f, 0.25f, 0.8f);
                    float newValue = scaleRange(heightmap[i], 0.0f, 1.0f, 0.25f, 0.8f);
                    DrawLine(b, i - 1, newValuePrev * 500, i, newValue * 500, g);
                    //DrawLine(b, i - 10, heightmap[i - 10] * 1000, i, heightmap[i] * 1000, g);
                }
                //DrawLine(b, 1999, heightmap[1999] * 1000, 2000, 1000, g);
                FloodFill(b, 1, 1, Color.FromArgb(255, 255, 255, 255)); //make transparent
                b.Save("C:/level.jpg");
            }

            return b;
        }

        public void DrawLine(Bitmap bananu, float x, float y, float x1, float y1, Graphics g)
        {
            //g.DrawLine(wpen, x, y, x1, y1);
            g.DrawLine(bpen, x, y, x1, y1);
        }

        public float scaleRange(float value, float oldMin, float oldMax, float newMin, float newMax)
        {
            return (value / ((oldMax - oldMin) / (newMax - newMin))) + newMin;
        }

        public static Microsoft.Xna.Framework.Graphics.Texture2D GenerateLevel(int width, int height, double seed)
        {
            Microsoft.Xna.Framework.Graphics.Texture2D boop = new Microsoft.Xna.Framework.Graphics.Texture2D(TheGame.Shazam, width, height, false, Microsoft.Xna.Framework.Graphics.SurfaceFormat.Color);
            Console.WriteLine("starting generation");

            Console.WriteLine("done c:");
            Stream stream = File.OpenWrite(@"C:/hi.jpg");

            boop.SaveAsJpeg(stream, width, height);
            stream.Dispose();

            boop.Dispose();
            return boop;
        }

        private void FloodFill(Bitmap bitmap, int x, int y, Color color)
        {
            BitmapData data = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int[] bits = new int[data.Stride / 4 * data.Height];
            Marshal.Copy(data.Scan0, bits, 0, bits.Length);

            LinkedList<Point> check = new LinkedList<Point>();
            int floodTo = color.ToArgb();
            int floodFrom = bits[x + y * data.Stride / 4];
            bits[x + y * data.Stride / 4] = floodTo;

            if (floodFrom != floodTo)
            {
                check.AddLast(new Point(x, y));
                while (check.Count > 0)
                {
                    Point cur = check.First.Value;
                    check.RemoveFirst();

                    foreach (Point off in new Point[] {
                new Point(0, -1), new Point(0, 1),
                new Point(-1, 0), new Point(1, 0)})
                    {
                        Point next = new Point(cur.X + off.X, cur.Y + off.Y);
                        if (next.X >= 0 && next.Y >= 0 &&
                            next.X < data.Width &&
                            next.Y < data.Height)
                        {
                            if (bits[next.X + next.Y * data.Stride / 4] == floodFrom)
                            {
                                check.AddLast(next);
                                bits[next.X + next.Y * data.Stride / 4] = floodTo;
                            }
                        }
                    }
                }
            }

            Marshal.Copy(bits, 0, data.Scan0, bits.Length);
            bitmap.UnlockBits(data);
        }
    }
}