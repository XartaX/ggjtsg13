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

        List<Texture2D> texture = new List<Texture2D>();
        List<Vector2> positions = new List<Vector2>();
        List<Animation> animations = new List<Animation>();
        List<bool> exists = new List<bool>();
        
        Vector2 temp = new Vector2();

        public int speed;
        public int counter=0;
      
         public void addObject(ContentManager content, String texturePath, int locationX, int locationY, int ScreenWidth, int ScreenHeight)
         {
            
                 texture.Add(content.Load<Texture2D>("Elements/interactive/sheet/object__01"));
                 positions.Add(new Vector2(locationX, locationY));
                 animations.Add(new Animation());
                 animations[counter].Initialize(texture[counter], positions[counter], 204, 320, ScreenWidth, ScreenHeight, 10, 100, Microsoft.Xna.Framework.Color.White, true, 100);
                 exists.Add(true);
                 counter++;
            
        }

        public void Update()
        {



        }
        public void updatePos(Camera cam)
        {

            for (int i = 0; i < texture.Count; i++)
            {

                temp = cam.ApplyTransformations(positions[i]);
                animations[i].updatePos(temp);
                
            }

        }

        public void Draw(Camera spriteBatch, SpriteBatch SP)
        {

            for (int i = 0; i < texture.Count; i++)
            {
                updatePos(spriteBatch);
                //Vector2 t = spriteBatch.ApplyTransformations(positions[i]);
                //positions[i] = spriteBatch.ApplyTransformations(positions[i]);
                animations[i].Draw(SP);
                //SP.Draw(texture[i], new Rectangle((int)t.X,(int)t.Y, 200,320 ), Microsoft.Xna.Framework.Color.White);
            }


        }

    }

}
