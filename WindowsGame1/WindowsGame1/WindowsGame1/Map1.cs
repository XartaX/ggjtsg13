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
        Texture2D Black;
        public void Initialize(Texture2D Black)
        {
            this.Black = Black;
            int StartX=50;
            int StartY=400;
            Ground = new List<Vector2>();
            for (int i = 0; i < 10; i++)
            {
                Ground.Add(new Vector2(StartX, StartY));
                StartX += 30;
                StartY -= 31;
            }


        }

        public void Draw(SpriteBatch spritebatch)
        {
           
                  for (int i = 0; i < Ground.Count; i++)
            {
                //Ground[i]
                      spritebatch.Draw(Black, Ground[i], Color.White);
            }
        }
    }
}
