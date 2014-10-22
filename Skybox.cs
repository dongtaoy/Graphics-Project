using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;

namespace Project
{
    using SharpDX.Toolkit.Graphics;
    class SkyBox : DrawableGameObject
    {
        public BasicEffect basicEffect;
        public VertexInputLayout inputLayout;
        public Buffer<VertexPositionTexture> vertices;
        public SkyBox(string modelName, Vector3 position, ProjectGame game)
        {
            // Specify the file name of the texture being used.  Note that SkyBox.png has been added as a reference inside Content with "Copy to Output Director" set to "Copy Always"
            //textureName = "Day_Skybox.png";
            this.gameObjectType = Project.GameObjectType.DRAWABLE;
            this.game = game;
            this.game.Add(this);
            basicEffect = new BasicEffect(game.GraphicsDevice);
            basicEffect.Texture = game.Content.Load<Texture2D>(modelName);
            basicEffect.TextureEnabled = true;
            basicEffect.VertexColorEnabled = false;

            // This is where the vertices of the cube are specififed.  Each vertex has a position and a texture co-ordinate.
            // TASK 2: Correct UV-Coordinates for the SkyBox

            vertices = Buffer.Vertex.New(
                game.GraphicsDevice,
                new[]
                    {
                        new VertexPositionTexture(new Vector3(-40.0f, -40.0f, -40.0f), new Vector2(0.0f, 2/3f)), // Front
                        new VertexPositionTexture(new Vector3(-40.0f, 40.0f, -40.0f), new Vector2(0.0f, 1/3f)),
                        new VertexPositionTexture(new Vector3(40.0f, 40.0f, -40.0f), new Vector2(1/4f, 1/3f)),
                        new VertexPositionTexture(new Vector3(-40.0f, -40.0f, -40.0f), new Vector2(0.0f, 2/3f)),
                        new VertexPositionTexture(new Vector3(40.0f, 40.0f, -40.0f), new Vector2(1/4f, 1/3f)),
                        new VertexPositionTexture(new Vector3(40.0f, -40.0f, -40.0f), new Vector2(1/4f, 2/3f)),

                        new VertexPositionTexture(new Vector3(-40.0f, -40.0f, 40.0f), new Vector2(3/4f, 2/3f)), // BACK
                        new VertexPositionTexture(new Vector3(40.0f, 40.0f, 40.0f), new Vector2(1/2f, 1/3f)),
                        new VertexPositionTexture(new Vector3(-40.0f, 40.0f, 40.0f), new Vector2(3/4f, 1/3f)),
                        new VertexPositionTexture(new Vector3(-40.0f, -40.0f, 40.0f), new Vector2(3/4f, 2/3f)),
                        new VertexPositionTexture(new Vector3(40.0f, -40.0f, 40.0f), new Vector2(1/2f, 2/3f)),
                        new VertexPositionTexture(new Vector3(40.0f, 40.0f, 40.0f), new Vector2(1/2f, 1/3f)),

                        new VertexPositionTexture(new Vector3(-40.0f, 40.0f, -40.0f), new Vector2(1/4f, 1/3f)), // Top
                        new VertexPositionTexture(new Vector3(-40.0f, 40.0f, 40.0f), new Vector2(1/4f, 0f)),
                        new VertexPositionTexture(new Vector3(40.0f, 40.0f, 40.0f), new Vector2(1/2f, 0f)),
                        new VertexPositionTexture(new Vector3(-40.0f, 40.0f, -40.0f), new Vector2(1/4f, 1/3f)),
                        new VertexPositionTexture(new Vector3(40.0f, 40.0f, 40.0f), new Vector2(1/2f, 0f)),
                        new VertexPositionTexture(new Vector3(40.0f, 40.0f, -40.0f), new Vector2(1/2f, 1/3f)),

                        new VertexPositionTexture(new Vector3(-40.0f, -40.0f, -40.0f), new Vector2(1/4f, 2/3f)), // Bottom
                        new VertexPositionTexture(new Vector3(40.0f, -40.0f, 40.0f), new Vector2(1/2f, 1f)),
                        new VertexPositionTexture(new Vector3(-40.0f, -40.0f, 40.0f), new Vector2(1/4f, 1f)),
                        new VertexPositionTexture(new Vector3(-40.0f, -40.0f, -40.0f), new Vector2(1/4f, 2/3f)),
                        new VertexPositionTexture(new Vector3(40.0f, -40.0f, -40.0f), new Vector2(1/2f, 2/3f)),
                        new VertexPositionTexture(new Vector3(40.0f, -40.0f, 40.0f), new Vector2(1/2f, 1f)),

                        new VertexPositionTexture(new Vector3(-40.0f, -40.0f, -40.0f), new Vector2(1/2f, 2/3f)), // Left
                        new VertexPositionTexture(new Vector3(-40.0f, -40.0f, 40.0f), new Vector2(1/4f, 2/3f)),
                        new VertexPositionTexture(new Vector3(-40.0f, 40.0f, 40.0f), new Vector2(1/4f, 1/3f)),
                        new VertexPositionTexture(new Vector3(-40.0f, -40.0f, -40.0f), new Vector2(1/2f, 2/3f)),
                        new VertexPositionTexture(new Vector3(-40.0f, 40.0f, 40.0f), new Vector2(1/4f, 1/3f)),
                        new VertexPositionTexture(new Vector3(-40.0f, 40.0f, -40.0f), new Vector2(1/2f, 1/3f)),

                        new VertexPositionTexture(new Vector3(40.0f, -40.0f, -40.0f), new Vector2(3/4f, 2/3f)), // Right
                        new VertexPositionTexture(new Vector3(40.0f, 40.0f, 40.0f), new Vector2(1f, 1/3f)),
                        new VertexPositionTexture(new Vector3(40.0f, -40.0f, 40.0f), new Vector2(1f, 2/3f)),
                        new VertexPositionTexture(new Vector3(40.0f, -40.0f, -40.0f), new Vector2(3/4f, 2/3f)),
                        new VertexPositionTexture(new Vector3(40.0f, 40.0f, -40.0f), new Vector2(3/4f, 1/3f)),
                        new VertexPositionTexture(new Vector3(40.0f, 40.0f, 40.0f), new Vector2(1f, 1/3f)),
            });
            inputLayout = VertexInputLayout.FromBuffer(0, vertices);
        }

        public override void Update(GameTime gametime)
        {
            basicEffect.World = Matrix.Identity;//Matrix.RotationX(time) * Matrix.RotationY(time * 2.0f);// * Matrix.RotationZ(time * .7f);
            basicEffect.Projection = game.Camera.Projection;
            basicEffect.View = game.Camera.View;
       
        }

        public override void Draw(GameTime gametime)
        {
            game.GraphicsDevice.SetVertexBuffer(vertices);
            game.GraphicsDevice.SetVertexInputLayout(inputLayout);

            // Apply the basic effect technique and draw the rotating cube
            basicEffect.CurrentTechnique.Passes[0].Apply();
            game.GraphicsDevice.Draw(PrimitiveType.TriangleList, vertices.ElementCount);
        }
    }
}
