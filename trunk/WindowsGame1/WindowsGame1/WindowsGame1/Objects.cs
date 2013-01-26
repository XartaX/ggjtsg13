using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using WindowsGame1.View;

namespace WindowsGame1
{
    class Objects
    {

        Texture2D[] texture;
        Vector2[] positions;
        Animation[] animations;
        bool[] exists;
        Animation test = new Animation();

        public int speed;
        public int counter=0;
        public int count;
        

        public void Initialize(int speed, int count)
        {

            this.count = count;
            texture = new Texture2D[count];
            positions = new Vector2[count];
            animations = new Animation[count];
            exists = new bool[count];

            //for (int i = 0; i < count; i++)
            //{
            //    texture[i] = content.Load<Texture2D>(texturePath + (i + 1).ToString().PadLeft(2, '0'));
            //}

            this.speed = speed;

            //int counter = -1, mult = 0;
            //for (int i = 0; i < count; i++)
            //{
            //    if (counter == 3)
            //    {
            //        mult++;
            //        counter = -1;
            //    }
            //    counter++;
            //    positions[i] = new Vector2(counter * texture[i].Width, mult * texture[i].Height);
            //    Console.WriteLine(("Y: " + mult * texture[i].Height).PadRight(8, ' ') + "X: " + counter * texture[i].Width);
            }

         public void addObject(ContentManager content, String texturePath, int x, int y, int ScreenWidth, int ScreenHeight)
         {
             if(counter < count)
             {
                 texture[counter] = content.Load<Texture2D>("Elements/interactive/sheet/object__01");
                 positions[counter] = new Vector2(x, y);
                 animations[counter] = new Animation();
                 animations[counter].Initialize(texture[counter], positions[counter], 204, 320, ScreenWidth, ScreenHeight, 10, 100, Microsoft.Xna.Framework.Color.White, true, 100);
                 exists[counter] = true;
                 counter++;
             }
        }

        public void Update()
        {
        }

        public void Draw(Camera spriteBatch, SpriteBatch SP)
        {

            for (int i = 0; i < count; i++)
            {
                Vector2 t = spriteBatch.ApplyTransformations(positions[i]);
                //positions[i] = spriteBatch.ApplyTransformations(positions[i]);
                //animations[i].Draw(SP);
                SP.Draw(texture[i], new Rectangle((int)t.X,(int)t.Y, 200,320 ), Microsoft.Xna.Framework.Color.White);
            }


        }

    }

}
