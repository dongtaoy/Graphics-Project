using Jitter.Collision.Shapes;
using Jitter.Dynamics;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class RPG : Weapon
    {
        public RPG(Creature shooter, ProjectGame game) : base(shooter, game)
        {
            this.projectileModelName = "Weapon/Orange";
            this.minShootForce = 20f;
            this.maxShootForce = 80f;
            this.impactRadius = 3f;
            this.impactForce = 3f;
            this.shootCD = 2000;
            this.currentShootCD = shootCD;
        }

        public override void shoot(int timePressed)
        {
            if (currentShootCD < 0)
            {
                float force = timePressed / 50 % (maxShootForce - minShootForce) + minShootForce;
                RigidBody rigidBody = new RigidBody(new SphereShape(0.2f));
                
                var shootPosition = shooter.Position + new Vector3(0,1,1);
                var shootDir = Matrix.RotationY(game.Camera.Rotation.Y) * Matrix.RotationX(game.Camera.Rotation.X);
                var shootDirForce = (Vector3)Vector3.Transform(new Vector3(0,0,force),shootDir);
                Projectile projectile = new Projectile(projectileModelName, rigidBody, shootDirForce, shootPosition, 2f, impactRadius, impactForce, game);
                currentShootCD = shootCD;
            }
        }
    }
}
