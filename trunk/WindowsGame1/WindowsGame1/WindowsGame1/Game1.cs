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
        enum state {playing, pause, menu, gameOver};
        state playState = state.menu;
        int mouseX;
        int mouseY;
        Backgrounds splitter = new Backgrounds();

        //Menu buttons
        Texture2D play;
       
        //Sprite Text
        SpriteFont spriteFont;

        SpriteBatch spriteBatch;
        Texture2D foreground;
        Texture2D txture;
        Texture2D Black;
        Boolean IsJumping;
        Animation txtAnim;
        Map1 map;
        bool IsGravity;
        Keys Movement;
        int GravityValue;
        Camera camera;
        System.Drawing.Color[] Collisionmap;
        System.Drawing.Bitmap Collisionbmp;

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
            graphics.PreferMultiSampling = true;

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
            IsJumping = false;
            txtAnim = new Animation();
            map = new Map1();
            Movement = Keys.None;
            GravityValue = 5;
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        Texture2D knappImg;
        private int worldWidth;
        private int worldHeight;
        private int maxSpriteNum;
        private List<SceneNode> nodeList;
        protected override void LoadContent()
        {
            maxSpriteNum = 100;
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Collisionmap = new System.Drawing.Color[5000 * 5000];
            DirectoryInfo DI = new DirectoryInfo(Environment.CurrentDirectory);
            string bmpPath = DI.FullName.Remove(DI.FullName.Length - 27) + "/WindowsGame1Content/backgrounds/TutorialMap_CollideZone.jpg";
            Collisionbmp = new System.Drawing.Bitmap(bmpPath);
            Collisionmap[1] = Collisionbmp.GetPixel(0, 0);

            txture = Content.Load<Texture2D>("spritesheets/base_Walk_200x200px");
            Black = Content.Load<Texture2D>("black");
            txtAnim.Initialize(txture, new Vector2(150, 375), 200, 200, ScreenWidth, ScreenHeight, 4, 150, Microsoft.Xna.Framework.Color.White, true, 100);
            map.Initialize(Black);
            // TODO: use this.Content to load your game content here
            knappImg = Content.Load<Texture2D>("Solids/Square");
            //Particle
            ParticleTex.Add(Content.Load<Texture2D>("Elements/Particles/Water/DarkBlue"));
            ParticleTex.Add(Content.Load<Texture2D>("Elements/Particles/Water/Blue"));
            ParticleTex.Add(Content.Load<Texture2D>("Elements/Particles/Water/LightBlue"));
            ParticleTex.Add(Content.Load<Texture2D>("Solids/Square"));

            camera = new Camera(spriteBatch);
            nodeList = new List<SceneNode>();
            worldHeight = 5000;
            worldWidth = 5000;
            Random randNums = new Random();
            for (int i = 0; i < maxSpriteNum; i++)
            {
                SceneNode node = new SceneNode(knappImg, new Vector2(randNums.Next(worldWidth), randNums.Next(worldHeight)));
                nodeList.Add(node);
            }
            // put camera in middle of world
            camera.Position = new Vector2(0, 0);

            //Load terrain
            splitter.Initialize(Content, "Foreground_tutorial/TutorialMap-foreground__", ScreenWidth, 0, 16);
            //foreground = Content.Load<Texture2D>("backgrounds/TutorialMap-Foreground");

            //Menu buttons
            play = Content.Load<Texture2D>("Elements/Particles/Water/DarkBlue");

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
        ParticleSquirter p1, p2, p3, p4;
        private void runFaun()
        {
            p1 = new ParticleSquirter(ParticleTex, new Vector3(200, 300, 0));
            p1.Shei = ScreenHeight;
            p1.Swid = ScreenWidth;
            p1.emitFlag = true;

        }
        private void runFaun2()
        {
            p2 = new ParticleSquirter(ParticleTex, new Vector3(400, 500, 0));
            p2.Shei = ScreenHeight;
            p2.Swid = ScreenWidth;
            p2.emitFlag = true;

        }
        private void runFaun3()
        {
            p3 = new ParticleSquirter(ParticleTex, new Vector3(600, 450, 0));
            p3.Shei = ScreenHeight;
            p3.Swid = ScreenWidth;
            p3.emitFlag = true;

        }
        private void runFaun4()
        {
            p4 = new ParticleSquirter(ParticleTex, new Vector3(900, 300, 0));
            p4.Shei = ScreenHeight;
            p4.Swid = ScreenWidth;
            p4.emitFlag = true;

        }
        bool once = false;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {            
            // move camera with keyboard
            MoveCamera();
            // cap the camera to the world width/height.
            CapCameraPosition();

            if (p1 != null)
                p1.Update();
            if (p2 != null)
                p2.Update();
            if (p3 != null)
                p3.Update();
            if (p4 != null)
                p4.Update();
            if (!once)
            {
                Thread th = new Thread(new ThreadStart(runFaun));
                th.Start();
                Thread th2 = new Thread(new ThreadStart(runFaun2));
                th2.Start();
                Thread th3 = new Thread(new ThreadStart(runFaun3));
                th3.Start();
                Thread th4 = new Thread(new ThreadStart(runFaun4));
                th4.Start();
                once = true;
            }
            UpdateInput(gameTime);
            
            // TODO: Add your update logic here

            //Collision detection
            Microsoft.Xna.Framework.Rectangle CharRect = new Microsoft.Xna.Framework.Rectangle((int)txtAnim.Position.X, (int)txtAnim.Position.Y, txture.Width/4, txture.Height);
            bool Collision = false;

            if(CollisionDetection(CharRect.X,CharRect.Y))//Upper left
            {


            }
            if (CollisionDetection(CharRect.X+(CharRect.Width/2), CharRect.Y))//Upper Mid
            {


            }
            if (CollisionDetection(CharRect.X + CharRect.Width, CharRect.Y))//Upper right
            {
                //Keyboard.
            }
            if (CollisionDetection(CharRect.X, CharRect.Y+(CharRect.Height/2)))//middle left
            {

            }
            if (CollisionDetection(CharRect.X + CharRect.Width, CharRect.Y + (CharRect.Height / 2)))//middle right
            {


            }
            if (CollisionDetection(CharRect.X, CharRect.Y + CharRect.Height))//bottom left
            {



            }
            if (CollisionDetection(CharRect.X + (CharRect.Width / 2), CharRect.Y + CharRect.Height))//Bottom mid
            {

            }
            if (CollisionDetection(CharRect.X + CharRect.Width, CharRect.Y + CharRect.Height))//bottom right
            {


            }

            Gravity(IsGravity);


            base.Update(gameTime);
        }
        private bool CollisionDetection(int PosX, int PosY)
        {
            if (Collisionmap[PosX + (PosY * 5000)].R == 0 &&
                Collisionmap[PosX + (PosY * 5000)].G == 0 &&
                Collisionmap[PosX + (PosY * 5000)].B == 0
                )
            {
                return true;
            }
            else
            {
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
        private void MoveCamera()
        {
            // make keyboard move the camera
            KeyboardState ks = Keyboard.GetState();
            Keys[] keys = ks.GetPressedKeys();
            foreach (Keys key in keys)
            {
                switch (key)
                {
                    case Keys.W:
                        camera.Translate(new Vector2(0, -speed));
                        break;
                    case Keys.D:
                        camera.Translate(new Vector2(speed, 0));
                        break;
                    case Keys.S:
                        camera.Translate(new Vector2(0, speed));
                        break;
                    case Keys.A:
                        camera.Translate(new Vector2(-speed, 0));
                        break;
                }
            }
        }

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

            IsGravity = true;

            if (key.IsKeyDown(Keys.Right))
            {
                txtAnim.Position.Y += 1;
                txtAnim.Update(gameTime, true);
            }
            else if (key.IsKeyDown(Keys.Left))
            {
                txtAnim.Position.X -= 1;
                txtAnim.Update(gameTime, true);
            }
            else if (key.IsKeyDown(Keys.Up))
            {
                IsGravity = false;
                txtAnim.Position.Y -= 2.5f;
                txtAnim.Update(gameTime, false);
            }
            else
                txtAnim.Update(gameTime, false);

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
                txtAnim.Position.Y += GravityValue;//Gravity
            }
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone);
                       
            //spriteBatch.Draw(txture, t//xVect, Microsoft.Xna.Framework.Color.White);
            if (playState == state.menu)
            {
                GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);
                spriteBatch.Draw(play, new Microsoft.Xna.Framework.Rectangle(900, 500, 100, 100), Microsoft.Xna.Framework.Color.White);
                spriteBatch.Draw(play, new Microsoft.Xna.Framework.Rectangle(mouseX, mouseY, 20, 20), Microsoft.Xna.Framework.Color.White);
            }

            if (playState == state.playing)
            {
                GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Gray);
                
                particleEngine.Draw(spriteBatch);
                p1.Draw(spriteBatch);
                p2.Draw(spriteBatch);
                p3.Draw(spriteBatch);
                p4.Draw(spriteBatch);
                
                GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.CornflowerBlue);
                particleEngine.Draw(spriteBatch);
                // TODO: Add your drawing code here
                //spriteBatch.Draw(txture, t//xVect, Microsoft.Xna.Framework.Color.White);
                //txtAnim.Draw(camera);
                //spriteBatch.Draw(foreground, new Vector2(2, 2), Microsoft.Xna.Framework.Color.White);

                txtAnim.Draw(spriteBatch);
                map.Draw(spriteBatch);
           
                splitter.Draw(camera);
            }

                foreach (SceneNode node in nodeList)
                {
                    camera.DrawNode(node);
                }
                spriteBatch.End();
                base.Draw(gameTime);    
        }
            
        }
        
        //private Camera camera;
    }

