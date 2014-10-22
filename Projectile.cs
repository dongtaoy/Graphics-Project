using Jitter.Dynamics;
using SharpDX;
using SharpDX.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class Projectile : DrawableGameObject
    {
        float impactForce;
        float impactRadius;

        public Projectile(string modelName, RigidBody rigidBody, Vector3 shootForce, Vector3 shootPosition, float mass, float impactRadius, float impactForce, ProjectGame game)
            : base(modelName, shootPosition, game)
        {
            this.impactForce = impactForce;
            this.impactRadius = impactRadius;
            this.rigidBody = rigidBody;
            this.rigidBody.Mass = mass;
            this.rigidBody.position = ProjectGame.toJVector(shootPosition);
            this.game.world.AddBody(rigidBody);
            this.rigidBody.AngularVelocity = new Jitter.LinearMath.JVector(10, 10, 10);
            this.rigidBody.ApplyImpulse(ProjectGame.toJVector(shootForce));

        }
        public override void Update(GameTime gametime)
        {
            if (game.world.CollisionSystem.CheckBoundingBoxes(rigidBody, game.Landscape.RigidBody))
            {
                foreach(RigidBody obj in game.world.RigidBodies){
                    var dir = rigidBody.Position - obj.Position;
                    if (dir.Length() < impactRadius)
                    {
                        if (!obj.isStatic)
                        {
                            System.Diagnostics.Debug.WriteLine(obj.Tag);
                            var force = (1 / dir.Length() * impactForce) > impactForce ? impactForce : (1 / dir.Length() * impactForce);
                            obj.ApplyImpulse(dir * force);
                        }
                    }
                }
                this.game.world.RemoveBody(rigidBody);
                this.game.Remove(this);
            }

            if (rigidBody.Position.Y < -10)
            {
                this.game.world.RemoveBody(rigidBody);
                this.game.Remove(this);
            }
        }

        public override void Draw(GameTime gametime)
        {
            BoundingSphere boundary = model.CalculateBounds();
            Matrix world = Matrix.Translation(-boundary.Center.X, -boundary.Center.Y, -boundary.Center.Z) * Rotation * Matrix.Translation(Position);
            game.effect.Parameters["World"].SetValue(world);
            game.effect.Parameters["View"].SetValue(game.Camera.View);
            game.effect.Parameters["Projection"].SetValue(game.Camera.Projection);
            game.effect.Parameters["cameraPos"].SetValue(game.Camera.Position);
            game.effect.Parameters["worldInvTrp"].SetValue(Matrix.Invert(world));
            model.Draw(game.GraphicsDevice, world, game.Camera.View, game.Camera.Projection);
        }
    }
}
