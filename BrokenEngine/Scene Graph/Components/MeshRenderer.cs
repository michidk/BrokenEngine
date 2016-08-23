using System;
using System.Linq;
using System.Security.Cryptography;
using BrokenEngine.Materials;
using BrokenEngine.Mesh;
using BrokenEngine.Open_GL;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace BrokenEngine.Scene_Graph.Components
{
    public class MeshRenderer : Component
    {

        public Mesh.Mesh Mesh;
        public Material Material;

        private Buffer<Vertex> vertexBuffer;
        private Buffer<ushort> indexBuffer;
        private VertexArray<Vertex> vertexArray;

        public MeshRenderer(Mesh.Mesh mesh) : this (mesh, new VertexColorMaterial())
        {
        }

        public MeshRenderer(Mesh.Mesh mesh, Material material)
        {
            Mesh = mesh;
            Material = material;

            if (Mesh == null)
                throw new ArgumentNullException("Mesh can't be null!");
        }

        public override void OnStart()
        {
            base.OnStart();

            Reload();
        }

        public void Reload()
        {
            vertexBuffer = new StaticBuffer<Vertex>(Vertex.Size, Mesh.Vertices, BufferTarget.ArrayBuffer);

            var indices = from face in Mesh.Faces from index in face.Indices select index;
            indexBuffer = new StaticBuffer<ushort>(sizeof(ushort), indices.ToArray(), BufferTarget.ElementArrayBuffer);
            

            unsafe
            {
                vertexArray = new VertexArray<Vertex>(
                   vertexBuffer, Material.ShaderProgram,
                   new VertexAttribute("v_position", 3, VertexAttribPointerType.Float, Vertex.Size, 0),
                   new VertexAttribute("v_color", 4, VertexAttribPointerType.Float, Vertex.Size, sizeof(Vector3)),  // TODO: make builder, which calc size automaticly
                   new VertexAttribute("v_normal", 3, VertexAttribPointerType.Float, Vertex.Size, sizeof(Vector3) + sizeof(Color4)),
                   new VertexAttribute("v_uv", 3, VertexAttribPointerType.Float, Vertex.Size, sizeof(Vector3) + sizeof(Color4) + sizeof(Vector3))
                );
            }

        }

        public void Render(Matrix4 viewMatrix, Matrix4 projMatrix)
        {
            Matrix4 modelViewProjection = this.GameObject.ModelMatrix * viewMatrix * projMatrix;
            Material.ModelViewProjMatrix = modelViewProjection;
            Material.ModelWorldMatrix = this.GameObject.ModelMatrix;
            Material.WorldViewMatrix = viewMatrix;
            Material.Apply();


            vertexBuffer.Bind();
            vertexBuffer.BufferData();

            vertexArray.Bind();

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