using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public Camera2D Camera = new Camera2D();
        Sprite background = new Sprite("hi"); //background picture to help illustrate Camera2D movement since we dont have a level system yet

        public string HUDChatMessage
        {
            get;
            private set;
        }

        string hudDisplayMessage;
        DateTime lastHudMessageTick;
        TimeSpan hudMessageTickDelay;

        Worm Stan;

        public MatchState()
        {
            CurrentWorld = new World();
            SetHUDChatMessage("");

            Team BestTeam = new Team("The Best", 0);
            Stan = new Worm("Stan");
            BestTeam.AddWorm(Stan);

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
            Camera.get_transformation(TheGame.graphics.GraphicsDevice));
            background.Draw();
            CurrentWorld.Render();

            //DRAW HUD

            if (hudDisplayMessage.Length > 0)
            {
                Text messageText = new Text(hudDisplayMessage);
                messageText.DrawColor = Color.Black;
                messageText.Position = new Vector2(Camera.Pos.X, Camera.Pos.Y);
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
                Camera.Move(0, -10);
                //to draw something at a position relative to the camera (eg. HUD elements) draw at Camera.Pos and offset from there
            }
            if (TheGame.keyboard.IsKeyDown(Keys.A))
            {
                Camera.Move(-10, 0);
            }
            if (TheGame.keyboard.IsKeyDown(Keys.S))
            {
                Camera.Move(0, 10);
            }
            if (TheGame.keyboard.IsKeyDown(Keys.D))
            {
                Camera.Move(10, 0);
            }

            if (TheGame.keyboard.IsKeyDown(Keys.Q))
            {
                Camera.ZoomIn(0.01f);
            }
            if (TheGame.keyboard.IsKeyDown(Keys.E))
            {
                Camera.ZoomIn(-0.01f);
            }

            if (TheGame.keyboard.IsKeyDown(Keys.Space))
            {
                SetHUDChatMessage("hi im brian");
            }
        }
    }
}