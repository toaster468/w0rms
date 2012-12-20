using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using w0rms.Rendering;

namespace w0rms
{
    class TheGame : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static ContentManager MainContentLoader;
        static GameState CurrentState;
        public static KeyboardState keyboard;
        public static Random Rng;
        public static GraphicsDevice Shazam;

        public TheGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "w0rmsContent";
            Rng = new Random();
        }

        protected override void Initialize()
        {
            base.Initialize();
            ChangeState(new GameStates.MatchState());
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Shazam = GraphicsDevice;
            MainContentLoader = Content;
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Escape))
                this.Exit();

            if (CurrentState != null)
                CurrentState.Update(gameTime.ElapsedGameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (CurrentState != null)
                CurrentState.Render();

            base.Draw(gameTime);
        }

        public static void ChangeState(GameState state)
        {
            if (CurrentState != null)
                CurrentState.End();

            CurrentState = state;
            CurrentState.Start();
        }
    }
}