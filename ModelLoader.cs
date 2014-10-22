using SharpDX.Toolkit.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jitter.Collision.Shapes;
using Jitter.LinearMath;
using Jitter.Collision;
using SharpDX;
using System.IO;
using Windows;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Reflection;
using System.Threading;
using Jitter.Dynamics;
namespace Project
{
    public class ModelLoader
    {
        //static TriangleMeshShape tms = new TriangleMeshShape();
        public static Model loadModel(string modelName, ProjectGame game)
        {
            return game.Content.Load<Model>(modelName);
        }

        public static RigidBody loadRigidBodyHull(string fileName)
        {
            //System.Diagnostics.Debug.WriteLine()
            //StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            List<JVector> positions = new List<JVector>();
            try
            {

                string file = SharpDX.IO.NativeFile.ReadAllText(@"Content\" + fileName + ".mesh");
                string[] lines = file.Split('\n');
                foreach (String line in lines)
                {
                    string[] lineArray = line.Split(' ');
                    if (lineArray[0].Equals("v"))
                    {
                        //System.Diagnostics.Debug.WriteLine(lineArray);
                        positions.Add(new JVector(float.Parse(lineArray[1]), float.Parse(lineArray[2]), float.Parse(lineArray[3])));
                    }
                }
                return new RigidBody(new ConvexHullShape(positions));

            }
            catch (ArgumentException e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return null;
            }
        }

        public static RigidBody loadRigidBody(string fileName)
        {
            //System.Diagnostics.Debug.WriteLine()
            //StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            List<JVector> positions = new List<JVector>();
            List<TriangleVertexIndices> tris = new List<TriangleVertexIndices>();
            try
            {

                string file = SharpDX.IO.NativeFile.ReadAllText(@"Content\"+fileName + ".mesh");
                string[] lines = file.Split('\n');
                foreach (String line in lines)
                {
                    string[] lineArray = line.Split(' ');
                    if (lineArray[0].Equals("v"))
                    {
                        //System.Diagnostics.Debug.WriteLine(lineArray);
                        positions.Add(new JVector(float.Parse(lineArray[1]), float.Parse(lineArray[2]), float.Parse(lineArray[3])));
                    }
                    if (lineArray[0].Equals("f"))
                    {
                        //System.Diagnostics.Debug.WriteLine(lineArray);
                        tris.Add(new TriangleVertexIndices(int.Parse(lineArray[1]) - 1, int.Parse(lineArray[2]) - 1, int.Parse(lineArray[3]) - 1));
                    }
                }
                Octree tree = new Octree(positions, tris);
                return new RigidBody(new TriangleMeshShape(tree));
                
            }
            catch (ArgumentException e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return null;
            }
        }



        public async static Task<SoftBody> loadSoftBody(string fileName)
        {
            //System.Diagnostics.Debug.WriteLine()
            //StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            List<JVector> positions = new List<JVector>();
            List<TriangleVertexIndices> tris = new List<TriangleVertexIndices>();
            try
            {
                var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                folder = await folder.GetFolderAsync("Content");
                System.Diagnostics.Debug.WriteLine(folder.ToString());
                var file = await folder.GetFileAsync(fileName + ".obj");
                var lines = await Windows.Storage.FileIO.ReadLinesAsync(file);

                foreach (String line in lines)
                {
                    string[] lineArray = line.Split(' ');
                    if (lineArray[0].Equals("v"))
                    {
                        //System.Diagnostics.Debug.WriteLine(lineArray);
                        positions.Add(new JVector(float.Parse(lineArray[1]), float.Parse(lineArray[2]), float.Parse(lineArray[3])));
                    }
                    if (lineArray[0].Equals("f"))
                    {
                        //System.Diagnostics.Debug.WriteLine(lineArray);
                        tris.Add(new TriangleVertexIndices(int.Parse(lineArray[1]) - 1, int.Parse(lineArray[2]) - 1, int.Parse(lineArray[3]) - 1));
                    }
                }
                return new SoftBody(tris, positions);

            }
            catch (ArgumentException e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return null;
            }
        }

        //public async static Task<RigidBody> loadModel(string path)
        //{
        //    TriangleMeshShape x = await loadTriangleMeshShape(path);
        //    return new RigidBody(x);
        //}

        //public static TriangleMeshShape loadModel()
        //{
        //    List<JVector> positions = new List<JVector>();
        //    List<TriangleVertexIndices> tris = new List<TriangleVertexIndices>();

        //    double[] x = new double[] {
        //        1.000000, -1.000000, -1.000000, 1.000000, -1.000000, 1.000000,-1.000000, -1.000000,
        //        1.000000,1.000000, -1.000000, -1.000000,1.000000, 1.000000, -0.999999,0.999999, 
        //        1.000000, 1.000001,-1.000000, 1.000000, 1.000000,-1.000000, 1.000000, -1.000000
        //    };
        //    int[] y = new int[] {
        //        1, 2, 3, 7, 6, 5,0, 4, 5,1, 5, 6,6 ,7 ,3,0, 3 ,7,0, 1 ,3,4, 7, 5,1, 0, 5,2, 1, 6,2, 6, 3,4, 0, 7
        //    };
        //    for (int i = 0; i < x.Length; i += 3)
        //    {
        //        positions.Add(new JVector((float)x[i], (float)x[i + 1], (float)x[i + 2]));
        //    }
        //    for (int i = 0; i < x.Length; i += 3)
        //    {
        //        tris.Add(new TriangleVertexIndices(y[i], y[i + 1], y[i + 2]));
        //    }
        //    Octree tree = new Octree(new List<JVector>(positions), new List<TriangleVertexIndices>(tris));
        //    return new TriangleMeshShape(tree);
        //}



        //public static TriangleMeshShape BuildOctree(Model model)
        //{

        //    List<JVector> vertices = new List<JVector>();
        //    List<TriangleVertexIndices> indices = new List<TriangleVertexIndices>();
        //    Matrix[] bones = new Matrix[model.Bones.Count];
        //    model.CopyAbsoluteBoneTransformsTo(bones);

        //    foreach (ModelMesh modelMesh in model.Meshes)
        //    {
        //        JMatrix boneTransform = toJMatrix(bones[modelMesh.ParentBone.Index]);
        //        foreach (ModelMeshPart meshPart in modelMesh.MeshParts)
        //        {
        //            int offset = vertices.Count;
        //            var meshVertices = meshPart.VertexBuffer.Resource.Buffer.GetData<JVector>();
        //            for (int i = 0; i < meshVertices.Length; ++i)
        //            {
        //                JVector.Transform(ref meshVertices[i], ref boneTransform, out meshVertices[i]);
        //            }
        //            vertices.AddRange(meshVertices);    // append transformed vertices

        //            var indexElements = meshPart.IndexBuffer.Resource.GetData<int>(); // this is dangerous if the model uses larger integers

        //            // Each TriangleVertexIndices holds the indices that constitute a triangle primitive
        //            TriangleVertexIndices[] tvi = new TriangleVertexIndices[indexElements.Length];
        //            for (int i = 0; i <= tvi.Length - 2; i += 3)
        //            {
        //                tvi[i].I0 = indexElements[i + 0] + offset;
        //                tvi[i].I1 = indexElements[i + 1] + offset;
        //                tvi[i].I2 = indexElements[i + 2] + offset;
        //            }
        //            indices.AddRange(tvi);  // append triangles           
        //        }
        //    }
        //    Octree ot = new Octree(vertices, indices);

        //    //ot.BuildOctree(); // (already happens in Octree constructor)
        //    return new TriangleMeshShape(ot);
        //}

        
    }
}
