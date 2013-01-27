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
        
        GameTime gt;

        // list"2" versions are for animations stored as different images (not single sheets)
        List<Texture2D> texture = new List<Texture2D>();
        List<Texture2D> texture2 = new List<Texture2D>();
        List<Vector2> positions = new List<Vector2>();
        List<Vector2> positions2 = new List<Vector2>();
        List<Animation> animations = new List<Animation>();
        List<Animation_multipleTextures> animations2 = new List<Animation_multipleTextures>();
        List<bool> exists = new List<bool>();
        List<bool> exists2 = new List<bool>();
        
        Vector2 temp = new Vector2();

        public int speed;
        public int counter = 0;
        public int counter2 = 0;
        

        
      
         public void addObject(ContentManager content, String texturePath, int locationX, int locationY, int ScreenWidth, int ScreenHeight, int objectWidth, int objectHeight, int frameTime, int frameCount)
         {
                 texture.Add(content.Load<Texture2D>(texturePath));
                 positions.Add(new Vector2(locationX, locationY));
                 animations.Add(new Animation());
                 animations[counter].Initialize(texture[counter], positions[counter], objectWidth, objectHeight, ScreenWidth, ScreenHeight, frameCount, frameTime, Microsoft.Xna.Framework.Color.White, true, 100);
                 animations[counter].Animate = true;
                 exists.Add(true);
                 counter++;
        }

         public void addObjectType2(ContentManager content, List<String> texturePath, int locationX, int locationY, int ScreenWidth, int ScreenHeight, int objectWidth, int objectHeight, int frameTime, int frameCount)
        {
            animations2.Add(new Animation_multipleTextures());
            positions2.Add(new Vector2(locationX, locationY));
            animations2[counter2].Initialize(positions2[counter2], objectWidth, objectHeight, ScreenWidth, ScreenHeight, frameCount, frameTime, Microsoft.Xna.Framework.Color.White, true, 100);
            exists2.Add(true);

            int textureCount = 0;

            for (int i = 0; i < texturePath.Count; i++)
            {
                texture2.Add(content.Load<Texture2D>(texturePath[i]));
                textureCount++;
            }
            
            for (int i = 0; i < textureCount; i++)
            {
                animations2[counter2].addTexture(texture2[counter2+i]);
            }
             counter2++;
        }

         public void startAnimation(int input, int type)
         {
             
             if(type==1)
                 for (int i = 0; i < animations.Count; i++)
                 {
                     if (i == input)
                     {

                         animations[i].Update(gt);

                     }
                 }
             else 
                 for (int i = 0; i < animations2.Count; i++)
                     if (i == input)
                         animations2[i].Update(gt, true);
         }

         public void Update(GameTime gt)
        {

            this.gt = gt;

        }

        //Moves the object, n==index of object to change, type=pos type 1/2
        public void changePos(int n, Vector2 pos, Camera cam, int type)
        {

            if (type == 1)
            {
                for (int i = 0; i < positions.Count; i++)
                {
                    if (i == n)
                    {
                        pos = cam.ApplyTransformations(positions[i]);
                        positions[n] = pos;
                        animations2[n].updatePos(pos);
                    }
                }
            }
            else
            {
                for (int i = 0; i < positions2.Count; i++)
                {
                    if (i == n)
                    {
                        pos = cam.ApplyTransformations(positions2[i]);
                        positions2[n] = pos;
                        animations2[n].updatePos(pos);
                    }
                }
            }
        }

        public void updatePos(Camera cam)
        {

            for (int i = 0; i < animations.Count; i++)
            {
                temp = cam.ApplyTransformations(positions[i]);
                animations[i].updatePos(temp);
            }

            for (int i = 0; i < animations2.Count; i++)
            {
                temp = cam.ApplyTransformations(positions2[i]);
                animations2[i].updatePos(temp);
            }
        }

        public void Draw(Camera cam, SpriteBatch SP)
        {
            updatePos(cam);
            for (int i = 0; i < animations.Count; i++)
            {
                animations[i].Draw(SP);
            }
            for (int i = 0; i < animations2.Count; i++)
            {
                animations2[i].Draw(SP);
            }


        }

    }

}
