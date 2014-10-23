// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using SharpDX;
using SharpDX.Toolkit;
using System;
using System.Collections.Generic;
using Windows.UI.Input;
using Windows.UI.Core;
using Windows.Devices.Sensors;
using Jitter;
using Jitter.Collision;
using Jitter.Collision.Shapes;
using Jitter.DataStructures;
using Jitter.Dynamics;
using Jitter.LinearMath;
using Matrix = SharpDX.Matrix;
using Vector3 = SharpDX.Vector3;

namespace Project
{
    // Use this namespace here in case we need to use Direct3D11 namespace as well, as this
    // namespace will override the Direct3D11.
    using SharpDX.Toolkit.Graphics;
    using SharpDX.Toolkit.Input;

    public class ProjectGame : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;


        public List<GameObject> gameObjects;
        private Stack<GameObject> addedGameObjects;
        private Stack<GameObject> removedGameObjects;
        public World world;
        public SpriteFont spriteFont;
        public SpriteBatch spriteBatch;
        public AccelerometerReading accelerometerReading;
        public GameInput gameInput;
        public MainPage mainPage;

        public BasicEffect basicEffect;
        private Camera camera;
        private Player player;
        private Landscape landscape;
        //private BoxController bonusBoxController;

        public Effect effect;
        public Effect landEffect;
        //private RigidBody baseRock;
        public bool started = false;

        //public KeyboardManager KeyboardManager { get { return keyboardManager; } }
        public Camera Camera { get { return camera; } }
        public Player Player { get { return player; } }

        public Landscape Landscape { get { return landscape; } }
       
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectGame" /> class.
        /// </summary>
        public ProjectGame(MainPage mainPage)
        {

            // create a physics world
            CollisionSystem collisionSystem = new CollisionSystemPersistentSAP();
            world = new World(collisionSystem);

            // Creates a graphics manager. This is mandatory.
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            
            //graphicsDeviceManager.PreferredGraphicsProfile = new SharpDX.Direct3D.FeatureLevel[] { SharpDX.Direct3D.FeatureLevel.Level_11_1};
            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";


            // Create the keyboard manager
            //keyboardManager = new KeyboardManager(this);

            // Initialise event handling.
            //input.gestureRecognizer.Tapped += Tapped;
            //input.gestureRecognizer.ManipulationStarted += OnManipulationStarted;
            //input.gestureRecognizer.ManipulationUpdated += OnManipulationUpdated;
            //input.gestureRecognizer.ManipulationCompleted += OnManipulationCompleted;

            this.mainPage = mainPage;
        }

        protected override void LoadContent()
        {
            // Initialise game object containers.
            spriteFont = Content.Load<SpriteFont>("Arial16");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            effect = Content.Load<Effect>("Shader/myShader");
            landEffect = Content.Load<Effect>("Shader/shadow");

            gameObjects = new List<GameObject>();
            addedGameObjects = new Stack<GameObject>();
            removedGameObjects = new Stack<GameObject>();
            // Create game objects.
            gameInput = new GameInput(this);
            player = new Player("Player/ball", new Vector3(15, 10, 0), this, 100, 100, 100);
            //player = await Player.Load("Cat", new Vector3(0, 20, 0), this, 100, 100, 100);
            camera = new Camera(this, new Vector3(0, 100, -10), new Vector3(0, 0, 0));
            landscape = new Landscape("Landscape/arab6", new Vector3(0, 0, 0), this);
            //var skybox = new SkyBox("SkyBox/Day_Skybox", new Vector3(0, 0, 0), this);
            var skydome = new Galaxy("SkyBox/Galaxy", new Vector3(0, 0, 0), this);
            world.CollisionSystem.Detect(player.RigidBody, landscape.RigidBody);
            //bonusBox = new BonusBox("weaponbox", new Vector3(0, 10, 0), this, BonusBoxType.Missile);
            //landscape = await Landscape.LoadLandscape("Map", new Vector3(0, 0, 0), this);
            Enemy enemy = new Enemy("Enemy/enemy", new Vector3(10, 15, 0), this, 100, 100, 100);

            //RigidBody test = ModelLoader.loadRigidBody(@"Content/Cat.obj");

            world.SetDampingFactors(0.5f, 0.5f);
            
            //gameObjects.Add(gameInput);
            //gameObjects.Add(camera);
            ////gameObjects.Add(bonusBox);

            gameInput.InputObject = player;
            camera.UnitFollow = player;
            spriteBatch = ToDisposeContent(new SpriteBatch(GraphicsDevice));
            base.LoadContent();
        }

        protected override void Initialize()
        {
            Window.Title = "Project";

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            
            if (started)
            {

                world.Step((float)gameTime.ElapsedGameTime.TotalSeconds, false);
                flushAddedAndRemovedGameObjects();
                //accelerometerReading = input.accelerometer.GetCurrentReading();
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    gameObjects[i].Update(gameTime);
                }


            }
            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            if (started)
            {
                //foreach (RigidBody body in world.RigidBodies)
                //{

                //    System.Diagnostics.Debug.WriteLine(body.Tag + body.Position.ToString());
                //}
                //System.Diagnostics.Debug.WriteLine("");
                // Clears the screen with the Color.CornflowerBlue
                GraphicsDevice.Clear(Color.CornflowerBlue);
                //GraphicsDevice.Clear(ClearOptions.Stencil, Color.Black, 0, 0);
                    

                for (int i = 0; i < gameObjects.Count; i++)
                {
                    if (gameObjects[i].GameObjectType.Equals(GameObjectType.DRAWABLE))
                        ((DrawableGameObject)gameObjects[i]).Draw(gameTime);
                }
            }
            // Handle base.Draw
            base.Draw(gameTime);
        }

        // Add a new game object.
        public void Add(GameObject obj)
        {
            if (!gameObjects.Contains(obj) && !addedGameObjects.Contains(obj))
            {
                addedGameObjects.Push(obj);
            }
        }

        // Remove a game object.
        public void Remove(GameObject obj)
        {
            if (gameObjects.Contains(obj) && !removedGameObjects.Contains(obj))
            {
                removedGameObjects.Push(obj);
            }
        }

        // Process the buffers of game objects that need to be added/removed.
        private void flushAddedAndRemovedGameObjects()
        {
            while (addedGameObjects.Count > 0) { gameObjects.Add(addedGameObjects.Pop()); }
            while (removedGameObjects.Count > 0) { gameObjects.Remove(removedGameObjects.Pop()); }
        }


        public void OnManipulationStarted(GestureRecognizer sender, ManipulationStartedEventArgs args)
        {
            // Pass Manipulation events to the game objects.

        }

        public void Tapped(GestureRecognizer sender, TappedEventArgs args)
        {
            // Pass Manipulation events to the game objects.
            foreach (var obj in gameObjects)
            {
                obj.Tapped(sender, args);
            }
        }

        public void OnManipulationUpdated(GestureRecognizer sender, ManipulationUpdatedEventArgs args)
        {
            // Update camera position for all game objects
            foreach (var obj in gameObjects)
            {
                obj.OnManipulationUpdated(sender, args);
            }
        }

        public void OnManipulationCompleted(GestureRecognizer sender, ManipulationCompletedEventArgs args)
        {

        }


        public static JVector toJVector(Vector3 vec)
        {
            JVector v = new JVector();
            v.X = vec.X;
            v.Y = vec.Y;
            v.Z = vec.Z;
            return v;
        }

        public static Vector3 toVector3(JVector vec)
        {
            return new Vector3(vec.X, vec.Y, vec.Z);
        }


        public static JMatrix toJMatrix(Matrix x)
        {
            JMatrix jmatrix = new JMatrix();
            jmatrix.M11 = x.Column1.X;
            jmatrix.M12 = x.Column1.Y;
            jmatrix.M13 = x.Column1.Z;
            jmatrix.M21 = x.Column2.X;
            jmatrix.M22 = x.Column2.Y;
            jmatrix.M23 = x.Column2.Z;
            jmatrix.M31 = x.Column3.X;
            jmatrix.M32 = x.Column3.Y;
            jmatrix.M33 = x.Column3.Z;
            return jmatrix;
        }

        public static Matrix toMatrix(JMatrix x)
        {
            Matrix matrix = new Matrix();
            matrix.Column1 = new Vector4(x.M11, x.M12, x.M13, 0);
            matrix.Column2 = new Vector4(x.M21, x.M22, x.M23, 0);
            matrix.Column3 = new Vector4(x.M31, x.M32, x.M33, 0);
            matrix.Column4 = new Vector4(0, 0, 0, 1);
            return matrix;
        }
        public void drawString(string str, Vector2 pos)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFont,str, pos, Color.Black);
            spriteBatch.End();
        }

    }
}
