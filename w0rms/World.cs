using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using w0rms.Rendering;

namespace w0rms
{
    class World
    {
        public List<Entity> Ents;
        public Terrain LevelTerrain;
        private Deformer Brush;
        private GameState Parent;
        public Camera2D Camera = new Camera2D();

        public World(GameState parent)
        {
            Parent = parent;
            Ents = new List<Entity>();
            LevelTerrain = new Terrain(TheGame.MainContentLoader.Load<Texture2D>("level"));
            //LevelTerrain = new Terrain(500, 250);
            //LevelTerrain.TheLevel.Position = new Vector2(-500, -250);
            Brush = new Deformer(TheGame.MainContentLoader.Load<Texture2D>("genericDeformer"));
        }

        public void AddEntity(Entity ent)
        {
            Ents.Add(ent);
            ent.MyWorld = this;
        }

        public int GetAbsX()
        {
            return (int)(Camera.Pos.X + Mouse.GetState().X - (TheGame.Shazam.Viewport.Width / 2));
        }

        public int GetAbsY()
        {
            return (int)(Camera.Pos.Y + Mouse.GetState().Y - (TheGame.Shazam.Viewport.Height / 2));
        }

        public void Update(TimeSpan ts)
        {
            for (int i = 0; i < Ents.Count; i++)
            {
                Ents[i].Update(ts);
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Brush.Deform(LevelTerrain.TheLevel.MyTexture, GetAbsX(), GetAbsY());
            }
        }

        public void Render()
        {
            LevelTerrain.TheLevel.Draw();

            Text boop = new Text();
            boop.ToDraw = new Vector2(GetAbsX(), GetAbsY()).ToString();
            boop.Position = Camera.Pos;
            boop.DrawColor = Color.Purple;
            boop.Draw();

            for (int i = 0; i < Ents.Count; i++)
            {
                Ents[i].Draw();
            }
        }
    }
}