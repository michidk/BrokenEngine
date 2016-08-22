using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenGLTest
{
    public class MeshRenderer : Component
    {

        public Mesh Mesh;
        public Material Material;

        private Buffer<Vertex> vertexBuffer;
        private Buffer<ushort> indexBuffer;
        private VertexArray<Vertex> vertexArray;

        public MeshRenderer(Mesh mesh)
        {
            Mesh = mesh;
            Material = new UnlitMaterial();
        }

        public MeshRenderer(Mesh mesh, Material material)
        {
            Mesh = mesh;
            Material = material;
        }

        public override void OnStart()
        {
            base.OnStart();

            Reload();
        }

        public void Reload()
        {
            vertexBuffer = new Buffer<Vertex>(Vertex.Size, Mesh.Vertices, BufferTarget.ArrayBuffer);

            indexBuffer = new Buffer<ushort>(sizeof(ushort), BufferTarget.ElementArrayBuffer);
            foreach (var group in Mesh.FaceGroups)
            {
                foreach (var face in group.Faces)
                {
                    indexBuffer.Data.Add(face.Vertices[0].VertexIndex);
                    indexBuffer.Data.Add(face.Vertices[1].VertexIndex);
                    indexBuffer.Data.Add(face.Vertices[2].VertexIndex);
                }
            }

            vertexArray = new VertexArray<Vertex>(
                vertexBuffer, Material.ShaderProgram,
                new VertexAttribute("vPosition", 3, VertexAttribPointerType.Float, Vertex.Size, 0),
                new VertexAttribute("vColor", 4, VertexAttribPointerType.Float, Vertex.Size, 12)
            );
        }

        public void Render(Matrix4 viewMatrix, Matrix4 projMatrix)
        {
            Matrix4 modelViewProjection = this.GameObject.ModelMatrix * viewMatrix * projMatrix;

            Material.Apply(new Material.MaterialProperties {
                ModelViewProjMatrix = modelViewProjection
            });

            // TODO: only bind the vertex array every frame, the other stuff can be bind and buffered in OnLoad() (https://github.com/opentk/opentk/blob/master/Source/Examples/OpenGL/3.x/HelloGL3.cs)
            vertexBuffer.Bind();
            vertexArray.Bind();

            vertexBuffer.BufferData();
            //GL.DrawArrays(BeginMode.Triangles, 0, vertexBuffer.Count);

            indexBuffer.Bind();
            indexBuffer.BufferData();

            GL.DrawElements(BeginMode.Triangles, indexBuffer.Count, DrawElementsType.UnsignedShort, 0);

            // reset state for potential further draw calls (optional, but good practice)
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            Material.CleanUp();
        }

    }
}