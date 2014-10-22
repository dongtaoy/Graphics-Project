using SharpDX;
using SharpDX.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct2D1;


namespace Project
{
    public class Camera : GameObject
    {
        private Matrix view;
        private Vector3 position;
        private Vector3 lookAt;
        private Vector3 rotation;
        private Matrix projection;
        //private MouseState prevMouseState;
        private Vector3 rotationBuffer;
        private Vector3 moveDirection;
        private DrawableGameObject unitFollow;
        public bool firstPerson = false;
        private Vector3 thirdPerosonReference = new Vector3(0, 0, -20);
        private Vector3 firstPerosonReference = new Vector3(0, 0, -1);
        public DrawableGameObject UnitFollow { set { unitFollow = value; } get { return unitFollow; } }
        public Vector3 MoveDirection { set { moveDirection = value; } }
        public Matrix View
        {
            get
            {
                if (unitFollow != null)
                    return Matrix.LookAtRH(position, unitFollow.Position, Vector3.UnitY);
                return Matrix.LookAtRH(position, lookAt, Vector3.UnitY);
            }
            set
            {
                view = value;
            }
        }
        public Matrix Projection
        {
            get { return projection; }
            set
            {
                projection = value;
            }
        }
        public Matrix World { get { return Matrix.Identity; } }

        public Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;
                UpdateLookAt();
            }
        }

        public Vector3 RotationBuffer
        {
            set
            {
                rotationBuffer = value;
            }
        }

        public Vector3 Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                UpdateLookAt();
            }
        }

        public Camera(ProjectGame game, Vector3 position, Vector3 rotation)
            : base(game)
        {
            this.position = position;
            this.rotation = rotation;
            this.lookAt = Vector3.Zero;
            this.projection = Matrix.PerspectiveFovRH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, 10000.0f);

        }

        private void Move(Vector3 displacement)
        {
            Matrix rotate = Matrix.RotationX(rotation.X) * Matrix.RotationY(rotation.Y);
            displacement = (Vector3)Vector3.Transform(displacement, rotate);
            Position += displacement;
        }
        private void UpdateLookAt()
        {
            Matrix r = Matrix.RotationX(rotation.X) * Matrix.RotationY(rotation.Y);
            Vector3 offset = (Vector3)Vector3.Transform(Vector3.UnitZ, r);

            lookAt = Position + offset;
        }

        public override void Update(GameTime gametime)
        {
            float time = (float)gametime.ElapsedGameTime.Milliseconds;
            //System.Diagnostics.Debug.WriteLine(position.ToString());
            if (!moveDirection.Equals(Vector3.Zero) && !firstPerson)
            {
                moveDirection.Normalize();
                Vector3 displacement = moveDirection * 0.35f * time;
                Move(displacement);
            }

            if (!rotationBuffer.Equals(Vector3.Zero) && !firstPerson)
            {
                //System.Diagnostics.Debug.WriteLine(rotationBuffer.ToString());
                Rotation += rotationBuffer * time * 0.0005f;
                if (Rotation.X > 1) { Rotation = new Vector3(1, Rotation.Y, Rotation.Z); }
                if (Rotation.X < 0.1) { Rotation = new Vector3(0.1f, Rotation.Y, Rotation.Z); }
            }
            if (!rotationBuffer.Equals(Vector3.Zero) && firstPerson)
            {
                //System.Diagnostics.Debug.WriteLine(rotationBuffer.ToString());
                Rotation += rotationBuffer * time * 0.0015f;
                if (Rotation.X > 0.25) { Rotation = new Vector3(0.25f, Rotation.Y, Rotation.Z); }
                if (Rotation.X < -1) { Rotation = new Vector3(-1f, Rotation.Y, Rotation.Z); }
            }
            followUnit();
        }


        private void followUnit()
        {
            //Rotation = unitFollow.Rotation;
            Matrix rotate = Matrix.RotationX(rotation.X) * Matrix.RotationY(rotation.Y);
            Vector3 displacement;
            if (firstPerson)
            {
                displacement = (Vector3)Vector3.Transform(firstPerosonReference, rotate);
            }
            else
            {
                displacement = (Vector3)Vector3.Transform(thirdPerosonReference, rotate);
            }
            Position = unitFollow.Position + displacement;
            //View = Matrix.LookAtRH(cameraPosition, unitFollow.Position, Vector3.UnitY);
        }
        public void DrawCross()
        {
            //Device d2dDevice = new SharpDX.Direct2D1.Device();
        }

        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

    }
}
