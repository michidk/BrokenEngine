using System.Linq;
using BrokenEngine.Materials;
using BrokenEngine.Models;
using BrokenEngine.OpenGL.Buffer;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace BrokenEngine.Components
{
    public class SubmeshRenderer : Component, IRenderable
    {

        public Submesh Submesh;
        public Shader Shader;

        private Buffer<ushort> indexBuffer;

        public SubmeshRenderer(Submesh submesh, Shader shader)
        {
            this.Submesh = submesh;
            this.Shader = shader;

            var indices = from face in Submesh.Faces from index in face.Indices select index;
            indexBuffer = new StaticBuffer<ushort>(sizeof(ushort), indices.ToArray(), BufferTarget.ElementArrayBuffer);
        }

        public void Render(Matrix4 viewMatrix, Matrix4 viewProjectionMatrix)
        {
            MeshRenderer.SetDefaultMaterialParameter(Shader, this.GameObject.LocalToWorldMatrix, viewMatrix, viewProjectionMatrix, GameObject.NormalMatrix);

            indexBuffer.Bind();
            indexBuffer.BufferData();

            GL.DrawElements(BeginMode.Triangles, indexBuffer.Count, DrawElementsType.UnsignedShort, 0);

            indexBuffer.Reset();
        }

    }
}
