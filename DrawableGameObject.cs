using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jitter.Dynamics;
using Jitter.Collision.Shapes;
namespace Project
{
    public abstract class DrawableGameObject : GameObject
    {
        protected Model model;
        protected Vector3 moveDirection;
        protected Vector3 rotationBuffer;
        protected float mass;
        protected RigidBody rigidBody;

        public Vector3 MoveDirection {
            get { return moveDirection; }
            set{    moveDirection = value;}

        }

        public DrawableGameObject()
        {

        }



        public DrawableGameObject(string modelName, Vector3 position, ProjectGame game)
        {
            this.GameObjectType = GameObjectType.DRAWABLE;
            this.model = ModelLoader.loadModel(modelName, game);
            this.ProjectGame = game;
            this.ProjectGame.Add(this);
            //BasicEffect.EnableDefaultLighting(model, true);
        }

        public RigidBody RigidBody
        {
            get
            {
                return rigidBody;
            }
        }

        public Vector3 Position
        {
            get
            {
                return ProjectGame.toVector3(rigidBody.Position);
            }
        }

        public Matrix Rotation
        {
            get
            {
                return ProjectGame.toMatrix(rigidBody.Orientation);
            }
        }


        public override void Update(GameTime gametime)
        {

        }

        public virtual void Draw(GameTime gametime)
        {

            BoundingSphere boundary = model.CalculateBounds();
            Matrix world = Matrix.Translation(-boundary.Center.X, -boundary.Center.Y, -boundary.Center.Z) * Matrix.Translation(Position);
            game.effect.Parameters["World"].SetValue(world);
            game.effect.Parameters["View"].SetValue(game.Camera.View);
            game.effect.Parameters["Projection"].SetValue(game.Camera.Projection);
            game.effect.Parameters["cameraPos"].SetValue(game.Camera.Position);
            game.effect.Parameters["worldInvTrp"].SetValue(Matrix.Invert(world));


            //model.Draw(game.GraphicsDevice, world, game.Camera.View, game.Camera.Projection);
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    //ModelSkinnedBone bone  part.SkinnedBones
                    if (part.Material.HasProperty(TextureKeys.DiffuseTexture))
                    {
                        Texture y = part.Material.GetProperty(TextureKeys.DiffuseTexture).ElementAt(0).Texture;
                        game.effect.Parameters["g_MeshTexture"].SetResource<Texture>(y);

                        foreach (EffectPass pass in game.effect.CurrentTechnique.Passes)
                        {
                            pass.Apply();
                        }
                        try
                        {
                            part.Draw(game.GraphicsDevice);
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                    }
                }
            }
        }
        //private void Move(Vector3 displacement)
        //{
        //    rotation.Y = game.Camera.Rotation.Y;
        //    Matrix rotate = Matrix.RotationY(rotation.Y);
        //    displacement = (Vector3)Vector3.Transform(displacement, rotate);
        //    Position += displacement;
        //}


        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }
    }
}
