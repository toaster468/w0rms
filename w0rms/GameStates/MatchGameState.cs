using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using w0rms.Rendering;

namespace w0rms.GameStates
{
    //gamestate for main Match/Game
    class MatchState : GameState
    {
        World CurrentWorld;

        public string HUDChatMessage
        {
            get;
            private set;
        }

        string hudDisplayMessage;
        DateTime lastHudMessageTick;
        TimeSpan hudMessageTickDelay;
        Texture2D Generated;

        Sprite background = new Sprite();

        int octaves = 12;
        Worm Stan;

        public MatchState()
        {
            CurrentWorld = new World(this);
            SetHUDChatMessage("");

            Team BestTeam = new Team("The Best", 0);
            Stan = new Worm("Stan");
            BestTeam.AddWorm(Stan);

            Generated = GenerateNoiseMap(256, 256, 4);
            background = new Sprite(Generated);
            Console.WriteLine(BestTeam.ToString());
        }

        public void Start()
        {
            lastHudMessageTick = DateTime.Now;
            hudMessageTickDelay = new TimeSpan(0, 0, 0, 0, 80);
        }

        public void End()
        {
        }

        public void SetHUDChatMessage(string text)
        {
            HUDChatMessage = text;
            hudDisplayMessage = "";
            lastHudMessageTick = DateTime.Now;
        }

        public void Render()
        {
            TheGame.spriteBatch.Begin(SpriteSortMode.Immediate,
            BlendState.AlphaBlend,
            null,
            null,
            null,
            null,
            CurrentWorld.Camera.get_transformation(TheGame.graphics.GraphicsDevice));
            CurrentWorld.Render();
            //background.Draw();
            //DRAW HUD

            if (hudDisplayMessage.Length > 0)
            {
                Text messageText = new Text(hudDisplayMessage);
                messageText.DrawColor = Color.Black;
                messageText.Position = new Vector2(CurrentWorld.Camera.Pos.X, CurrentWorld.Camera.Pos.Y);
                messageText.Position.Y -= messageText.Font.MeasureString("T").Y;
                messageText.Position.X -= messageText.Font.MeasureString(messageText.ToDraw).X / 2;

                messageText.Draw();
            }
            TheGame.spriteBatch.End();
        }

        public void Update(TimeSpan ts)
        {
            Input();
            CurrentWorld.Update(ts);

            //HUD STEPPING

            if (DateTime.Now - lastHudMessageTick >= hudMessageTickDelay)
            {
                lastHudMessageTick = DateTime.Now;
                if (hudDisplayMessage.Length < HUDChatMessage.Length)
                {
                    hudDisplayMessage += HUDChatMessage[hudDisplayMessage.Length];
                }
            }
        }

        public void Input()
        {
            if (TheGame.keyboard.IsKeyDown(Keys.W))
            {
                CurrentWorld.Camera.Move(0, -10);
                //to draw something at a position relative to the CurrentWorld.Camera (eg. HUD elements) draw at CurrentWorld.Camera.Pos and offset from there
            }
            if (TheGame.keyboard.IsKeyDown(Keys.A))
            {
                CurrentWorld.Camera.Move(-10, 0);
            }
            if (TheGame.keyboard.IsKeyDown(Keys.S))
            {
                CurrentWorld.Camera.Move(0, 10);
            }
            if (TheGame.keyboard.IsKeyDown(Keys.D))
            {
                CurrentWorld.Camera.Move(10, 0);
            }

            if (TheGame.keyboard.IsKeyDown(Keys.Q))
            {
                CurrentWorld.Camera.ZoomIn(0.01f);
            }
            if (TheGame.keyboard.IsKeyDown(Keys.E))
            {
                CurrentWorld.Camera.ZoomIn(-0.01f);
            }

            if (TheGame.keyboard.IsKeyDown(Keys.Space))
            {
                Generated = GenerateNoiseMap(256, 256, octaves);
                background = new Sprite(Generated);
            }
        }

        private Texture2D GenerateNoiseMap(int width, int height, int octaves)
        {
            Texture2D noiseTexture;

            var data = new float[width * height];

            var min = float.MaxValue;
            var max = float.MinValue;

            Noise2d.Reseed();

            var frequency = 0.5f;
            var amplitude = 1f;
            var persistence = 0.25f;

            for (var octave = 0; octave < octaves; octave++)
            {
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
    }
}