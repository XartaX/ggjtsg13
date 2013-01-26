using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Particle
{
    class ParticleStat
    {
        public Vector3 BoxLocation { get; set; }
        public Texture2D Texture { get; set; }       // The texture that will be drawn to represent the particle
        public Vector3 Position { get; set; }        // The current position of the particle        
        public Vector3 Velocity { get; set; }        // The speed of the particle at the current instance
        public float Size { get; set; }              // The size of the particle
        public int TTL { get; set; }                 // The 'time to live' of the particle
        Color color;
        public ParticleStat(Texture2D texture, Vector3 position, Vector3 velocity,
            Color color2, float size, int ttl, int Shei, int Swid)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            Size = size;
            TTL = ttl;
            color = color2;
            iWidth = Swid;
            iHeight = Shei;
        }
        int iWidth;
        int iHeight;
        public bool IsSmoke = false;
        public bool IsRed = false;
        public bool p1 = false, p2 = false;
        public Vector3 EmitterLocation = Vector3.Zero;
        Vector3 SaveVelocity;
        public void Update()
        {

            SaveVelocity = Velocity; //Push
            TTL--;
            
            if (Position.X >= iWidth)
            {
                Position = new Vector3(0.0f, Position.Y, 0.0f);
            }
            else if (Position.X <= 0)
            {
                Position = new Vector3((float)iWidth, Position.Y, 0.0f);
            }
            if (Position.Y >= iHeight)
            {
                Position = new Vector3(Position.X, 0.0f, 0.0f);
            }
            else if (Position.Y <= 0)
            {
                Position = new Vector3(Position.X, (float)iHeight, 0.0f);
            }
            Vector3 a = new Vector3(Velocity.X,Velocity.Y,Velocity.Z);
            Vector3.Multiply(ref a, 2/2, out a);
            



            if (Position.X >= (BoxLocation.X - 48) && Position.X <= BoxLocation.X && Position.Y >= (BoxLocation.Y - 48) && Position.Y <= BoxLocation.Y)
            {
                float Movement = Position.X - BoxLocation.X + 48;

                if (Movement <= 24)
                    Position = new Vector3(BoxLocation.X, Position.Y, Position.Z);
                else if (Movement > 24)
                    Position = new Vector3(BoxLocation.X - 48, Position.Y, Position.Z);

                Position += new Vector3(0.0f, a.Y, a.Z);
            }
            else
            {
                Position += a;
                
                if (TTL == 150)
                {
                    IsRed = true;
                    //color = new Color(TTL, TTL, TTL, 1);
                }
                if (TTL == 50)
                {
                    IsSmoke = true;
                    IsRed = false;
                    //color = new Color(TTL, TTL, TTL, 1);
                }
                if (IsSmoke)
                {
                    if (!p2)
                    {
                        Texture = textures[2];
                        p2 = true;
                    }
                    Size += 0.3f;
                    int gamble = random.Next(1, 10);
                    
                    if (gamble == 2)
                        TTL += 5;
                    else if (gamble == 3)
                        TTL += 9;
                    else if (gamble == 4 || gamble == 5)
                        TTL += 1;
                    else if (gamble == 9)
                        TTL = 1;
                }
                else if (IsRed && Size <= 5)
                {
                    if (!p1)
                    {
                        Texture = textures[1];
                        p1 = true;
                    }
                    Size += 0.1f;
                }
            }
            Velocity = new Vector3(SaveVelocity.X, SaveVelocity.Y - (float)0.01, 0.0f); //Pop

        }
        public List<Texture2D> textures;
        Random random = new Random();
        public SpriteBatch spriteBatch;
        public void Draw()
        {

            spriteBatch.Draw(Texture, new Vector2(Position.X, Position.Y), new Rectangle(0, 0, 1, 1), color,
                0, new Vector2(1.0f, 1.0f), Size, SpriteEffects.None, 0f);
        }
    }
}
