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
    public class MeshRenderer : Component, IRenderable
    {

        public Mesh.Mesh Mesh;
        public Material Material;

        private Buffer<Vertex> vertexBuffer;
        private Buffer<ushort> indexBuffer;
        private VertexArray<Vertex> vertexArray;

        private SubmeshRenderer[] subMeshRenderers;

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


            // create subs
            int submeshCnt = Mesh.Submeshes.Count(e => e.Faces.Length != 0);
            if (subMeshRenderers == null)
                subMeshRenderers = new SubmeshRenderer[submeshCnt];

            int c = 0;
            foreach (var submesh in Mesh.Submeshes.Where(e => e.Faces.Length != 0))
            {
                // create / restore objects if lost
                if (subMeshRenderers[c] == null)
                {
                    var go = new GameObject(submesh.Name, parent: this.GameObject);
                    var comp = new SubmeshRenderer(submesh, Material);
                    go.AddComponent(comp);
                    subMeshRenderers[c] = comp;
                }
                else
                {
                    subMeshRenderers[c].Submesh = submesh;
                }

                c++;
            }
        }

        public void Render(Matrix4 viewMatrix, Matrix4 projMatrix)
        {
            SetDefaultMaterialParameter(ref Material, this.GameObject.ModelMatrix, viewMatrix, projMatrix);

            vertexBuffer.Bind();
            vertexBuffer.BufferData();

            vertexArray.Bind();

            indexBuffer.Bind();
            indexBuffer.BufferData();

            GL.DrawElements(BeginMode.Triangles, indexBuffer.Count, DrawElementsType.UnsignedShort, 0);

            foreach (var renderer in subMeshRenderers)
                renderer.Render(viewMatrix, projMatrix);

            // reset state for potential further draw calls (optional, but good practice)
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            Material.CleanUp();
        }

        public static void SetDefaultMaterialParameter(ref Material material, Matrix4 modelMatrix, Matrix4 viewMatrix, Matrix4 projMatrix)
        {
            //Console.WriteLine(viewMatrix.ExtractTranslation() + "\n==========");
            material.Parameters.ModelWorldMatrix = modelMatrix;
            material.Parameters.WorldViewMatrix = viewMatrix;
            material.Parameters.ModelViewProjMatrix = modelMatrix * viewMatrix * projMatrix;
            material.Parameters.NormalMatrix = Matrix4.Transpose(Matrix4.Invert(modelMatrix));
            material.Apply();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            foreach (var go in subMeshRenderers)
            {
                go.GameObject.Destroy();
            }
        }

    }
}