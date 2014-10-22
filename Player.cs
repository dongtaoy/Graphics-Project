using Jitter.Dynamics;
using SharpDX;
using SharpDX.Toolkit.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Toolkit;
using Jitter.Collision.Shapes;
namespace Project
{
    public class Player : Creature
    {
        
        public Weapon currentWeapon;
        public int weaponIndex;
        public Player(string modelName, Vector3 pos, ProjectGame game, double attackPower, double maxHP, double maxMoveDistance)
            : base(modelName, pos, game, attackPower, maxHP, maxMoveDistance)
        {
            rigidBody = new RigidBody(new SphereShape(1f));
            rigidBody.position = ProjectGame.toJVector(pos);
            rigidBody.Tag = modelName;
            rigidBody.Mass = 7f;
            rigidBody.Material.KineticFriction = 10f;
            rigidBody.IsActive = false;
            game.world.AddBody(rigidBody);

            weapons = new List<Weapon>();
            weapons.Add(new Missile(this, game));
            weapons.Add(new RPG(this, game));
            weaponIndex = 0;
            currentWeapon = weapons[weaponIndex];
        }

        public void jump()
        {
            if (game.world.CollisionSystem.CheckBoundingBoxes(rigidBody, game.Landscape.RigidBody))
            {
                rigidBody.ApplyImpulse(ProjectGame.toJVector(new Vector3(0, 50, 0)));
            }
        }

        public void switchWeapon()
        {
            if (weaponIndex < weapons.Count-1)
            {
                weaponIndex++;
            }
            else
            {
                weaponIndex = 0;
            }
            currentWeapon = weapons[weaponIndex];
        }

        public void shoot(int pressedTime){
            if (game.Camera.firstPerson)
            {
                currentWeapon.shoot(pressedTime);
            }
        }

        public override void Update(SharpDX.Toolkit.GameTime gametime)
        {
            base.Update(gametime);
            //game.mainPage.setTxtScore(currentWeapon.CurrentShootCD.ToString());
            //game.mainPage.txtScore.txt = "1";
        }

        public override void Draw(GameTime gametime)
        {
            //
            //float max = currentWeapon.MaxShootForce*100;
            //float min = currentWeapon.MinShootForce*100;
            //float force = (game.gameInput.SpaceKeyDownTime % (max - min) + min)/100;
            //game.drawString(force.ToString(), new Vector2(150, 150));
            //game.drawString(rigidBody.Tag.ToString() + " X: " + rigidBody.Position.X, new Vector2(50, 50));
            //game.drawString(rigidBody.Tag.ToString() + " Z: " + rigidBody.Position.Z, new Vector2(50, 70));

            //game.drawString(rigidBody.Tag.ToString() + " Y: " + rigidBody.Position.Y, new Vector2(50, 90));
            if (!game.Camera.firstPerson)
            {
                base.Draw(gametime);
            }
        }
        //public async static Task<Player> Load(string modelName, Vector3 pos, ProjectGame game, double attackPower, double maxHP, double maxMoveDistance)
        //{
        //    RigidBody body = await ModelLoader.loadRigidBody(modelName);
        //    return new Player(modelName, pos, game, attackPower, maxHP, maxMoveDistance, body);
        //}
    }
}
