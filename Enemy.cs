using Jitter.Collision.Shapes;
using Jitter.Dynamics;
using Jitter.LinearMath;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class Enemy : Creature
    {

        private int movCoolDown = 5000;
        private int currentMovCoolDown;

        private float direction;
        private float accRotation;

        public Enemy(string modelName, Vector3 pos, ProjectGame game, double attackPower, double maxHP, double maxMoveDistance)
            : base(modelName, pos, game, attackPower, maxHP, maxMoveDistance)
        {
            rigidBody = new RigidBody(new SphereShape(1f));
            rigidBody.position = ProjectGame.toJVector(pos);
            rigidBody.Tag = modelName;
            this.rigidBody.Material.KineticFriction = 20.0f;
            this.rigidBody.Mass = 5f;
            this.game.world.AddBody(rigidBody);
            currentMovCoolDown = movCoolDown;
            Random rnd = new Random();
            direction = rnd.Next(30, 100);
            accRotation = 0;
            //rigidBody.IsActive = false;
            this.MoveDirection = new Vector3(1, 0, 1);
        }

        public override void Update(SharpDX.Toolkit.GameTime gametime)
        {
            currentMovCoolDown -= gametime.ElapsedGameTime.Milliseconds;
            //game.drawString("rotate", new Vector2(5, 5));
            if (currentMovCoolDown < 0)
            {


                rotate();
                if (accRotation > direction)
                {
                    accRotation = 0;
                    currentMovCoolDown = 5000;
                    Random rnd = new Random();
                    direction = rnd.Next(30, 100);
                    move();


                }
                //this.rigidBody.Position = ProjectGame.toJVector(new Vector3(20.3f, 0.49f, 3.7f));
            }


            //base.Update(gametime);

        }

        public void rotate()
        {

            //System.Diagnostics.Debug.WriteLine("1");
            //this.rigidBody.Orientation = this.rigidBody.Orientation * ProjectGame.toJMatrix(Matrix.RotationY((float)DegreeToRadian(1)));

            accRotation++;
        }

        public void move()
        {
            Random rnd = new Random();
            //float force = /*rnd.Next(20, 30);*/10;
            float angle = new Random().NextFloat(0, (float)(2 * Math.PI));

            ///Vector3.Transform(moveDirection, ProjectGame.toMatrix(this.rigidBody.Orientation));
            game.drawString(moveDirection.ToString(), new Vector2(100, 100));

            this.rigidBody.ApplyImpulse(ProjectGame.toJVector((Vector3)Vector3.Transform(new Vector3(0, 0, 20), Matrix.RotationY(angle))));

            //this.rigidBody.AddForce(ProjectGame.toJVector(moveDirection) * force);
            //this.rigidBody.Position = new JVector();

            //this.rigidBody.ApplyImpulse(ProjectGame.toJVector(moveDirection)*force);
            //this.rigidBody.LinearVelocity = ProjectGame.toJVector(Vector3.Multiply(moveDirection, new Vector3(1, 0, 1))) * 2.0f;

        }


        public override void Draw(GameTime gametime)
        {
            BoundingSphere boundary = model.CalculateBounds();
            Matrix x = new Matrix();
            x.Column1 = new Vector4(Rotation.M11, 0, Rotation.M13, 0);
            x.Column2 = new Vector4(0, 1, 0, 0);
            x.Column3 = new Vector4(Rotation.M31, 0, Rotation.M33, 0);
            x.Column4 = new Vector4(0, 0, 0, 1);

            Vector3 v = new Vector3();
            v.X = Position.X;
            v.Y = 0.5f;
            v.Z = Position.Z;

            Matrix world = Matrix.Translation(-boundary.Center.X, -boundary.Center.Y, -boundary.Center.Z) * Rotation * Matrix.Translation(Position);
            game.effect.Parameters["World"].SetValue(world);
            game.effect.Parameters["View"].SetValue(game.Camera.View);
            game.effect.Parameters["Projection"].SetValue(game.Camera.Projection);
            game.effect.Parameters["cameraPos"].SetValue(game.Camera.Position);
            game.effect.Parameters["worldInvTrp"].SetValue(Matrix.Invert(world));
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
            //base.Draw(gametime);

        }
    }
}
