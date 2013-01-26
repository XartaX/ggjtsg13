using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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

            for (int i = 0; i < positions.Length; i++)
            {
                texture[i] = content.Load<Texture2D>(texturePath+(i+1));
            }

            this.speed = speed;

            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = new Vector2(i * texture[i].Width, 0);
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

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                spriteBatch.Draw(texture[i], positions[i], Color.White);
            }


        }


    }
}
