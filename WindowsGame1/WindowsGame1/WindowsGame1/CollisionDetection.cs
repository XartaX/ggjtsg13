using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;
using WindowsGame1.View;

namespace WindowsGame1
{
    class CollisionDetection
    {
        Bitmap Collisionbmp;

        public void CheckCollision
            (
            ref GameTime gameTime,
            ref Bitmap Collisionbmp,
            ref int FireEmitterWCX,
            ref int FireEmitterWCY,
            ref int shootTime,
            ref Map1 map,
            ref ToTo MainCharracter,
            ref Camera camera,
            ref bool CharMoveR,
            ref bool CharMoveL,
            ref PlayState playState,
            ref Texture2D[] MainCharracterTextureActive,
            ref bool[] CrashDirection,
            ref bool Godmode,
            ref int GravityValue,
            ref bool IsGravity,
            ref bool IsJumping
            )
        {
            this.Collisionbmp = Collisionbmp;
            //Collision1
            KeyboardState key = Keyboard.GetState();
            //collision test for the particle against the map
            if (Collisionbmp.GetPixel(FireEmitterWCX, FireEmitterWCY).R == 255)
            {
                shootTime = 0;
            }

            //Collision test for the particle against the vine plant
            if (FireEmitterWCX > 1240 && FireEmitterWCX < 1444 &&
                FireEmitterWCY > 285 && FireEmitterWCY < 605)
            {
                if (map.vineDestroyed == false)
                {
                    Console.WriteLine("Success!");
                    map.vineDestroyed = true;
                    shootTime = 0;
                }
            }
            //Collision test for the particle against the pillar
            if (FireEmitterWCX > 980 && FireEmitterWCX < 1180 &&
                FireEmitterWCY > 1690 && FireEmitterWCY < 2080)
            {
                if (map.pillarDestroyed == false)
                {
                    Console.WriteLine("Success!");
                    map.pillarShotCount++;
                    map.destroyPillar();
                    shootTime = 0;
                }
            }
            ///Gets the Characters real position relative to the world map
            int CharX, CharY, CharOffset;
            if (key.IsKeyDown(Keys.D))
            {
                CharOffset = 175;
            }
            else
            {
                CharOffset = 0;
            }

            CharX = (int)(MainCharracter.Position.X + camera.Position.X + CharOffset);
            CharY = (int)(MainCharracter.Position.Y + camera.Position.Y + 58);

            //Collision test for the character against the vine plant
            if (CharX > 1240 && CharX < 1444 &&
                CharY > 285 && CharY < 605)
            {
                if (!map.vineDestroyed)
                {
                    CharMoveR = false;
                }
            }
            else
            {
                CharMoveR = true;
            }
            //Collision test for the character against the pillar
            if (CharX > 980 && CharX < 1180 &&
                CharY > 1690 && CharY < 2080)
            {
                if (map.pillarDestroyed == false)
                {
                    CharMoveL = false;
                }
            }
            else
            {
                CharMoveL = true;
            }
            //Collision test for the character against the Toxic pool(tm)
            if (CharX > 2800 && CharX < 3650 &&
                CharY > 4370 && CharY < 4650)
            {
                playState = PlayState.gameOver;
                //might be a bug on the line above
            }

            //Collision2-----------------------

            //Collision detection for the character against the map
            Microsoft.Xna.Framework.Rectangle CharRect
                = new Microsoft.Xna.Framework.Rectangle(
                    (int)(MainCharracter.Position.X + camera.Position.X), //X position
                    (int)(MainCharracter.Position.Y + camera.Position.Y), //Y position
                    MainCharracterTextureActive[0].Width / 4, //Widht
                    MainCharracterTextureActive[0].Height);   //Height
            for (int i = 0; i < CrashDirection.Length; i++)
            {
                CrashDirection[i] = false;
            }
            //0     Upwards
            //1     Right
            //2     Downwards
            //3     Left

            if (!Godmode)
            {
                GravityValue = 5;
                //Quadratic hitbox
                if (CollisionDetection(CharRect.X + 110, CharRect.Y + 85))//Upper hitbox
                {
                    CrashDirection[0] = true;
                    IsGravity = true;
                    IsJumping = false;

                }
                if (CollisionDetection(CharRect.X + 160, CharRect.Y + 130))//Right hitbo
                {
                    CrashDirection[1] = true;

                }
                if (CollisionDetection(CharRect.X + 100, CharRect.Y + 200))//Bottom hitbox
                {
                    CrashDirection[2] = true;

                }
                if (CollisionDetection(CharRect.X + 50, CharRect.Y + 130))//Left hitbox
                {
                    CrashDirection[3] = true;
                    IsGravity = false;
                    MainCharracter.Position.Y -= 1f;

                }

                //Two-points hitbox to prevent characther to not clip through upwards hills
                if (CollisionDetection(CharRect.X + 120, CharRect.Y + 200))//Rigth bottom hitbox
                {
                    IsGravity = false;
                    MainCharracter.Position.Y -= 1f;
                    GravityValue = 0;
                    MainCharracter.Update(gameTime);
                    CrashDirection[4] = true;
                }
                if (CollisionDetection(CharRect.X + 80, CharRect.Y + 200))//Left bottom hitbox
                {

                    IsGravity = false;
                    MainCharracter.Position.Y -= 1f;
                    GravityValue = 0;
                    MainCharracter.Update(gameTime);
                    CrashDirection[4] = true;
                }
            }
            else
            {
                IsGravity = false;
            }
            //End Collision2

        }//End collision

        private bool CollisionDetection(int PosX, int PosY)
        {   //Prevents the next test to not test outside of the given picture
            if (PosX > 0 && PosY > 0 && PosX < Collisionbmp.Width && PosY < Collisionbmp.Height)
            {   //Checks if a pixel in the collisionmap is white, checks only red as checking blue and green would be redundant
                if (Collisionbmp.GetPixel(PosX, PosY).R == 255)//Collision true
                    return true;

                else//Collision false
                    return false;
            }
            else
            {
                return true;
            }
        }
    }//end Class
}//End Namespace
