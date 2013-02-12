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
        
        public PlayState playState = PlayState.story;
        int mouseX;
        int mouseY;
        public int FireEmitterWCX;
        public int FireEmitterWCY;
        Vector2 FireEmitterWC;
        //Backgrounds splitter = new Backgrounds();

        CollisionDetection Collision = new CollisionDetection();
        CapCameraPosition CapCamera = new CapCameraPosition();
        Input UserInput = new Input();
        //Menu buttons
        Texture2D play;
    
        //Sprite Text
        SpriteFont spriteFont;

        //General Game Objects
        SpriteBatch spriteBatch;
        Texture2D vineWall;
        Vector2 vineWallVect;
        private int worldWidth;
        private int worldHeight;

        //Fullscreen Textures
        List<Texture2D> StoryBoard = new List<Texture2D>();
        Texture2D GameOver;

        //Charracter
        public ToTo MainCharracter;
        Texture2D[] MainCharracterTextureActive;
        Texture2D[] MainCharracterTextureBase,MainCharracterTextureWater,MainCharracterTextureLeaf, MainCharracterTextureFire;

        //Used in UpdateInput
        float speed = 5.0f;
        bool shoot = false;
        bool ShootInProgress = false;

        public int shootTime;


        public bool once = false, CharMoveR = true, CharMoveL = true;

        public Map1 map;
        TimeSpan AirTime;
        bool IsGravity;
        bool IsJumping;
        Keys Movement;
        int GravityValue;
        Camera camera;
        bool[] CrashDirection;
        System.Drawing.Color[,] Collisionmap;
        public System.Drawing.Bitmap Collisionbmp;

        //Particle land
        public ParticleSquirter particleEngine,FireParticle;
        public List<Texture2D> WaterParticleTexture = new List<Texture2D>();
        public List<Texture2D> FireParticleTexture = new List<Texture2D>();
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

        bool btnpress = false;
        bool progressBoard = false;
        bool ShootExplode = false;
        int ExplodeCount = 0;
        int storyCount = 0;
        bool WasRightLastDirection = true;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            IsFixedTimeStep = false;//Ulåst fps
            Content.RootDirectory = "Content";
            InitGraphicsMode();
        }

        private void InitGraphicsMode()
        {
            //GRAPHICS
            DisplayMode dm = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            ScreenHeight = dm.Height;
            ScreenWidth = dm.Width;
            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.PreferMultiSampling = true;
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
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Creates the Collision map, uses color collision
            Collisionmap    = new System.Drawing.Color[5000,5000];
            DirectoryInfo DI= new DirectoryInfo(Environment.CurrentDirectory);
            string bmpPath  = Path.GetFullPath("TutorialMap_CollideZone.jpg");
            Collisionbmp    = new System.Drawing.Bitmap(bmpPath);
                        
            //MainCharracter Textures
            //Base Skill
            MainCharracterTextureBase = new Texture2D[]{Content.Load<Texture2D>("spritesheets/Base_Walk_Right"),Content.Load<Texture2D>("spritesheets/Base_Walk_Left")};
            /*
             //NOT IMPLEMENTED YET
            //Leaf Skill
            MainCharracterTextureWater = new Texture2D[]{Content.Load<Texture2D>("spritesheets/Water_Walk_Right"), Content.Load<Texture2D>("spritesheets/Water_Walk_Left")}
            //Leaf Skill
            MainCharracterTextureLeaf = new Texture2D[]{Content.Load<Texture2D>("spritesheets/Leaf_Walk_Right"), Content.Load<Texture2D>("spritesheets/Leaf_Walk_Left")}
             */
            //Fire Skill
            MainCharracterTextureFire = new Texture2D[]{Content.Load<Texture2D>("spritesheets/Flame_Walk_Right"),Content.Load<Texture2D>("spritesheets/Flame_Walk_Left")};
            MainCharracterTextureActive = MainCharracterTextureFire;
            MainCharracter.Initialize(MainCharracterTextureFire[0], new Vector2(150, 375), 200, 200, ScreenWidth, ScreenHeight, 4, 150, Microsoft.Xna.Framework.Color.White, true, 100);

            //Particle
            WaterParticleTexture.Add(Content.Load<Texture2D>("Elements/Particles/Water/DarkBlue"));
            WaterParticleTexture.Add(Content.Load<Texture2D>("Elements/Particles/Water/Blue"));
            WaterParticleTexture.Add(Content.Load<Texture2D>("Elements/Particles/Water/LightBlue"));
            WaterParticleTexture.Add(Content.Load<Texture2D>("Solids/Square"));
            FireParticleTexture.Add(Content.Load<Texture2D>("Elements/Particles/Fire/Core"));
            FireParticleTexture.Add(Content.Load<Texture2D>("Elements/Particles/Fire/Smoke"));
            FireParticleTexture.Add(Content.Load<Texture2D>("Elements/Particles/Fire/RedFire"));
            FireParticleTexture.Add(Content.Load<Texture2D>("Solids/Square"));

            //Particle Engine
            particleEngine = new ParticleSquirter(WaterParticleTexture, new Vector3((float)random.Next(500), (float)random.Next(500), (float)random.Next(500)));
            FireParticle = new ParticleSquirter(FireParticleTexture, new Vector3((float)random.Next(500), (float)random.Next(500), (float)random.Next(500)));
            FireParticle.EmitterLocation = new Vector3(MainCharracter.Position.X + SpriteMoover, MainCharracter.Position.Y, 0);
            FireParticle.frameRate = frameRate;
            FireParticle.Shei = ScreenHeight;
            FireParticle.Swid = ScreenWidth;
            FireParticle.emitFlag = true;

            //Map Textures
            GameOver = Content.Load<Texture2D>("storyboard/gameOver_2000x1500");
            map.Initialize(Content, ScreenWidth, ScreenHeight);

            //Add storyboard content
            for(int a = 1; a < 18;a++)
            {
                StoryBoard.Add(Content.Load<Texture2D>("storyboard/intro/intro_"+a.ToString().PadLeft(2,'0')));
            }

            //Sets up the camera
            camera = new Camera(spriteBatch);
            //Put camera in middle of world
            camera.Position = new Vector2(0, 0);
            //Sets the world size
            worldHeight = 5000;
            worldWidth = 5000;

            //Menu buttons
            play = Content.Load<Texture2D>("Solids/Square");

            spriteFont = Content.Load<SpriteFont>("Fonts/TestFont");
            /*
            SoundEmber = Content.Load<SoundEffect>("Sound/Ember");
            soundEffectInstance = SoundEmber.CreateInstance();*/
            //graphics.IsFullScreen = bFullScreen;
            //graphics.ApplyChanges();
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
        ///
        float lastCamY = 0;
        protected override void Update(GameTime gameTime)
        {
            //Gets the key stat which is used in the update method
            KeyboardState key = Keyboard.GetState();
            // Allows the game to exit
            if (key.IsKeyDown(Keys.Escape))
            {
                particleEngine.exit = true;
                FireParticle.exit = true;
                this.Exit();
            }
            //Sets the particles spawn location
            Console.WriteLine("X: " + FireEmitterWCX +"   Y: " + FireEmitterWCY);
            FireEmitterWCX = (int)(FireParticle.EmitterLocation.X + camera.Position.X);
            FireEmitterWCY = (int)(FireParticle.EmitterLocation.Y + camera.Position.Y);
            //CollisionDetection.cs
            Collision.CheckCollision
                (
                ref gameTime,
                ref Collisionbmp,
                ref FireEmitterWCX,
                ref FireEmitterWCY,
                ref shootTime,
                ref map,
                ref MainCharracter,
                ref camera,
                ref CharMoveR,
                ref CharMoveL,
                ref playState,
                ref MainCharracterTextureActive,
                ref CrashDirection,
                ref Godmode,
                ref GravityValue,
                ref IsGravity,
                ref IsJumping
                );
            //Cut collision1 --------------------------------------------------
            //End cut--------------
            //CapCameraPosition.cs
            CapCamera.CapCameraPosition
                (
                   ref camera,
                   ref worldWidth,
                   ref worldHeight,
                   graphics.GraphicsDevice.Viewport.Width,
                   graphics.GraphicsDevice.Viewport.Height
                );
            map.Update(gameTime);
            UserInput.UpdateInput
                (
                    ref gameTime,
                    ref mouseX,
                    ref mouseY,
                    ref playState,
                    ref  CrashDirection,
                    ref WasRightLastDirection,
                    ref CharMoveR,
                    ref camera,
                    ref ScreenWidth,
                    ref MainCharracter,
                    ref speed,
                    ref lastCamY,
                    ref IsGravity,
                    ref SpriteMoover,
                    ref ShootInProgress,
                    ref CharMoveL,
                    ref IsJumping,
                    ref AirTime,
                    ref GravityValue,
                    ref FireParticle,
                    ref particleEngine,
                    ref shoot,
                    ref shootTime,
                    ref ShootExplode,
                    ref ExplodeCount,
                    ref SpritePosi,
                    ref ScreenHeight,
                    ref frameRate,
                    ref progressBoard,
                    ref btnpress,
                    ref StoryBoard,
                    ref storyCount,
                    ref MainCharracterTextureActive
                );
            
            elapsedTime += gameTime.ElapsedGameTime;

            //FPS counter
            //if (elapsedTime > TimeSpan.FromSeconds(1))
            if (gameTime.TotalGameTime - elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime = gameTime.TotalGameTime;
                //elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
            //SoundUpdate();
            // TODO: Add your update illogic here
            //Cut Collision2-------------------------
            //End Collision2---------------------
            Gravity(IsGravity);
            base.Update(gameTime);
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

            switch (playState)
            {
                case PlayState.gameOver:
                GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);
                spriteBatch.Draw(GameOver, new Microsoft.Xna.Framework.Rectangle(0, 0, ScreenWidth, ScreenHeight), Color.White);
                particleEngine.Draw(spriteBatch);
                break;

                case PlayState.story:
                GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);
                spriteBatch.Draw(StoryBoard[storyCount], new Microsoft.Xna.Framework.Rectangle(0, 0, ScreenWidth, ScreenHeight), Color.White);
                particleEngine.Draw(spriteBatch);
                break;

                case PlayState.playing:
                particleEngine.Draw(spriteBatch);
                GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.CornflowerBlue);
                MainCharracter.Draw(spriteBatch);
                map.Draw(spriteBatch, camera);
                FireParticle.Draw(spriteBatch);
                // TODO: Add your drawing code here
                //spriteBatch.Draw(MainCharracterTextureActive, t//xVect, Microsoft.Xna.Framework.Color.White);
                //spriteBatch.Draw(foreground, new Vector2(2, 2), Microsoft.Xna.Framework.Color.White);
                break;
            }
            /*
            spriteBatch.DrawString(spriteFont,
                 "FPS: " + frameRate,
                new Vector2(pst10Xangle, ScreenHeight - pst10Yangle),
               Microsoft.Xna.Framework.Color.OrangeRed);*/
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

