using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1.Particle
{
    class ParticleSquirter
    {

        Random random = new Random();

        public Vector3 EmitterLocation { get; set; }
        public Vector3 BoxLocation { get; set; }

        List<ParticleStat> particles;
        List<Texture2D> textures;

        public bool emitFlag = false, exit = false;
        bool runonce = true;

        public int frameRate = 0, pcount = 0, Shei, Swid;
        int total = 0;
        float correcter = 0.0f, mover = 1.5f;

        public ParticleSquirter(List<Texture2D> Textures, Vector3 location)
        {
            correcter = mover / 2;
            EmitterLocation = location;
            textures = Textures;
            particles = new List<ParticleStat>();
        }
        private void runParticleMovement()
        {
            if (total < 10000 & emitFlag)
            {
                total += 500;
            }

            int SafeIndex = -1;
            for (int particle = 0; particle < particles.Count; particle++)
            {
                if (particles[particle] == null)
                {
                    if (SafeIndex != -1)
                        particles[particle] = particles[SafeIndex];
                }
                else
                {
                    if (particles[particle].TTL <= 0)
                    {
                        particles.RemoveAt(particle);
                        particle--;
                        total--;
                        pcount--;
                    }
                    else
                    {
                        if (SafeIndex == -1)
                        {
                            SafeIndex = particle;
                        }
                        particles[particle].BoxLocation = BoxLocation;
                        particles[particle].textures = textures;
                        particles[particle].Update();
                    }
                }
            }
        }
        public void Update()
        {
            while (total > particles.Count && emitFlag)
            {
                GenerateNewParticle();
            }
            runonce = false;
            runParticleMovement();
        }
        private void GenerateNewParticle()
        {
            /* velocity begynner i 4 kvadrant, så trekkes fra 1 
             * for å skyve den opp til 2.kvadrant, og ganges med 2 for 
             * å doble størrelsen så den fyller ut 1 3 og 4 kvadrant.
             */

            Vector3 velocity = new Vector3(
                ((float)random.NextDouble() * mover) - mover / 2,
                ((float)random.NextDouble() * mover) - mover / 2,
                ((float)random.NextDouble() * mover) - mover / 2
                );

            int ttl = random.Next(25);
            Vector3.Divide(ref velocity, 1, out velocity);
            pcount++;
            particles.Add(new ParticleStat(textures[0], EmitterLocation, velocity, new Color(125, 125, 125, 1), 1.0f, ttl, Shei, Swid));
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }


            spriteBatch.Draw(textures[3], new Vector2(EmitterLocation.X, EmitterLocation.Y), new Rectangle(0, 0, 1, 1), Color.Transparent,
                0, new Vector2(1.0f, 1.0f), 5.0f, SpriteEffects.None, 0f);



        }
    }
}