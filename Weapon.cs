using Jitter.Dynamics;
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
    public abstract class Weapon : GameObject
    {
        protected string projectileModelName;
        protected Creature shooter;
        protected float minShootForce;
        protected float maxShootForce;
        protected float impactRadius;
        protected float impactForce;
        protected int shootCD;
        protected int currentShootCD;

        public int CurrentShootCD
        {
            get { return currentShootCD; }
        }
        public float MinShootForce
        {
            get { return minShootForce; }
        }
        public float MaxShootForce
        {
            get { return maxShootForce; }
        }
        public int ShootCD
        {
            get { return shootCD; }
            set { shootCD = value; }
        }
        public float ImpactForce
        {
            get
            {
                return impactForce;
            }
        }

        public Weapon(Creature shooter, ProjectGame game)
            : base(game)
        {
            this.shooter = shooter;
        }

        public abstract void shoot(int timePressed);

        public override void Update(GameTime gametime)
        {
            currentShootCD -= gametime.ElapsedGameTime.Milliseconds;

        }
        public float getForce(int timePressed)
        {
            return timePressed / 50 % (maxShootForce  - minShootForce ) + minShootForce;
        }
    }
}
