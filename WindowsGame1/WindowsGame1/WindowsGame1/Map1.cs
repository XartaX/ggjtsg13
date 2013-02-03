using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using WindowsGame1.View;

namespace WindowsGame1
{
    class Map1
    {
        public List<Vector2> Ground;
        public List<Vector2> Wall;
        Texture2D Black;
        Backgrounds splitter = new Backgrounds();
        Objects staticObjects = new Objects();
        Objects foregroundObjects = new Objects();
        List<String> paths;
        public bool vineDestroyed=false;
        public bool pillarDestroyed = false;
        public int pillarShotCount = 1;

        public void addToStringList(String bane, int antall)
        {
            paths = new List<String>();
            for (int i = 0; i < antall; i++)
            {
                paths.Add(bane + (i + 1).ToString().PadLeft(2, '0'));
            }
        }
        int counter = 0;
        public void destroyPillar()
        {
            if (pillarShotCount < 11)
            {
                Console.WriteLine("1");
                staticObjects.setFrame(2, 1, 1);
            }
            else if (pillarShotCount <21)
            {
                Console.WriteLine("2");
                staticObjects.setFrame(2, 1, 2);
            }
            else if (pillarShotCount <31)
            {
                Console.WriteLine("3");
                pillarDestroyed = true;
            }
            
        }

        public void Initialize(Texture2D Black, ContentManager Content, int ScreenWidth, int ScreenHeight)
        {
            this.Black = Black;
            int StartX=50;
            int StartY=600;
            Ground = new List<Vector2>();
            Wall = new List<Vector2>();

            //vineWall = Content.Load<Texture2D>("vine-wall_204x320");
            splitter.Initialize(Content, "Foreground_tutorial/TutorialMap-foreground__", ScreenWidth, 0, 16);
            staticObjects.addObject(Content, "Elements/interactive/sheet/object__01", 1240, 285, ScreenWidth, ScreenHeight, 204, 320, 100, 10);
            staticObjects.addObject(Content, "Elements/interactive/sheet/object__03", 1560, 4400, ScreenWidth, ScreenHeight, 410, 529, 100, 1);
            staticObjects.addObject(Content, "Elements/interactive/sheet/object__04", 980, 1690, ScreenWidth, ScreenHeight, 200, 390, 150, 10);
            staticObjects.addObject(Content, "Elements/interactive/sheet/object__05", 3300, 2200, ScreenWidth, ScreenHeight, 1604, 433, 100, 1);
            staticObjects.addObject(Content, "Elements/interactive/sheet/object__07", 3750, 620, ScreenWidth, ScreenHeight, 100, 106, 100, 1);
            staticObjects.addObject(Content, "Elements/interactive/sheet/object__08", 870, 4450, ScreenWidth, ScreenHeight, 125, 126, 100, 1);
            foregroundObjects.addObject(Content, "Elements/interactive/sheet/object__02", 2800, 420, ScreenWidth, ScreenHeight, 430, 350, 100, 4);
            addToStringList("Elements/interactive/sheet/boiling-water/boil-water_", 10);
            staticObjects.addObjectType2(Content, paths, 1580, 2170, ScreenWidth, ScreenHeight, 932, 251, 100, 10);
            addToStringList("Elements/interactive/sheet/toxic-pool/toxic-pool_", 6);
            staticObjects.addObjectType2(Content, paths, 2800, 4370, ScreenWidth, ScreenHeight, 848, 280, 150, 6);
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


            //StartX = 300;
            //StartY = 300;
            //for (int i = 0; i < 60; i++)
            //{
            //    Wall.Add(new Vector2(StartX, StartY));
            //    StartY -= 5;
                
            //}

        }
        public void Update(GameTime gt)
        {
            staticObjects.Update(gt);
            foregroundObjects.Update(gt);

            if (vineDestroyed == true)
            {
                staticObjects.startAnimation(0, 1, false);
            }
            if (pillarDestroyed == true)
            {
                staticObjects.startAnimation(2, 1, false);
            }
            //TODO ==> Set to trigger

           
            staticObjects.startAnimation(0, 2, false);
            staticObjects.startAnimation(1, 2, false); 
            foregroundObjects.startAnimation(0, 1, true);
           
        }

        public void Draw(SpriteBatch spritebatch, Camera camera)
        {
            staticObjects.Draw(camera, spritebatch);
            splitter.Draw(camera);
            foregroundObjects.Draw(camera, spritebatch);

        }
    }
}
