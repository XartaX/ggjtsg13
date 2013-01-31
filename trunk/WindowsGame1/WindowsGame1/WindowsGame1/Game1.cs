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
        enum state { playing, pause, menu, gameOver, story };
        state playState = state.story;
        int mouseX;
        int mouseY;
        int FireEmitterWCX;
        int FireEmitterWCY;
        Vector2 FireEmitterWC;
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
        TimeSpan AirTime;
        bool IsGravity;
        bool IsJumping;
        Keys Movement;
        int GravityValue;
        Camera camera;
        bool[] CrashDirection;
        System.Drawing.Color[,] Collisionmap;
        System.Drawing.Bitmap Collisionbmp;

        //Particle land
        ParticleSquirter particleEngine,FireParticle;
        List<Texture2D> WaterParticleTexture = new List<Texture2D>();
        List<Texture2D> FireParticleTexture = new List<Texture2D>();
        //Sound Engine
        SoundEffectInstance soundEffectInstance;
        //Sounds
        SoundEffect SoundEmber;
        //Location of Emitter
        Vector3 SpritePosi = Vector3.Zero;

        //SETTINGS
        bool bFullScreen =true;
        bool Godmode = false;
        public int ScreenHeight, ScreenWidth;
        int frameRate = 0, frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;

        float pst10Xangle, pst10Yangle;
        int SpriteMoover =100;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            IsFixedTimeStep = false;//Ulåst fps
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
            AirTime = TimeSpan.Zero;
            base.Initialize();
            FireEmitterWC = camera.ApplyTransformations(new Vector2(FireParticle.EmitterLocation.X, FireParticle.EmitterLocation.Y));

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        private int worldWidth;
        private int worldHeight;
        List<Texture2D> StoryBoard = new List<Texture2D>();
        Texture2D txtureRight, txtureLeft, GameOver;
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Collisionmap = new System.Drawing.Color[5000,5000];
            DirectoryInfo DI = new DirectoryInfo(Environment.CurrentDirectory);
            //string bmpPath = DI.FullName.Remove(DI.FullName.Length - 27) + "/WindowsGame1Content/backgrounds/TutorialMap_CollideZone.jpg";
            string bmpPath = Path.GetFullPath("TutorialMap_CollideZone.jpg");//\\"TutorialMap_CollideZone.jpg");
            //bmpPath = bmpPath.Remove(bmpPath.Length - 1) + "TutorialMap_CollideZone.jpg";
            //string a= properti
            Collisionbmp = new System.Drawing.Bitmap(bmpPath);

            Console.WriteLine("Finished");
            
            txtureRight = Content.Load<Texture2D>("spritesheets/base_Walk_200x200px");
            txture = txtureRight;
            GameOver = Content.Load<Texture2D>("storyboard/gameOver_2000x1500");
            txtureLeft = Content.Load<Texture2D>("spritesheets/base_Walk_left");
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

            for(int a = 1; a < 18;a++){
            StoryBoard.Add(Content.Load<Texture2D>("storyboard/intro/intro_"+a.ToString().PadLeft(2,'0')));
            }

            camera = new Camera(spriteBatch);
            worldHeight = 5000;
            worldWidth = 5000;
            // put camera in middle of world
            camera.Position = new Vector2(0, 0);

            //Menu buttons
            play = Content.Load<Texture2D>("Solids/Square");

            particleEngine = new ParticleSquirter(WaterParticleTexture, new Vector3((float)random.Next(500), (float)random.Next(500), (float)random.Next(500)));
            FireParticle = new ParticleSquirter(FireParticleTexture, new Vector3((float)random.Next(500), (float)random.Next(500), (float)random.Next(500)));
            FireParticle.EmitterLocation = new Vector3(MainCharracter.Position.X + SpriteMoover, MainCharracter.Position.Y, 0);
            FireParticle.frameRate = frameRate;
            FireParticle.Shei = ScreenHeight;
            FireParticle.Swid = ScreenWidth;
            FireParticle.emitFlag = true;
            

            spriteFont = Content.Load<SpriteFont>("Fonts/TestFont");
            /*
            SoundEmber = Content.Load<SoundEffect>("Sound/Ember");
            soundEffectInstance = SoundEmber.CreateInstance();*/
            graphics.IsFullScreen = bFullScreen;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        bool once = false, CharMoveR = true, CharMoveL = true;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        ///
        float lastCamY = 0;
        protected override void Update(GameTime gameTime)
        {
            vineWallVect2 = camera.ApplyTransformations(vineWallVect);

            Console.WriteLine("X: " + FireEmitterWCX +"   Y: " + FireEmitterWCY);

            FireEmitterWCX = (int)(FireParticle.EmitterLocation.X + camera.Position.X);
            FireEmitterWCY = (int)(FireParticle.EmitterLocation.Y + camera.Position.Y);

            if (Collisionbmp.GetPixel(FireEmitterWCX, FireEmitterWCY).R == 255)
            {
                shootTime = 0;
            }

            //animation-triggers
            //vine
            if (FireEmitterWCX > 1240 && FireEmitterWCX < 1444 &&
                FireEmitterWCY > 285 && FireEmitterWCY < 605)
            {
                if (map.vineDestroyed == false)
                {   
                    Console.WriteLine("Success!");
                    map.vineDestroyed = true;
                    shootTime = 0;
                }
            }
            //pillar
            if (FireEmitterWCX > 980 && FireEmitterWCX < 1180 &&
                FireEmitterWCY > 1690 && FireEmitterWCY < 2080)
            {
                if (map.pillarDestroyed == false)
                {
                    Console.WriteLine("Success!");
                    map.pillarDestroyed = true;
                    shootTime = 0;
                }
            }
            KeyboardState key = Keyboard.GetState();
            int CharX, CharY,CharOffset = 0;
            if (key.IsKeyDown(Keys.D))
            {
                CharOffset = 175;
            }
            else CharOffset = 0;
            CharX = (int)(MainCharracter.Position.X + camera.Position.X + CharOffset);
            CharY = (int)(MainCharracter.Position.Y + camera.Position.Y + 58);
            //Vine
            if (CharX > 1240 && CharX < 1444 &&
                CharY > 285 && CharY < 605)
            {
                if (!map.vineDestroyed)
                {
                    CharMoveR = false;
                }
            }
            else
            {
                CharMoveR = true;
            }
            //pillar
            if (CharX > 980 && CharX < 1180 &&
                CharY > 1690 && CharY < 2080)
            {
                if (map.pillarDestroyed == false)
                {
                    CharMoveL = false;
                }
            }
            else
            {
                CharMoveL = true;
            }
            //Toxic
            if (CharX > 2800 && CharX < 3650 &&
                CharY > 4370 && CharY < 4650)
            {
                playState = state.gameOver;
            }

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

            // TODO: Add your update illogic here

            //Collision detection
            Microsoft.Xna.Framework.Rectangle CharRect = new Microsoft.Xna.Framework.Rectangle((int)(MainCharracter.Position.X+camera.Position.X), (int)(MainCharracter.Position.Y+camera.Position.Y), txture.Width/4, txture.Height);
            for (int i = 0; i < CrashDirection.Length; i++)
            {
                CrashDirection[i] = false;
            }
            //0     Upwards
            //1     Right
            //2     Downwards
            //3     Left

            if (!Godmode)
            {
                if (CollisionDetection(CharRect.X + 110, CharRect.Y + 85))//Upper left
                {
                    CrashDirection[0] = true;
                    IsGravity = true;
                    GravityValue = 5;
                    IsJumping = false;

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
                    MainCharracter.Position.Y -= 0.5f;
                    MainCharracter.Update(gameTime);
                    CrashDirection[4] = true;
                }
                if (CollisionDetection(CharRect.X + 80, CharRect.Y + 200))//left
                {

                    IsGravity = false;
                    //CrashDirection[3] = true;
                    MainCharracter.Position.Y -= 0.5f;
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
        bool WasRightLastDirection = true;
        private bool CollisionDetection(int PosX, int PosY)
        {
            if (PosX > 0 && PosY > 0 && PosX < Collisionbmp.Width && PosY < Collisionbmp.Height)
                if (Collisionbmp.GetPixel(PosX, PosY).R == 255)//Collision true
                    return true;
                else//Collision false
                    return false;
            else 
                return true;
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
        bool shoot =  false;
        bool ShootInProgress = false;
        int shootTime;
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



            if (key.IsKeyDown(Keys.D) && !CrashDirection[1] && CharMoveR)
            {
                MainCharracter.Spritestrip = txtureRight;
                WasRightLastDirection = true;

                if (camera.Position.X+ScreenWidth > 5000-10)
                {
                    MainCharracter.Position.X += (speed);
                }
                else
                {
                    camera.Translate(new Vector2(speed, 0));
                    MainCharracter.Position.X += 0;
                }
                
                lastCamY = camera.Position.Y;
                MainCharracter.Animate = true;
                MainCharracter.Update(gameTime);
                IsGravity = true;
                if (SpriteMoover >= 0 && !ShootInProgress)
                {
                    SpriteMoover -= 3;
                }
            }
            if (key.IsKeyDown(Keys.A) && !CrashDirection[3] && CharMoveL)
            {
                    MainCharracter.Spritestrip = txtureLeft;
                WasRightLastDirection = false;
                if (camera.Position.X < 10)
                {
                    MainCharracter.Position.X -= (speed);
                }
                else
                {
                    camera.Translate(new Vector2(-speed, 0));
                    MainCharracter.Position.X -= 0;
                }
                lastCamY = camera.Position.Y;   
                MainCharracter.Animate = true;
                MainCharracter.Update(gameTime);
                IsGravity = true;
                if (SpriteMoover <= 200 && !ShootInProgress)
                {
                    SpriteMoover += 3;
                }
            }
            if (key.IsKeyDown(Keys.W) && !CrashDirection[0])
            {
                if (gameTime.TotalGameTime - AirTime > TimeSpan.FromSeconds(1) && !IsJumping && CrashDirection[2])
                {
                    
                        IsJumping = true;
                        
                    AirTime = gameTime.TotalGameTime;
                }
                

                
            }
            if (IsJumping && (gameTime.TotalGameTime - AirTime < TimeSpan.FromSeconds(1)))
            {
                camera.Translate(new Vector2(0, -speed));
                //IsGravity = false;
                GravityValue = 0;
                MainCharracter.Position.Y -= 2.5f;
                MainCharracter.Update(gameTime);
                if (ShootInProgress)
                {
                    FireParticle.EmitterLocation += new Vector3(0, 2.5f, 0);
                }
            }
            else
            {
                IsJumping = false;
                IsGravity = true;
                GravityValue = 5;
            }
            if (key.IsKeyDown(Keys.S) && !CrashDirection[2])
            {
                camera.Translate(new Vector2(0, speed + speed));
                MainCharracter.Position.Y += 2.5f;
                MainCharracter.Update(gameTime);
                if (ShootInProgress)
                {
                    FireParticle.EmitterLocation += new Vector3(0, -2.5f, 0);
                }
            }

            // Allows the game to exit
            if (key.IsKeyDown(Keys.Escape))
            {
                particleEngine.exit = true;
                FireParticle.exit = true;
                this.Exit();
            }
            if (shoot && key.IsKeyUp(Keys.Space))
            {
                shoot = false;
                ShootInProgress = true;
                shootTime = 100;
            }
            if (ShootInProgress)
            {
                shootTime--;
                if(WasRightLastDirection)
                    FireParticle.EmitterLocation += new Vector3(5, 0, 0);
                else
                    FireParticle.EmitterLocation += new Vector3(-5, 0, 0);
                if (shootTime <= 0)
                {
                    ShootInProgress = false;
                    ShootExplode = true;
                    ExplodeCount = 7;
                    FireParticle.exploding = true;
                    FireParticle.Spread = (float)(1 / 5d);
                }
            }
            else if (shootTime == 0 && ShootExplode)
            {
                ExplodeCount--;
                if (ExplodeCount == 0)
                {
                    ShootExplode = false;
                    FireParticle.exploding = false;
                    FireParticle.Spread = 1f;
                }
            }
            else
            {
                ShootExplode = false;
                FireParticle.exploding = false;
                FireParticle.Spread = 1f;
                FireParticle.EmitterLocation = new Vector3(MainCharracter.Position.X + SpriteMoover, MainCharracter.Position.Y, 0);
            }

            if (key.IsKeyDown(Keys.Space) && !shoot && !ShootExplode && !ShootInProgress)
            {
                shoot = true;
                particleEngine.emitFlag = true;
            }
            else if (playState == state.menu)
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
            FireParticle.Update();

            particleEngine.EmitterLocation = new Vector3(mouseX,mouseY,0);
            particleEngine.frameRate = frameRate;
            particleEngine.Update();
            particleEngine.Shei = ScreenHeight;
            particleEngine.Swid = ScreenWidth;
            if (key.IsKeyDown(Keys.Enter) && playState == state.story)
            {
                progressBoard = true;
            }
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                btnpress = true;
            }
            if ((mouse.LeftButton == ButtonState.Released && btnpress) || key.IsKeyUp(Keys.Enter) && progressBoard && playState == state.story)
            {
                btnpress = false;
                if (StoryBoard.Count< storyCount + 2)
                {
                    playState = state.playing;
                }
                else
                {
                    storyCount++;
                    progressBoard = false;
                }

            }
            if (key.IsKeyDown(Keys.F1) && playState == state.story)
            {
                playState = state.playing;
            }
        }
        bool btnpress = false;
        bool progressBoard = false;
        bool ShootExplode = false;
        int ExplodeCount = 0;
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
        int storyCount = 0;
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            frameCounter++;
            spriteBatch.Begin();

            if (playState == state.gameOver)
            {
                GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);
                spriteBatch.Draw(GameOver, new Microsoft.Xna.Framework.Rectangle(0, 0, ScreenWidth, ScreenHeight), Color.White);
                particleEngine.Draw(spriteBatch);
                
            }
            if (playState == state.story)
            {
                GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);
                spriteBatch.Draw(StoryBoard[storyCount], new Microsoft.Xna.Framework.Rectangle(0, 0, ScreenWidth, ScreenHeight), Color.White);
                particleEngine.Draw(spriteBatch);
                
            }

            if (playState == state.playing)
            {
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
                FireParticle.Draw(spriteBatch);
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

