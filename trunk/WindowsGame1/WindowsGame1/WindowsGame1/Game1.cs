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
using WindowsGame1.Particle;

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        Random random = new Random();
        GraphicsDeviceManager graphics;

        //Sprite Text
        SpriteFont spriteFont;

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

        //Particle land
        ParticleSquirter particleEngine;
        List<Texture2D> ParticleTex = new List<Texture2D>();
        //Sound Engine
        SoundEffectInstance soundEffectInstance;
        //Sounds
        SoundEffect SoundEmber;
        //Location of Emitter
        Vector3 SpritePosi = Vector3.Zero;

        bool bFullScreen = false;
        public int ScreenHeight, ScreenWidth;
        int frameRate = 0, frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            InitGraphicsMode();
        }

        private void InitGraphicsMode()
        {
            DisplayMode dm = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            ScreenHeight = dm.Height;
            ScreenWidth = dm.Width;
            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.IsFullScreen = bFullScreen;
            graphics.ApplyChanges();
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

            //Particle
            ParticleTex.Add(Content.Load<Texture2D>("Elements/Particles/Water/DarkBlue"));
            ParticleTex.Add(Content.Load<Texture2D>("Elements/Particles/Water/Blue"));
            ParticleTex.Add(Content.Load<Texture2D>("Elements/Particles/Water/LightBlue"));
            ParticleTex.Add(Content.Load<Texture2D>("Solids/Square"));

            particleEngine = new ParticleSquirter(ParticleTex, new Vector3((float)random.Next(500), (float)random.Next(500), (float)random.Next(500)));
            /*
            spriteFont = Content.Load<SpriteFont>("Fonts/TestFont");
            SoundEmber = Content.Load<SoundEffect>("Sound/Ember");
            soundEffectInstance = SoundEmber.CreateInstance();*/
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
            UpdateInput();
            
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
        protected void UpdateInput()
        {
            KeyboardState key = Keyboard.GetState();

            if (key.IsKeyDown(Keys.Escape))
            { this.Exit(); }
            IsGravity = true;
            if (key.IsKeyDown(Keys.Right))
            {
                Movement = Direction.Right;

            }
            else if (key.IsKeyDown(Keys.Left))
            {
                Movement = Direction.Left;

            }
            else if (key.IsKeyDown(Keys.Up))
            {
                Movement = Direction.Up;

                //bool
            }
            else
            {
                Movement = Direction.None;

            }

            // Allows the game to exit
            if (key.IsKeyDown(Keys.Escape))
            {
                particleEngine.exit = true;
                this.Exit();
            }

            if (key.IsKeyDown(Keys.Space))
            {
                particleEngine.emitFlag = true;
            }
            else
            {
                particleEngine.emitFlag = false;
            }
                float SpeedEmiter = 5, SpeedBox = 10;
                float x1 = 0, x2 = 0, y1 = 0, y2 = 0;
                if (key.IsKeyDown(Keys.W))
                    y1 -= SpeedEmiter;
                if (key.IsKeyDown(Keys.S))
                    y1 += SpeedEmiter;
                if (key.IsKeyDown(Keys.Up))
                    y2 -= SpeedBox;
                if (key.IsKeyDown(Keys.Down))
                    y2 += SpeedBox;
                if (key.IsKeyDown(Keys.D))
                    x1 += SpeedEmiter;
                if (key.IsKeyDown(Keys.A))
                    x1 -= SpeedEmiter;
                if (key.IsKeyDown(Keys.Right))
                    x2 += SpeedBox;
                if (key.IsKeyDown(Keys.Left))
                    x2 -= SpeedBox;
                SpritePosi += new Vector3(x1, y1, 0.0f);
            
            if (SpritePosi.X >= ScreenWidth)
            {
                SpritePosi = new Vector3(0.0f, SpritePosi.Y, 0.0f);
            }
            else if (SpritePosi.X <= 0)
            {
                SpritePosi = new Vector3((float)ScreenWidth, SpritePosi.Y, 0.0f);
            }
            if (SpritePosi.Y >= ScreenHeight)
            {
                SpritePosi = new Vector3(SpritePosi.X, 0.0f, 0.0f);
            }
            else if (SpritePosi.Y <= 0)
            {
                SpritePosi = new Vector3(SpritePosi.X, (float)ScreenHeight, 0.0f);
            }
            particleEngine.EmitterLocation = SpritePosi;
            particleEngine.frameRate = frameRate;
            particleEngine.Update();
            particleEngine.Shei = ScreenHeight;
            particleEngine.Swid = ScreenWidth;
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

            particleEngine.Draw(spriteBatch);
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
