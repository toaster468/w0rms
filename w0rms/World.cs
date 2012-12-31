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
        public bool wormIsColliding = false;
        public Worm hi;

        public World(GameState parent)
        {
            Parent = parent;
            Ents = new List<Entity>();
            LevelTerrain = new Terrain(Resources<Texture2D>.Get("level"));
            //LevelTerrain = new Terrain(500, 250);
            //LevelTerrain.TheLevel.Position = new Vector2(-500, -250);
            //Camera.Move(new Vector2(-800, -600));
            Brush = new Deformer(Resources<Texture2D>.Get("genericDeformer"));
            hi = new Worm("hi");
            AddEntity(hi);
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
                Brush.Deform(LevelTerrain.TheLevel.MyTexture, GetAbsX() - 64, GetAbsY() - 64);
                Console.WriteLine("AAAAAAA");
                LevelTerrain.RecalculateCollision();
                Console.WriteLine("BBBBB");
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                hi.Position.Y -= 10;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                hi.Position.Y += 10;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                hi.Position.X -= 10;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                hi.Position.X += 10;
            }

            if (LevelTerrain.TheLevel.CollidesWith(hi.Sprite))
            {
                wormIsColliding = true;
            }
            else
            {
                wormIsColliding = false;
            }
        }

        public void Render()
        {
            LevelTerrain.TheLevel.Draw();

            Text boop = new Text();
            boop.ToDraw = "Absolute Mouse Pos: " + new Vector2(GetAbsX(), GetAbsY()).ToString() + "\n" + wormIsColliding.ToString();
            boop.Position.X = Camera.Pos.X - (TheGame.Shazam.Viewport.Width / 2);
            boop.Position.Y = Camera.Pos.Y - (TheGame.Shazam.Viewport.Height / 2);
            boop.DrawColor = Color.Purple;
            boop.Draw();

            for (int i = 0; i < Ents.Count; i++)
            {
                Ents[i].Draw();
            }
        }
    }
}