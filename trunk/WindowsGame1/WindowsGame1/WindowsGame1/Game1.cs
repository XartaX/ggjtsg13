using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D txture;
        Texture2D Black;
        Vector2 txVect;
        Boolean IsJumping;
        Animation txtAnim;
        Map1 map;
        bool IsGravity;
        enum Direction { Left, Right,Up, None };
        Direction Movement;
        int GravityValue;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            txVect = new Vector2(150, 70);
            IsJumping = false;
            txtAnim = new Animation();
            map = new Map1();
            Movement = Direction.None;
            GravityValue = 5;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            txture = Content.Load<Texture2D>("texture");
            Black = Content.Load<Texture2D>("black");
            txtAnim.Initialize(txture, txVect, 100, 100, 4, 500, Color.White, true);
            map.Initialize(Black);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            { this.Exit(); }
            IsGravity=true;
            if(Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                Movement = Direction.Right;
                
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                Movement = Direction.Left;
                
            }
            else if(Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                Movement = Direction.Up;
                
                //bool
            }
            else
            {
                Movement = Direction.None;
               
            }

            
            // TODO: Add your update logic here

            Rectangle CharRect = new Rectangle((int)txVect.X, (int)txVect.Y, txture.Width/4, txture.Height);

            
            for (int i = 0; i < map.Wall.Count(); i++)
            {
                Rectangle MapRect = new Rectangle((int)map.Wall[i].X, (int)map.Wall[i].Y, Black.Width, Black.Height);
                if (CharRect.Intersects(MapRect))
                {
                    if (MapRect.X> CharRect.X&&Movement == Direction.Right)
                    {
                        Movement = Direction.None;

                    }
                    if (MapRect.X < CharRect.X && Movement == Direction.Left)
                    {
                        Movement = Direction.None;

                    }
                    
                }

            }
            for (int i = 0; i < map.Ground.Count(); i++)
            {
                Rectangle MapRect = new Rectangle((int)map.Ground[i].X, (int)map.Ground[i].Y, Black.Width, Black.Height);
                if (CharRect.Intersects(MapRect))
                {
                    if ((CharRect.Y + CharRect.Height) > MapRect.Y)
                    {
                        txVect.Y = MapRect.Y - txture.Height;
                        if ((CharRect.Y + CharRect.Height + (GravityValue * 2) < MapRect.Y + MapRect.Height) && (Movement == Direction.Right))
                        {
                            Console.WriteLine("HammerTime!" + gameTime.TotalGameTime.Seconds);
                            Movement = Direction.None;
                        }
                        IsGravity = false;


                    }
                    else if ((CharRect.Y + CharRect.Height - (GravityValue) > MapRect.Y) && (Movement == Direction.Right))
                    {
                        Console.WriteLine("puoajøheøhush!" + gameTime.TotalGameTime.Seconds);
                        Movement = Direction.None;
                        IsGravity = true;
                        //  txVect.X--;
                    }

                    else
                    {
                        IsGravity = true;
                    }


                    if ((CharRect.X < MapRect.X) && (CharRect.Y > MapRect.Y))
                    {

                        //Movement = Direction.None;
                    }
                }
            }
            switch (Movement)
            {
                case Direction.Left:
                    txVect.X--;
                    txtAnim.Position = txVect;
                    txtAnim.Update(gameTime);
                    break;
                case Direction.Right:
                    txVect.X++;
                    txtAnim.Position = txVect;
                    txtAnim.Update(gameTime);
                    break;
                case Direction.None:
                    txtAnim.Position = txVect;
                    
                    txtAnim.UpdateNotChangeFrame(gameTime);
                    break;
                case Direction.Up:
                    txVect.Y -= 2.5f;
                    IsGravity = false;
                    txtAnim.Position = txVect;
                    
                    txtAnim.UpdateNotChangeFrame(gameTime);
                    break;

            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                txVect.Y -= 2.5f;
                IsGravity = false;
                //bool
            }
            else if (txVect.Y < 400)
            {
               
            }
            Gravity(IsGravity);


            base.Update(gameTime);
        }

        public void Gravity(bool fall)
        {
            if (fall)
            {
                txVect.Y += GravityValue;//Gravity
            }
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            //spriteBatch.Draw(txture, t//xVect, Color.White);
            txtAnim.Draw(spriteBatch);
            map.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
