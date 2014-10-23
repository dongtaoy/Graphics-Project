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
    using SharpDX.Toolkit.Graphics;
    class Galaxy : DrawableGameObject
    {
        

        public Galaxy(string modelName, Vector3 position, ProjectGame game):base(modelName,position,game)
        {
                   
        }

        public override void Update(GameTime gametime)
        {
          
        }

        public override void Draw(GameTime gametime)
        {            
            BoundingSphere boundary = model.CalculateBounds();
            Matrix world = Matrix.Translation(-boundary.Center.X, -boundary.Center.Y, -boundary.Center.Z);
            model.Draw(game.GraphicsDevice, Matrix.Identity, game.Camera.View, game.Camera.Projection);
            
        }
    }
}
