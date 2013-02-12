using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WindowsGame1.View;
using WindowsGame1.Particle;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1
{
    class Input
    {
        //This might get a bit big
        ToTo MainCharracter;
        Texture2D[] MainCharracterTextureActive;
        public void UpdateInput
            (
            ref GameTime gameTime,
            ref int mouseX,
            ref int mouseY,
            ref PlayState playState,
            ref bool[] CrashDirection,
            ref bool WasRightLastDirection,
            ref bool CharMoveR,
            ref Camera camera,
            ref int ScreenWidth,
            ref ToTo MainCharracter,
            ref float speed,
            ref float lastCamY,
            ref bool IsGravity,
            ref int SpriteMoover,
            ref bool ShootInProgress,
            ref bool CharMoveL,
            ref bool IsJumping,
            ref TimeSpan AirTime,
            ref int GravityValue,
            ref ParticleSquirter FireParticle,
            ref ParticleSquirter particleEngine,
            ref bool shoot,
            ref int shootTime,
            ref bool ShootExplode,
            ref int ExplodeCount,
            ref Vector3 SpritePosi,
            ref int ScreenHeight,
            ref int frameRate,
            ref bool progressBoard,
            ref bool btnpress,
            ref List<Texture2D> StoryBoard,
            ref int storyCount,
            ref Texture2D[] MainCharracterTextureActive
            )
        {
            this.MainCharracter = MainCharracter;
            this.MainCharracterTextureActive = MainCharracterTextureActive;

            KeyboardState key = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            mouseX = mouse.X;
            mouseY = mouse.Y;

            //Starts the game if player clicks on the given retangle.
            //Is supposed to work only when player is in menu, but no limit is set. yet.
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                if (mouse.X > 900 && mouse.X < 1000 && mouse.Y > 500 && mouse.Y < 600)
                {
                    playState = PlayState.playing;
                }
            }


            //Moves left
            if (key.IsKeyDown(Keys.D) && !CrashDirection[1] && CharMoveR)
            {
                setMainCharracterTextureActive(0);
                WasRightLastDirection = true;

                if (camera.Position.X + ScreenWidth > 5000 - 10)
                {
                    MainCharracter.Position.X += (speed);
                }
                else
                {
                    camera.Translate(new Vector2(speed, 0));
                    MainCharracter.Position.X += 0;
                }

                lastCamY = camera.Position.Y;
                MainCharracter.Animate = true;
                MainCharracter.Update(gameTime);
                IsGravity = true;
                if (SpriteMoover >= 0 && !ShootInProgress)
                {
                    SpriteMoover -= 3;
                }
            }
            //Moves Right
            if (key.IsKeyDown(Keys.A) && !CrashDirection[3] && CharMoveL)
            {
                setMainCharracterTextureActive(1);
                WasRightLastDirection = false;
                if (camera.Position.X < 10)
                {
                    MainCharracter.Position.X -= (speed);
                }
                else
                {
                    camera.Translate(new Vector2(-speed, 0));
                }
                lastCamY = camera.Position.Y;
                MainCharracter.Animate = true;
                MainCharracter.Update(gameTime);
                IsGravity = true;
                if (SpriteMoover <= 200 && !ShootInProgress)
                {
                    SpriteMoover += 3;
                }
            }
            //Jumps
            if (key.IsKeyDown(Keys.W) && !CrashDirection[0])
            {
                if (gameTime.TotalGameTime - AirTime > TimeSpan.FromSeconds(1) && !IsJumping && CrashDirection[2])
                {

                    IsJumping = true;

                    AirTime = gameTime.TotalGameTime;
                }
            }

            //Makes the character jump, could potentially be moved back to the update method
            if (IsJumping && (gameTime.TotalGameTime - AirTime < TimeSpan.FromSeconds(1)))
            {
                camera.Translate(new Vector2(0, -speed));
                //IsGravity = false;
                GravityValue = 0;
                MainCharracter.Position.Y -= 2.5f;
                MainCharracter.Update(gameTime);
                if (ShootInProgress)
                {
                    FireParticle.EmitterLocation += new Vector3(0, 2.5f, 0);
                }
            }
            else
            {
                IsJumping = false;
                IsGravity = true;
                GravityValue = 5;
            }

            //Makes the character go down, might be redundant?
            if (key.IsKeyDown(Keys.S) && !CrashDirection[2])
            {
                camera.Translate(new Vector2(0, speed + speed));
                MainCharracter.Position.Y += 2.5f;
                MainCharracter.Update(gameTime);
                if (ShootInProgress)
                {
                    FireParticle.EmitterLocation += new Vector3(0, -2.5f, 0);
                }
            }



            //Shooting mechanic
            if (shoot && key.IsKeyUp(Keys.Space))
            {
                shoot = false;
                ShootInProgress = true;
                shootTime = 100;
            }
            if (ShootInProgress)
            {
                shootTime--;
                if (WasRightLastDirection)
                    FireParticle.EmitterLocation += new Vector3(5, 0, 0);
                else
                    FireParticle.EmitterLocation += new Vector3(-5, 0, 0);
                if (shootTime <= 0)
                {
                    ShootInProgress = false;
                    ShootExplode = true;
                    ExplodeCount = 7;
                    FireParticle.exploding = true;
                    FireParticle.Spread = (float)(1 / 5d);
                }
            }
            else if (shootTime == 0 && ShootExplode)
            {
                ExplodeCount--;
                if (ExplodeCount == 0)
                {
                    ShootExplode = false;
                    FireParticle.exploding = false;
                    FireParticle.Spread = 1f;
                }
            }
            else
            {
                ShootExplode = false;
                FireParticle.exploding = false;
                FireParticle.Spread = 1f;
                FireParticle.EmitterLocation = new Vector3(MainCharracter.Position.X + SpriteMoover, MainCharracter.Position.Y, 0);
            }

            if (key.IsKeyDown(Keys.Space) && !shoot && !ShootExplode && !ShootInProgress)
            {
                shoot = true;
                particleEngine.emitFlag = true;
            }
            else if (playState == PlayState.menu)
            {
                particleEngine.emitFlag = true;
            }
            else
            {
                particleEngine.emitFlag = false;
            }
            float SpeedEmiter = 5, x1 = 0, y1 = 0;
            if (key.IsKeyDown(Keys.W))
                y1 -= SpeedEmiter;
            if (key.IsKeyDown(Keys.S))
                y1 += SpeedEmiter;
            if (key.IsKeyDown(Keys.D))
                x1 += SpeedEmiter;
            if (key.IsKeyDown(Keys.A))
                x1 -= SpeedEmiter;
            SpritePosi += new Vector3(x1, y1, 0.0f);

            if (SpritePosi.X >= ScreenWidth)
            {
                SpritePosi = new Vector3(0.0f, SpritePosi.Y, 0.0f);
            }
            else if (SpritePosi.X <= 0)
            {
                SpritePosi = new Vector3((float)ScreenWidth, SpritePosi.Y, 0.0f);
            }
            if (SpritePosi.Y >= ScreenHeight)
            {
                SpritePosi = new Vector3(SpritePosi.X, 0.0f, 0.0f);
            }
            else if (SpritePosi.Y <= 0)
            {
                SpritePosi = new Vector3(SpritePosi.X, (float)ScreenHeight, 0.0f);
            }
            FireParticle.Update();

            particleEngine.EmitterLocation = new Vector3(mouseX, mouseY, 0);
            particleEngine.frameRate = frameRate;
            particleEngine.Update();
            particleEngine.Shei = ScreenHeight;
            particleEngine.Swid = ScreenWidth;
            if (key.IsKeyDown(Keys.Enter) && playState == PlayState.story)
            {
                progressBoard = true;
            }
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                btnpress = true;
            }
            if ((mouse.LeftButton == ButtonState.Released && btnpress) || key.IsKeyUp(Keys.Enter) && progressBoard && playState == PlayState.story)
            {
                btnpress = false;
                if (StoryBoard.Count < storyCount + 2)
                {
                    playState = PlayState.playing;
                }
                else
                {
                    storyCount++;
                    progressBoard = false;
                }

            }
            if (key.IsKeyDown(Keys.F1) && playState == PlayState.story)
            {
                playState = PlayState.playing;
            }
        }
         
        private void setMainCharracterTextureActive(int Index)
        {
            if (MainCharracter.Spritestrip != MainCharracterTextureActive[Index])
            {
                MainCharracter.Spritestrip = MainCharracterTextureActive[Index];
            }
        }
    }
}
