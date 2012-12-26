using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace w0rms
{
    class Deformer
    {
        private Texture2D _deformTexture;
        private uint[] _deformData;

        public Deformer(Texture2D deformer)
        {
            DeformTexture = deformer;
        }

        public void Deform(Texture2D level, int px, int py)
        {
            uint[] _levelData = new uint[level.Width * level.Height];
            level.GetData(_levelData, 0, level.Width * level.Height);

            for (int x = 0; x < _deformTexture.Width; x++)
            {
                for (int y = 0; y < _deformTexture.Height; y++)
                {
                    if (px + x < level.Width && py + y < level.Height)
                    {
                        if (px + x >= 0 && py + y >= 0)
                        {
                            var dest = (px + x) + (py + y) * level.Width;
                            var dest2 = x + y * _deformTexture.Width;

                            var alpha = new Color(255, 255, 255, 0).PackedValue;
                            var white = new Color(255, 255, 255, 255).PackedValue;

                            if (_deformData[dest2] != alpha && _levelData[dest] != alpha)
                            {
                                if (_deformData[dest2] == white)
                                    _levelData[dest] = alpha;
                                else
                                    _levelData[dest] = _deformData[dest2];
                            }
                        }
                    }
                }
            }

            level.SetData(_levelData);
        }

        public Texture2D DeformTexture
        {
            get { return _deformTexture; }
            set
            {
                _deformTexture = value;
                _deformData = new uint[_deformTexture.Width * _deformTexture.Height];
                _deformTexture.GetData(_deformData, 0, _deformTexture.Width * _deformTexture.Height);

            }
        }   
    }
}
