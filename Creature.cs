using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jitter.Dynamics;

namespace Project
{
    public abstract class Creature : DrawableGameObject
    {
        protected double attackPower;
        protected double maxHP;
        protected double currentHP;
        protected double maxMoveDistance;
        protected double currentMoveDistance;
        protected List<Weapon> weapons;

        public List<Weapon> Weapons {
            get { return weapons; }
            set { weapons = value; }
        }
        public double MaxHP
        {
            get { return maxHP; }
            set { maxHP = value; }
        }
        public double CurrentHP
        {
            get { return currentHP; }
            set { currentHP = value; }
        }
        public Creature(string modelName, Vector3 pos, ProjectGame game, double attackPower, double maxHP, double maxMoveDistance)
            : base(modelName, pos, game)
        {
            this.attackPower = attackPower;
            this.maxHP = maxHP;
            this.currentHP = maxHP;
            this.maxMoveDistance = maxMoveDistance;
            this.currentMoveDistance = 0;
        }

        public override void Update(GameTime gametime)
        {
            base.Update(gametime);
            
            //System.Diagnostics.Debug.WriteLine("dwoqidjqoi");
            
            Vector3 x = (Vector3)Vector3.Transform(moveDirection, Matrix.RotationY(game.Camera.Rotation.Y));
            
            rigidBody.ApplyImpulse(ProjectGame.toJVector(x));
            
        }

        public override void Draw(GameTime gametime)
        {
            base.Draw(gametime);
            //BoundingSphere boundary = model.CalculateBounds();
            //Matrix world = Matrix.RotationY((float)Math.PI) * Matrix.Translation(-boundary.Center.X, -boundary.Center.Y, -boundary.Center.Z);
            //model.Draw(game.GraphicsDevice, world, game.Camera.View, game.Camera.Projection);
        }
        
    }
}
