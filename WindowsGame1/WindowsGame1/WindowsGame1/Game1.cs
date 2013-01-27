using System;
using System.IO;
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
using System.Threading;
using WindowsGame1.View;

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        Random random = new Random();
        GraphicsDeviceManager graphics;
        enum state { playing, pause, menu, gameOver };
        state playState = state.menu;
        int mouseX;
        int mouseY;
        //Backgrounds splitter = new Backgrounds();

        //Menu buttons
        Texture2D play;

        //Sprite Text
        SpriteFont spriteFont;

        SpriteBatch spriteBatch;
        Texture2D txture;
        Texture2D vineWall;
        Texture2D Black;
        Vector2 vineWallVect;
        Vector2 vineWallVect2;
        ToTo MainCharracter;
        Map1 map;
        bool IsGravity = true;
        int GravityValue;
        Camera camera;
        bool[] CrashDirection;
        System.Drawing.Color[,] Collisionmap;
        System.Drawing.Bitmap Collisionbmp;

        //Particle land
        ParticleSquirter particleEngine,BallParticle;
        List<Texture2D> WaterParticleTexture = new List<Texture2D>();
        List<Texture2D> FireParticleTexture = new List<Texture2D>();
        //Sound Engine
        SoundEffectInstance soundEffectInstance;
        //Sounds
        SoundEffect SoundEmber;
        //Location of Emitter
        Vector3 SpritePosi = Vector3.Zero;

        //SETTINGS
        bool bFullScreen =false;
        bool Godmode = true;
        public int ScreenHeight, ScreenWidth;
        int frameRate = 0, frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;

        float pst10Xangle, pst10Yangle;
        int SpriteMoover =100;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            IsFixedTimeStep = false;//Ul�st fps
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
            
            graphics.PreferMultiSampling = true;
            //graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            pst10Xangle = ScreenWidth * 0.1f;
            pst10Yangle = ScreenHeight * 0.1f;
            // TODO: Add your initialization logic here
            vineWallVect = new Vector2(1240, 285);
            MainCharracter = new ToTo();
            map = new Map1();
            GravityValue = 5;
            CrashDirection = new bool[7] { false, false, false, false, false, false, false };
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        private int worldWidth;
        private int worldHeight;
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Collisionmap = new System.Drawing.Color[5000,5000];
            DirectoryInfo DI = new DirectoryInfo(Environment.CurrentDirectory);
            string bmpPath = DI.FullName.Remove(DI.FullName.Length - 27) + "/WindowsGame1Content/backgrounds/TutorialMap_CollideZone.jpg";
            Collisionbmp = new System.Drawing.Bitmap(bmpPath);

            Console.WriteLine("Finished");
            graphics.IsFullScreen = bFullScreen;
            graphics.ApplyChanges();

            txture = Content.Load<Texture2D>("spritesheets/base_Walk_200x200px");
            vineWall = Content.Load<Texture2D>("Elements/interactive/sheet/object__01");
            Black = Content.Load<Texture2D>("black");
            MainCharracter.Initialize(txture, new Vector2(150, 375), 200, 200, ScreenWidth, ScreenHeight, 4, 150, Microsoft.Xna.Framework.Color.White, true, 100);
            //vineWallAnim.Initialize(vineWall, vineWallVect, 204, 320, ScreenWidth, ScreenHeight, 10, 100, Microsoft.Xna.Framework.Color.White, false, 100);
            map.Initialize(Black, Content, ScreenWidth, ScreenHeight);
            // TODO: use this.Content to load your game content here
            //Particle
            WaterParticleTexture.Add(Content.Load<Texture2D>("Elements/Particles/Water/DarkBlue"));
            WaterParticleTexture.Add(Content.Load<Texture2D>("Elements/Particles/Water/Blue"));
            WaterParticleTexture.Add(Content.Load<Texture2D>("Elements/Particles/Water/LightBlue"));
            WaterParticleTexture.Add(Content.Load<Texture2D>("Solids/Square"));
            FireParticleTexture.Add(Content.Load<Texture2D>("Elements/Particles/Fire/Core"));
            FireParticleTexture.Add(Content.Load<Texture2D>("Elements/Particles/Fire/Smoke"));
            FireParticleTexture.Add(Content.Load<Texture2D>("Elements/Particles/Fire/RedFire"));
            FireParticleTexture.Add(Content.Load<Texture2D>("Solids/Square"));


            camera = new Camera(spriteBatch);
            worldHeight = 5000;
            worldWidth = 5000;
            // put camera in middle of world
            camera.Position = new Vector2(0, 0);

            //Menu buttons
            play = Content.Load<Texture2D>("Solids/Square");

            particleEngine = new ParticleSquirter(WaterParticleTexture, new Vector3((float)random.Next(500), (float)random.Next(500), (float)random.Next(500)));
            BallParticle = new ParticleSquirter(FireParticleTexture, new Vector3((float)random.Next(500), (float)random.Next(500), (float)random.Next(500)));
            BallParticle.EmitterLocation = new Vector3(MainCharracter.Position.X + SpriteMoover, MainCharracter.Position.Y, 0);
            BallParticle.frameRate = frameRate;
            BallParticle.Shei = ScreenHeight;
            BallParticle.Swid = ScreenWidth;
            BallParticle.emitFlag = true;

            spriteFont = Content.Load<SpriteFont>("Fonts/TestFont");
            /*
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
        bool once = false;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            vineWallVect2 = camera.ApplyTransformations(vineWallVect);

           

            // cap the camera to the world width/height.
            CapCameraPosition();
            map.Update(gameTime);
            UpdateInput(gameTime);
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
            //SoundUpdate();

            // TODO: Add your update logic here

            //Collision detection
            Microsoft.Xna.Framework.Rectangle CharRect = new Microsoft.Xna.Framework.Rectangle((int)(MainCharracter.Position.X+camera.Position.X), (int)(MainCharracter.Position.Y+camera.Position.Y), txture.Width/4, txture.Height);
            for (int i = 0; i < CrashDirection.Length; i++)
            {
                CrashDirection[i] = false;
            }
            //0     Upwards
            //1     Right
            //2     Downwards
            //3     Letf
            //Console.Clear();
            //Console.WriteLine("Char.X :"+CharRect.X+" Char.Y: "+CharRect.Y);
            //Console.WriteLine("Char.X2 :" + CharRect.X + CharRect.Width+ " Char.Y2: " + CharRect.Y+CharRect.Height);
            //Console.WriteLine("1");

            if (!Godmode)
            {
                if (CollisionDetection(CharRect.X + 110, CharRect.Y + 85))//Upper left
                {
                    CrashDirection[0] = true;

                }
                if (CollisionDetection(CharRect.X + 160, CharRect.Y + 125))//Over
                {
                    CrashDirection[1] = true;

                }
                if (CollisionDetection(CharRect.X + 100, CharRect.Y + 200))//Right
                {
                    CrashDirection[2] = true;

                }
                if (CollisionDetection(CharRect.X + 50, CharRect.Y + 135))//Under
                {
                    CrashDirection[3] = true;

                }
                if (CollisionDetection(CharRect.X + 120, CharRect.Y + 200))//left
                {
                    IsGravity = false;
                    //CrashDirection[3] = true;
                    MainCharracter.Position.Y -= 2.5f;
                    MainCharracter.Update(gameTime);
                    CrashDirection[4] = true;
                }
                if (CollisionDetection(CharRect.X + 80, CharRect.Y + 200))//left
                {

                    IsGravity = false;
                    //CrashDirection[3] = true;
                    MainCharracter.Position.Y -= 2.5f;
                    MainCharracter.Update(gameTime);
                    CrashDirection[4] = true;
                }
            }
            else
            {
                IsGravity = false;
            }


            Gravity(IsGravity);


            base.Update(gameTime);
        }
        private bool CollisionDetection(int PosX, int PosY)
        {
        //    Console.Clear();
        //    Console.WriteLine(Collisionbmp.GetPixel(PosX, PosY).R);
            //if (Collisionmap[PosX ,PosY] == System.Drawing.Color.White)
            if (Collisionbmp.GetPixel(PosX ,PosY).R==255)
            {
                //Console.WriteLine("XXXXXXXXXXXXXXX");
                return true;
            }
            else
            {
                //Console.WriteLine("-------------------");
                return false;
            }
        }
        private void CapCameraPosition()
        {
            Vector2 cameraPosition = camera.Position;
            if (cameraPosition.X > worldWidth - graphics.GraphicsDevice.Viewport.Width)
            {
                cameraPosition.X = worldWidth - graphics.GraphicsDevice.Viewport.Width;
            }
            if (cameraPosition.X < 0)
            {
                cameraPosition.X = 0;
            }
            if (cameraPosition.Y > worldHeight - graphics.GraphicsDevice.Viewport.Height)
            {
                cameraPosition.Y = worldHeight - graphics.GraphicsDevice.Viewport.Height;
            }
            if (cameraPosition.Y < 0)
            {
                cameraPosition.Y = 0;
            }
            camera.Position = cameraPosition;
        }

        float speed = 5.0f;
        protected void UpdateInput(GameTime gameTime)
        {
            KeyboardState key = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            mouseX = mouse.X;
            mouseY = mouse.Y;

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                if (mouse.X > 900 && mouse.X < 1000 && mouse.Y > 500 && mouse.Y < 600)
                {
                    playState = state.playing;
                }
            }

            

            if (key.IsKeyDown(Keys.D) && !CrashDirection[1])
            {
                camera.Translate(new Vector2(speed, 0));
                MainCharracter.Position.X += 1;
                MainCharracter.Update(gameTime);
                IsGravity = true;
                if (SpriteMoover >= 0)
                {
                    SpriteMoover -= 3;
                }
            }
            if (key.IsKeyDown(Keys.A) && !CrashDirection[3])
            {
                camera.Translate(new Vector2(-speed, 0));
                MainCharracter.Position.X -= 1;
                MainCharracter.Animate = true;
                MainCharracter.Update(gameTime);
                IsGravity = true;
                if (SpriteMoover <= 200)
                {
                    SpriteMoover += 3;
                }
            }
            if (key.IsKeyDown(Keys.W) && !CrashDirection[0])
            {
                camera.Translate(new Vector2(0, -speed - speed));
                IsGravity = true;
                MainCharracter.Position.Y -= 2.5f;
                MainCharracter.Update(gameTime);
                
            }
            if (key.IsKeyDown(Keys.S) && !CrashDirection[2])
            {
                camera.Translate(new Vector2(0, speed + speed));
                MainCharracter.Position.Y += 2.5f;
                MainCharracter.Update(gameTime);
            }

            BallParticle.EmitterLocation = new Vector3(MainCharracter.Position.X + SpriteMoover, MainCharracter.Position.Y, 0);
            // Allows the game to exit
            if (key.IsKeyDown(Keys.Escape))
            {
                particleEngine.exit = true;
                BallParticle.exit = true;
                this.Exit();
            }

            if (key.IsKeyDown(Keys.Space) || playState == state.menu)
            {
                particleEngine.emitFlag = true;
            }
            else
            {
                particleEngine.emitFlag = false;
            }
            float SpeedEmiter = 5, x1 = 0, y1 = 0;
            if (key.IsKeyDown(Keys.W))
                y1 -= SpeedEmiter;
            if (key.IsKeyDown(Keys.S))
                y1 += SpeedEmiter;
            if (key.IsKeyDown(Keys.D))
                x1 += SpeedEmiter;
            if (key.IsKeyDown(Keys.A))
                x1 -= SpeedEmiter;
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
            BallParticle.Update();

            particleEngine.EmitterLocation = new Vector3(mouseX,mouseY,0);
            particleEngine.frameRate = frameRate;
            particleEngine.Update();
            particleEngine.Shei = ScreenHeight;
            particleEngine.Swid = ScreenWidth;

        }
        public void Gravity(bool fall)
        {
            if (fall && !CrashDirection[2])
            {
                MainCharracter.Position.Y += GravityValue;
            }
                if (MainCharracter.Position.Y > ScreenHeight / 2+50 && MainCharracter.Position.Y > ScreenHeight / 2 - 50)
                {
                    Console.WriteLine("CharY: " + MainCharracter.Position.Y);
                    camera.Translate(new Vector2(0, GravityValue));
                    
                        MainCharracter.Position.Y -= GravityValue;
                    
                }
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            frameCounter++;
            spriteBatch.Begin();

            if (playState == state.menu)
            {
                GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);
                spriteBatch.Draw(play, new Microsoft.Xna.Framework.Rectangle(900, 500, 100, 100), Microsoft.Xna.Framework.Color.White);
                particleEngine.Draw(spriteBatch);
                
            }

            if (playState == state.playing)
            {
                GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Gray);
                BallParticle.Draw(spriteBatch);
                particleEngine.Draw(spriteBatch);

                GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.CornflowerBlue);
                // TODO: Add your drawing code here
                //spriteBatch.Draw(txture, t//xVect, Microsoft.Xna.Framework.Color.White);
                //MainCharracter.Draw(camera);
                //spriteBatch.Draw(foreground, new Vector2(2, 2), Microsoft.Xna.Framework.Color.White);

                MainCharracter.Draw(spriteBatch);
                //vineWallAnim.Draw(spriteBatch);

                //splitter.Draw(camera);
                map.Draw(spriteBatch, camera);
            }

            spriteBatch.DrawString(spriteFont,
                 "FPS: " + frameRate,
                new Vector2(pst10Xangle, ScreenHeight - pst10Yangle),
               Microsoft.Xna.Framework.Color.OrangeRed);
            spriteBatch.End();
            base.Draw(gameTime);
        }

    }

    //private Camera camera;
}
