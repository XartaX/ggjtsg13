using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WindowsGame1.View;

namespace WindowsGame1
{
    class Animation
    {
        Texture2D Spritestrip;
        int elapsedTime;
        int frameTime;
        int frameCount;
        int currentFrame;
        Color color;
        Rectangle sourceRect;
        Rectangle destRect;
        Single Scale;
        public int frameWidth;
        public int frameHeigth;
        public bool Active;
        public bool Looping;
        public Vector2 Position;

        public void Initialize(Texture2D texture, Vector2 Position, int frameWidth, int frameHeigth, int ScreenWidth, int ScreenHeigth, int frameCount, int frameTime, Color colour, bool Looping, int Percentage)
        {
            this.color = colour;
            this.frameWidth = frameWidth;
            this.frameHeigth = frameHeigth;
            this.frameCount = frameCount;
            this.frameTime = frameTime;
            this.Looping = Looping;
            this.Position = Position;
            Spritestrip = texture;
            Scale = (float)((double)Percentage / (double)100);//Casting shit'n yo
            elapsedTime = 0;
            currentFrame = 0;
            Active = true;


            sourceRect = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeigth);
            destRect = new Rectangle((int)Position.X, (int)Position.Y, (int)(ScreenWidth * 0.01 * Percentage), (int)(ScreenHeigth * 0.01 * Percentage));
        }

        public void Update(GameTime gameTime)
        {
            if (!Active)
            {
                return;
            }

            //Vil mulig gjøre denne om til TimeSpan
            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsedTime > frameTime)
            {
                currentFrame++;
                if (currentFrame == frameCount)
                {
                    currentFrame = 0;
                    //if (!Looping) //er ikke i bruk
                    //{
                    //    Active = false;
                    //}
                }
                elapsedTime = 0;
            }
            sourceRect.X = currentFrame * frameWidth;
            sourceRect.Width = frameWidth;
            sourceRect.Height = frameHeigth;

            destRect.X = (int)Position.X;
            destRect.Y = (int)Position.Y;
            destRect.Width =  frameWidth;
            destRect.Height = frameHeigth;


        }

        public void UpdateNotChangeFrame(GameTime gameTime)
        {
            sourceRect.X = currentFrame * frameWidth;
            sourceRect.Width = frameWidth;
            sourceRect.Height = frameHeigth;

            destRect.X = (int)Position.X;
            destRect.Y = (int)Position.Y;
            destRect.Width = frameWidth;
            destRect.Height = frameHeigth;
        }

        public void ChangeFrame()
        {
            if (!Active)
            {
                return;
            }
            currentFrame++;
            if (currentFrame == frameCount)
            {
                currentFrame = 0;
                //if (!Looping) //er ikke i bruk
                //{
                //    Active = false;
                //}
            }


            sourceRect = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeigth);

            destRect = new Rectangle((int)Position.X, (int)Position.Y, frameWidth, frameHeigth);

        }


        public void Draw(Camera spritebatch)
        {

            if (Active)
            {
                SceneNode node = new SceneNode(Spritestrip,Position);
                spritebatch.DrawNode(node);
                
            }

        }
    }
}
