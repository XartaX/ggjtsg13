using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{
    class Map1
    {
        public List<Vector2> Ground;
        public List<Vector2> Wall;
        Texture2D Black;
        public void Initialize(Texture2D Black)
        {
            this.Black = Black;
            int StartX=50;
            int StartY=600;
            Ground = new List<Vector2>();
            Wall = new List<Vector2>();
            for (int i = 0; i < 100; i++)
            {
                Ground.Add(new Vector2(StartX, StartY));
                StartX += 5;
                //StartY -= 5;
            }
            StartX = 50;
            StartY = 350;
            for (int i = 0; i < 100; i++)
            {
                Ground.Add(new Vector2(StartX, StartY));
                StartX += 5;
                //StartY -= 5;
            }


            StartX = 300;
            StartY = 300;
            for (int i = 0; i < 60; i++)
            {
                Wall.Add(new Vector2(StartX, StartY));
                StartY -= 5;
                
            }

        }

        public void Draw(SpriteBatch spritebatch)
        {
           
                  for (int i = 0; i < Ground.Count; i++)
            {
                      spritebatch.Draw(Black, Ground[i], Color.White);
            }
                  for (int i = 0; i < Wall.Count; i++)
                  {
                      spritebatch.Draw(Black, Wall[i], Color.White);
                  }
        }
    }
}
