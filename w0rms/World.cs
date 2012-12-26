using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace w0rms
{
    class World
    {
        public List<Entity> Ents;
        public Terrain LevelTerrain;
        private Deformer Brush;
        private GameState Parent;

        public World(GameState parent)
        {
            Parent = parent;
            Ents = new List<Entity>();
            LevelTerrain = new Terrain(TheGame.MainContentLoader.Load<Texture2D>("level"));
            Brush = new Deformer(TheGame.MainContentLoader.Load<Texture2D>("genericDeformer"));
        }

        public void AddEntity(Entity ent)
        {
            Ents.Add(ent);
            ent.MyWorld = this;
        }

        public void Update(TimeSpan ts)
        {
            for (int i = 0; i < Ents.Count; i++)
            {
                Ents[i].Update(ts);
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Brush.Deform(LevelTerrain.TheLevel, Mouse.GetState().X, Mouse.GetState().Y);
            }
        }

        public void Render()
        {
            TheGame.spriteBatch.Draw(LevelTerrain.TheLevel, new Vector2(0, 0), Color.White);

            for (int i = 0; i < Ents.Count; i++)
            {
                Ents[i].Draw();
            }
        }
    }
}