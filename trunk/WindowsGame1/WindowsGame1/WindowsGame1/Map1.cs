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

        public void Initialize(Texture2D Black, ContentManager Content, int ScreenWidth, int ScreenHeight)
        {
            this.Black = Black;
            int StartX=50;
            int StartY=600;
            Ground = new List<Vector2>();
            Wall = new List<Vector2>();

            //vineWall = Content.Load<Texture2D>("vine-wall_204x320");
            splitter.Initialize(Content, "Foreground_tutorial/TutorialMap-foreground__", ScreenWidth, 0, 16);
            staticObjects.addObject(Content, "Elements/interactive/sheet/object__01", 1240, 285, ScreenWidth, ScreenHeight);

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

        public void Draw(SpriteBatch spritebatch, Camera camera)
        {
            splitter.Draw(camera);
            staticObjects.Draw(camera, spritebatch);
        }
    }
}
