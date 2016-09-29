using System.Linq;
using BrokenEngine.Materials;
using BrokenEngine.Mesh;
using BrokenEngine.Open_GL.Buffer;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace BrokenEngine.Components
{
    public class SubmeshRenderer : Component, IRenderable
    {

        public Submesh Submesh;
        public Material Material;

        private Buffer<ushort> indexBuffer;

        public SubmeshRenderer(Submesh submesh, Material material)
        {
            this.Submesh = submesh;
            this.Material = material;

            var indices = from face in Submesh.Faces from index in face.Indices select index;
            indexBuffer = new StaticBuffer<ushort>(sizeof(ushort), indices.ToArray(), BufferTarget.ElementArrayBuffer);
        }

        public void Render(Matrix4 viewMatrix, Matrix4 viewProjectionMatrix)
        {
            MeshRenderer.SetDefaultMaterialParameter(ref Material, this.GameObject.LocalToWorldMatrix, viewMatrix, viewProjectionMatrix, GameObject.NormalMatrix);

            indexBuffer.Bind();
            indexBuffer.BufferData();

            GL.DrawElements(BeginMode.Triangles, indexBuffer.Count, DrawElementsType.UnsignedShort, 0);

            indexBuffer.Reset();
        }

    }
}
