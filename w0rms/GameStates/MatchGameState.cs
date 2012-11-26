using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using w0rms.Entities;
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

        Worm Stan;

        public MatchState()
        {
            CurrentWorld = new World();
            SetHUDChatMessage("");

            Stan = new Worm();

            CurrentWorld.AddEntity(Stan);
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
            TheGame.spriteBatch.Begin();

            CurrentWorld.Render();

            //DRAW HUD

            if (hudDisplayMessage.Length > 0)
            {
                Text messageText = new Text(hudDisplayMessage);
                messageText.DrawColor = Color.Black;
                messageText.Position = new Vector2(TheGame.graphics.PreferredBackBufferWidth / 2, TheGame.graphics.PreferredBackBufferHeight);
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
            if (TheGame.keyboard.IsKeyDown(Keys.A))
            {
                SetHUDChatMessage("hi im brian");
            }
        }
    }
}