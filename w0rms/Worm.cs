using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace w0rms
{
    class Worm : Entity
    {
        public string Name = "stan";
        public float Health = 100.0f;
        public AnimatedSprite Sprite;

        public bool isActive = false;

        private float hSave, vSave, hSpeed, vSpeed;
        private bool canJump;
        private float jumpEnergy;

        public short Team = 0; //0 indexed team number (0 = team1, 1 = team2)

        public Worm(string naam)
        {
            this.Name = naam;
            Sprite = new AnimatedSprite(0, 1);
            Sprite.Load("werm", 3, 150, 32, 32);
        }

        public override void Update(TimeSpan ts)
        {
            Sprite.UpdateFrame((float)ts.TotalMilliseconds);
            Sprite.Position = Position;

            if (isActive) Movement();
            isActive = false;
        }

        private bool PlaceFree(Rectangle bounds)
        {
            return MyWorld.LevelTerrain.CanPass(bounds);
        }

        private void Movement()
        {
            const float maxHSpeed = 2.5f;
            const float maxVSpeed = 6;
            const float acceleration = 1;
            const float jumpPotential = 10;
            const float jumpSpeed = 4;
            const float gravity = 0.7f;
            const float friction = 0.7f;

            // Left/right acceleration
            if (TheGame.keyboard.IsKeyDown(Keys.Left)) hSpeed -= acceleration;
            if (TheGame.keyboard.IsKeyDown(Keys.Right)) hSpeed += acceleration;

            hSpeed *= friction;

            // Done with horizontal math, clamp to maximum speed
            hSpeed = MathHelper.Clamp(hSpeed, -maxHSpeed, maxHSpeed);

            vSpeed += gravity;

            // Check if we are on a solid surface
            var bounds = BoundingBox;
            bounds.Y++;
            var onGround = !PlaceFree(bounds);

            if (canJump)
            {
                // Begin a jump by setting jumpEnergy
                if (onGround)
                    jumpEnergy = jumpPotential;
                canJump = false;
            }

            // As long as you continue to hold W and still have energy to jump you will rise
            if (TheGame.keyboard.IsKeyDown(Keys.Back) && jumpEnergy > 0)
            {
                // Drain energy so you can't float forever
                float usedEnergy = Math.Min(jumpSpeed + vSpeed, jumpEnergy);
                jumpEnergy -= usedEnergy;
                vSpeed -= usedEnergy;
            }

            // Done with vertical math, clamp to maximum speed
            vSpeed = MathHelper.Clamp(vSpeed, -maxVSpeed, maxVSpeed);

            var hMove = hSpeed;
            var vMove = vSpeed;

            // My movement code only works with whole pixel movements (no partial)
            // Figure out how many pixels to move on each axis
            var hRep = (int)Math.Floor(Math.Abs(hMove));
            var vRep = (int)Math.Floor(Math.Abs(vMove));

            // Save whatever fraction of a pixel we can not move this frame
            hSave += (float)(Math.Abs(hMove) - Math.Floor(Math.Abs(hMove)));
            vSave += (float)(Math.Abs(vMove) - Math.Floor(Math.Abs(vMove)));

            // The fraction can add up so we make sure to include any whole pixels that have accumulated
            while (hSave >= 1)
            {
                --hSave;
                ++hRep;
            }

            while (vSave >= 1)
            {
                --vSave;
                ++vRep;
            }

            var testRect = BoundingBox;

            // Now the actual movement
            // Loop for every pixel we need to move
            while (hRep-- > 0)
            {
                // If moving one pixel in that direction creates a collision, stop moving
                testRect.X += Math.Sign(hMove);
                if (!PlaceFree(testRect))
                {
                    hSave = 0;
                    hSpeed = 0;
                    break;
                }

                // Otherwise we continue to move
                Position.X += Math.Sign(hMove);
            }

            testRect = BoundingBox;

            // Same thing here but for the Y axis
            while (vRep-- > 0)
            {
                testRect.Y += Math.Sign(vMove);
                if (!PlaceFree(testRect))
                {
                    vSave = 0;
                    vSpeed = 0;
                    break;
                }

                Position.Y += Math.Sign(vMove);
            }
        }

        public override void Draw()
        {
            Sprite.Draw();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}