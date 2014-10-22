using Jitter.Dynamics;
using SharpDX;
using SharpDX.Toolkit.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class BonusBox:DrawableGameObject
    {
        private BonusType bonusType;
        public BonusBox(string modelName, Vector3 pos, ProjectGame game, BonusType bonusType): base(modelName, pos, game)
        {
            this.bonusType = bonusType;

        }

        public override void Update(SharpDX.Toolkit.GameTime gametime)
        {
            if (game.world.CollisionSystem.CheckBoundingBoxes(this.rigidBody, game.Player.RigidBody))
            {
                affect();
                game.Remove(this);
            }
        }

        private void affect()
        {
            //if (bonusType.Equals(BonusType.Missile))
            //{
            //    //game.Player.Weapons.Add(new Missile("ItemBox", new Vector3(0, 0, 0), game));       
            //}
            //if (bonusType.Equals(BonusType.Health))
            //{
            //    game.Player.CurrentHP = game.Player.MaxHP;
            //}
            //if (bonusType.Equals(BonusType.Gravity))
            //{
            //    game.world.Gravity = new Jitter.LinearMath.JVector(0,3,0);
            //}

        }
        

        
    }
}
