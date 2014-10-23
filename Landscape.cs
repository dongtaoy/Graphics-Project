using Jitter.Dynamics;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jitter.Collision.Shapes;
using Jitter.DataStructures;
using Jitter.LinearMath;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
namespace Project
{
    public class Landscape : DrawableGameObject
    {
        private List<RigidBody> rigidBodies;
        public Matrix lightView;
        public Matrix lightProj;
        public Vector3 lightPos;
        public Matrix lightsViewProjectionMatrix;
        RenderTarget2D _shadowMap;
        DepthStencilBuffer _shadowZBuffer;

        public SharpDX.Direct3D11.Texture2D renderToTexture;
        public RenderTargetView renderToTextureRTV;

        public Landscape(string modelName, Vector3 pos, ProjectGame game)
            : base(modelName, pos, game)
        {

            lightPos = new Vector3(25, 20, 0);
            lightView = Matrix.LookAtRH(lightPos, new Vector3(0, 0, 0), Vector3.UnitY);
            lightProj = Matrix.PerspectiveFovRH((float)(Math.PI / 2), 1f, 5f, 1000f);
            lightsViewProjectionMatrix = lightView * game.Camera.Projection;


            rigidBodies = new List<RigidBody>();

            RigidBody r1 = new RigidBody(new BoxShape(5.667f, 4.964f, 3.078f));
            r1.IsStatic = true;
            r1.Tag = "r1";
            r1.Position = ProjectGame.toJVector(new Vector3(18.5f + 0.2f, 2.45f, -4.52f - 0.4f));
            r1.Orientation = ProjectGame.toJMatrix(Matrix.RotationY(DegreeToRadian(-2.5f)));
            game.world.AddBody(r1);


            RigidBody r2 = new RigidBody(new BoxShape(5.823f, 4.343f, 4.227f));
            r2.IsStatic = true;
            r2.Tag = "r2";
            r2.Position = ProjectGame.toJVector(new Vector3(10.4f + 0.2f, 2.17f, -5.457054f - 0.4f));
            r2.Orientation = ProjectGame.toJMatrix(Matrix.RotationY(DegreeToRadian(0)));
            game.world.AddBody(r2);


            RigidBody r3 = new RigidBody(new BoxShape(5.994f, 5.557f, 2.128f));
            r3.IsStatic = true;
            r3.Tag = "r3";
            r3.Position = ProjectGame.toJVector(new Vector3(3.37f + 0.2f, 2.78f, -4.59f - 0.4f));
            r3.Orientation = ProjectGame.toJMatrix(Matrix.RotationY(DegreeToRadian(0)));
            game.world.AddBody(r3);

            RigidBody l3 = new RigidBody(new BoxShape(7.109f, 5.629f, 7.177f));
            l3.IsStatic = true;
            l3.Tag = "l3";
            l3.Position = ProjectGame.toJVector(new Vector3(-13.5f + 0.2f, 2.815f, -0.937f - 0.4f));
            l3.Orientation = ProjectGame.toJMatrix(Matrix.RotationY(DegreeToRadian(89.962f)));
            game.world.AddBody(l3);

            RigidBody m2 = new RigidBody(new BoxShape(2.185f, 0.962f + 1.193f + 1.256f, 2.175f));
            m2.IsStatic = true;
            m2.Tag = "m2";
            m2.Position = ProjectGame.toJVector(new Vector3(5.353f + 0.2f, (0.962f + 1.193f + 1.256f) / 2, 2.66f - 0.4f));
            m2.Orientation = ProjectGame.toJMatrix(Matrix.RotationY(DegreeToRadian(0)));
            game.world.AddBody(m2);


            RigidBody l1 = new RigidBody(new BoxShape(4.35f, 4.964f, 2.586f));
            l1.IsStatic = true;
            l1.Tag = "l1";
            l1.Position = ProjectGame.toJVector(new Vector3(15.0f + 0.2f, .49f, 9.98f - 0.4f));
            l1.Orientation = ProjectGame.toJMatrix(Matrix.RotationY(DegreeToRadian(-165.254f)));
            game.world.AddBody(l1);

            RigidBody l2 = new RigidBody(new BoxShape(5.166f, 2.838f, 4.017f));
            l2.IsStatic = true;
            l2.Tag = "l2";
            l2.Position = ProjectGame.toJVector(new Vector3(3.12f + 0.2f, 0.49f, 8.93f - 0.4f));
            l2.Orientation = ProjectGame.toJMatrix(Matrix.RotationY(DegreeToRadian(-165.579f)));
            game.world.AddBody(l2);

            RigidBody m11 = new RigidBody(new BoxShape(0.3f, 2.838f, 2.0f));
            m11.IsStatic = true;
            m11.Tag = "m11";
            m11.Position = ProjectGame.toJVector(new Vector3(20.3f + 0.2f, 0.49f, 3.7f - 0.4f));
            m11.Orientation = ProjectGame.toJMatrix(Matrix.RotationY(DegreeToRadian(-2.5f)));
            game.world.AddBody(m11);

            RigidBody m12 = new RigidBody(new BoxShape(2.0f, 2.838f, 0.3f));
            m12.IsStatic = true;
            m12.Tag = "m12";
            m12.Position = ProjectGame.toJVector(new Vector3(19.35f + 0.2f, 0.49f, 4.3f - 0.4f));
            m12.Orientation = ProjectGame.toJMatrix(Matrix.RotationY(DegreeToRadian(-182.374f)));
            game.world.AddBody(m12);
            

            RigidBody m13 = new RigidBody(new BoxShape(0.3f, 2.838f, 2.0f));
            m13.IsStatic = true;
            m13.Tag = "m13";
            m13.Position = ProjectGame.toJVector(new Vector3(18.65f + 0.2f, 0.49f, 3.7f - 0.4f));
            m13.Orientation = ProjectGame.toJMatrix(Matrix.RotationY(DegreeToRadian(-2.5f)));
            game.world.AddBody(m13);

            RigidBody ground = new RigidBody(new BoxShape(53.6f, 0f, 60.5f));
            ground.IsStatic = true;
            ground.Tag = "ground";
            ground.Position = new JVector(0.18680f, -0.9f, -8.08779f);

            this.rigidBody = ground;
            game.world.AddBody(ground);
            //this.rigidBody.IsStatic = true;

            //renderToTexture = new SharpDX.Direct3D11.Texture2D(game.GraphicsDevice, new Texture2DDescription
            //{
            //    ArraySize = 1,
            //    BindFlags = BindFlags.RenderTarget,
            //    CpuAccessFlags = CpuAccessFlags.None,
            //    Format = Format.R8G8B8A8_UNorm,
            //    Width = 1024,
            //    Height = 1024,
            //    MipLevels = 1,
            //    OptionFlags = ResourceOptionFlags.None,
            //    SampleDescription = new SampleDescription(1, 0),
            //    Usage = ResourceUsage.Default
            //});


            _shadowMap = RenderTarget2D.New(game.GraphicsDevice, game.GraphicsDevice.BackBuffer.Width, game.GraphicsDevice.BackBuffer.Height, PixelFormat.R32.Float);
            _shadowZBuffer = DepthStencilBuffer.New(game.GraphicsDevice, game.GraphicsDevice.BackBuffer.Width, game.GraphicsDevice.BackBuffer.Height, DepthFormat.Depth32);
        }

        //public async static Task<Landscape> LoadLandscape(string modelName, Vector3 position, ProjectGame game)
        //{
        //    RigidBody body = await ModelLoader.loadRigidBody(modelName);
        //    return new Landscape(modelName, position, game, body);
        //}

        public override void Update(GameTime gametime)
        {
            
        }

        public override void Draw(GameTime gametime)
        {

            var depth = game.GraphicsDevice.DepthStencilBuffer;
            //base.Draw(gametime);
            BoundingSphere boundary = model.CalculateBounds();
            Matrix world = Matrix.Translation(-boundary.Center.X, -boundary.Center.Y, -boundary.Center.Z);
            game.landEffect.Parameters["lwvp"].SetValue(lightsViewProjectionMatrix);
            game.landEffect.Parameters["g_LightPos"].SetValue(lightPos);
            game.landEffect.Parameters["g_mLightView"].SetValue(lightView);
            game.landEffect.Parameters["g_mLightProj"].SetValue(lightProj);
            game.landEffect.Parameters["g_mWorld"].SetValue(world);
            //model.Draw(game.GraphicsDevice, world, game.Camera.View, game.Camera.Projection);
            //renderToTextureRTV = new RenderTargetView(game.GraphicsDevice, _shadowMap);
            //_shadowMap = RenderTarget2D.New(game.GraphicsDevice, game.GraphicsDevice.BackBuffer.Width, game.GraphicsDevice.BackBuffer.Height, PixelFormat.R32.Float);
            //_shadowZBuffer = DepthStencilBuffer.New(game.GraphicsDevice, game.GraphicsDevice.BackBuffer.Width, game.GraphicsDevice.BackBuffer.Height, DepthFormat.Depth32);
            game.GraphicsDevice.SetRenderTargets(_shadowZBuffer, _shadowMap);
            game.GraphicsDevice.Clear(Color.Black);
            Matrix[] boneTrans = new Matrix[1000];
            model.CopyBoneTransformsTo(boneTrans);
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    foreach (var x in part.VertexBuffer.Resource.Layout.BufferLayouts)
                    {
                        foreach(var y in x.VertexElements)
                        {
                            y.ToString();
                        }
                    }
                    game.landEffect.Techniques[0].Passes[0].Apply();
                    part.Draw(game.GraphicsDevice);
                }

            }
            game.GraphicsDevice.SetRenderTargets(depth, game.GraphicsDevice.BackBuffer);
            game.landEffect.Parameters["g_CameraPos"].SetValue(game.Camera.Position);
            game.landEffect.Parameters["g_mCameraView"].SetValue(game.Camera.View);
            game.landEffect.Parameters["g_mCameraProj"].SetValue(game.Camera.Projection);
            game.landEffect.Parameters["g_ShadowMapTexture"].SetResource(_shadowMap);
            foreach (ModelMesh mesh in model.Meshes)
            {

                foreach (ModelMeshPart part in mesh.MeshParts)
                {

                    if (part.Material.HasProperty(TextureKeys.DiffuseTexture))
                    {
                        Texture y = part.Material.GetProperty(TextureKeys.DiffuseTexture).ElementAt(0).Texture;
                        game.landEffect.Parameters["g_MeshTexture"].SetResource(y);

                        game.landEffect.Techniques[1].Passes[0].Apply();
                        if (y != null)
                        {
                            part.Draw(game.GraphicsDevice);
                        }


                    }
                    else
                    {

                    }

                }

            }
        }
        private float DegreeToRadian(float angle)
        {
            return (float)Math.PI * angle / 180.0f;
        }


    }
}


