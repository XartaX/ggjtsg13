using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WindowsGame1.View;

namespace WindowsGame1
{
    class Backgrounds
    {
        Texture2D[] texture;

        Vector2[] positions;

        public int speed;


        public void Initialize(ContentManager content, String texturePath, int screenWidth, int speed, int count)
        {

            texture = new Texture2D[count];
            positions = new Vector2[count];

            for (int i = 0; i < count; i++)
            {
                texture[i] = content.Load<Texture2D>(texturePath+(i+1).ToString().PadLeft(2,'0'));
            }

            this.speed = speed;

            int counter = -1, mult = 0;
            for (int i = 0; i < count; i++)
            {
                if (counter == 3)
                {
                    mult++;
                    counter = -1;
                }
                counter++;
                positions[i] = new Vector2(counter * texture[i].Width, mult * texture[i].Height);
                Console.WriteLine(("Y: " + mult * texture[i].Height).PadRight(8,' ') + "X: " + counter * texture[i].Width);
            }



        }

        public void Update()
        {
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i].X += speed;

                if (speed <= 0)
                {
                    if (positions[i].X <= -texture[i].Width)
                    {
                        positions[i].X = texture[i].Width * (positions.Length - 1);

                    }

                }
                else
                {
                    if (positions[i].X >= texture[i].Width * (positions.Length - 1))
                    {
                        positions[i].X = -texture[i].Width;
                    }

                }



            }


        }

        public void Draw(Camera spriteBatch)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                SceneNode node = new SceneNode(texture[i], positions[i]);
                spriteBatch.DrawNode(node);
            }


        }


    }
}
