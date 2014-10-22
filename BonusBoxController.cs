using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;


namespace Project
{
    class BonusBoxController:GameObject
    {
        protected int dropBoxCD;
        Random r;
        public int DropBoxCD
        {
            set { dropBoxCD = value; }
        }
        public BonusBoxController(ProjectGame game)
            : base(game)
        {
            this.dropBoxCD = 0;
            r = new Random();
        }

        public override void Update(SharpDX.Toolkit.GameTime gameTime)
        {
            dropBoxCD -= gameTime.ElapsedGameTime.Milliseconds;
            if (dropBoxCD <= 0)
            {
                Array bonuses = Enum.GetValues(typeof(BonusType));
                BonusType bonusType = (BonusType)bonuses.GetValue(r.Next(bonuses.Length));
                game.gameObjects.Add(new BonusBox("weaponbox",new Vector3(r.Next(20)-10,40,r.Next(20)-10),game,bonusType));
                dropBoxCD = r.Next(10000) + 30000;
            }
        }

    }
}
