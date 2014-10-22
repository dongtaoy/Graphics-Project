using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Sensors;
using Windows.UI.Xaml;
using Windows.UI.Input;
using Windows.UI.Core;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit;
using SharpDX;


namespace Project
{
    public class GameInput : GameObject
    {
        private Accelerometer accelerometer;
        private CoreWindow window;
        private GestureRecognizer gestureRecognizer;
        private KeyboardManager keyboardManager;
        private MouseManager mouseManager;
        

        private DrawableGameObject inputObject;
        private int spaceKeyDownTime;
        public int SpaceKeyDownTime
        {
            get { return spaceKeyDownTime; }
        }

        public DrawableGameObject InputObject
        {
            set
            {
                inputObject = value;
            }
        }

        public GameInput(ProjectGame game) : base(game)
        {
            // Get the accelerometer object
            accelerometer = Accelerometer.GetDefault();
            window = Window.Current.CoreWindow;

            // Set up the gesture recognizer.  In this example, it only responds to TranslateX, Scale and Tap events
            gestureRecognizer = new Windows.UI.Input.GestureRecognizer();
            gestureRecognizer.GestureSettings = GestureSettings.ManipulationTranslateX | GestureSettings.ManipulationScale | GestureSettings.Tap;
            
            // Set up mouseManager and keyboardManager
            mouseManager = new MouseManager(game);
            keyboardManager = new KeyboardManager(game);

            // Register event handlers for pointer events
            window.PointerPressed += OnPointerPressed;
            window.PointerMoved += OnPointerMoved;
            window.PointerReleased += OnPointerReleased;
        }

        public override void Update(GameTime gameTime)
        {
            // get Mouse and Keyboard State
            KeyboardState keyboardState = keyboardManager.GetState();
            MouseState mouseState = mouseManager.GetState();

            Vector3 direction = new Vector3();
            if (keyboardState.IsKeyPressed(Keys.F))
            {
                game.Camera.firstPerson = !game.Camera.firstPerson;
            }

            if (keyboardState.IsKeyPressed(Keys.G))
            {
                game.Player.RigidBody.IsActive = true;
            }

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                direction.Z = 1;
                //System.Diagnostics.Debug.WriteLine("aa");
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                direction.Z = -1;
                //System.Diagnostics.Debug.WriteLine("aa");
            }
            
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                direction.X = 1;
                //System.Diagnostics.Debug.WriteLine("aa");
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                direction.X = -1;
            }

            inputObject.MoveDirection = direction;
            //game.Camera.MoveDirection = direction;
            
            // Rotation Input
            Vector3 rotationBuffer = new Vector3();

            if (keyboardState.IsKeyDown(Keys.W))
            {
                rotationBuffer.X--;
            }

            if (keyboardState.IsKeyDown(Keys.S))
            {
                rotationBuffer.X++;
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                rotationBuffer.Y++;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                rotationBuffer.Y--;
            }
            game.Camera.RotationBuffer = rotationBuffer;


            if (keyboardState.IsKeyDown(Keys.Space))
            {
                spaceKeyDownTime += gameTime.ElapsedGameTime.Milliseconds;
                game.mainPage.setTxtScore(game.Player.currentWeapon.getForce(spaceKeyDownTime).ToString());
            }

            if (keyboardState.IsKeyReleased(Keys.Space))
            {
                game.Player.shoot(spaceKeyDownTime);
                spaceKeyDownTime = 0;
            }


            if (keyboardState.IsKeyDown(Keys.Q))
            {
                game.Player.jump();
                //System.Diagnostics.Debug.WriteLine("aa");
            }

            if (keyboardState.IsKeyPressed(Keys.Tab))
            {
                game.Player.switchWeapon();
                //System.Diagnostics.Debug.WriteLine("aa");
            }
            //if (prevMouseState != null && !mouseState.Equals(prevMouseState))
            //{
            //    System.Diagnostics.Debug.WriteLine("11");
            //    rotationBuffer.X = mouseState.Y - 0.5f;
            //    rotationBuffer.Y = mouseState.X - 0.5f;
            
            //}

            //mouseManager.SetPosition(new Vector2(0.5f, 0.5f));
            
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                game.Exit();
                game.Dispose();
                App.Current.Exit();
            }

        }

        // Call the gesture recognizer when a pointer event occurs
        void OnPointerPressed(CoreWindow sender, PointerEventArgs args)
        {
            gestureRecognizer.ProcessDownEvent(args.CurrentPoint);
        }

        void OnPointerMoved(CoreWindow sender, PointerEventArgs args)
        {
            gestureRecognizer.ProcessMoveEvents(args.GetIntermediatePoints());
        }

        void OnPointerReleased(CoreWindow sender, PointerEventArgs args)
        {
            gestureRecognizer.ProcessUpEvent(args.CurrentPoint);
        }
    }
}