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
    class Animation_multipleTextures
    {

        List<Texture2D> Spritestrip = new List<Texture2D>();
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

        public void Initialize(Vector2 Position, int frameWidth, int frameHeigth, int ScreenWidth, int ScreenHeigth, int frameCount, int frameTime, Color colour, bool Looping, int Percentage)
        {
            this.color = colour;
            this.frameWidth = frameWidth;
            this.frameHeigth = frameHeigth;
            this.frameCount = frameCount;
            this.frameTime = frameTime;
            this.Looping = Looping;
            this.Position = Position;
            
            Scale = (float)((double)Percentage / (double)100);//Casting shit'n yo
            elapsedTime = 0;
            currentFrame = 0;
            Active = true;

            sourceRect = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeigth);
            destRect = new Rectangle((int)Position.X, (int)Position.Y, (int)(ScreenWidth * 0.01 * Percentage), (int)(ScreenHeigth * 0.01 * Percentage));
        }

        public void addTexture(Texture2D texture)
        {
            Spritestrip.Add(texture);
        }

        public void Update(GameTime gameTime, bool Animate)
        {
            if (!Active)
            {
                return;
            }

            if (Animate)
            {
                elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (elapsedTime > frameTime)
                {
                    currentFrame++;
                    if (currentFrame == frameCount)
                    {
                        currentFrame = 0;
                    }
                    elapsedTime = 0;
                }
            }
            sourceRect.X = currentFrame * frameWidth;
            sourceRect.Width = frameWidth;
            sourceRect.Height = frameHeigth;
        }
        public void updatePos(Vector2 pos)
        {
            this.Position = pos;
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
            }
            sourceRect = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeigth);
        }

        public void Draw(SpriteBatch spritebatch)
        {
            if (Active)
            {
                for (int i = 0; i < Spritestrip.Count; i++)
                {
                    spritebatch.Draw(Spritestrip[i], Position, sourceRect, Color.White,
                        0, new Vector2(0, 0), Scale, SpriteEffects.None, 1);
                }
            }
        }

    }
}
