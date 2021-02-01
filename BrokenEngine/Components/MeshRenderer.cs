using System;
using System.Linq;
using System.Xml.Serialization;
using BrokenEngine.Materials;
using BrokenEngine.Models;
using BrokenEngine.OpenGL;
using BrokenEngine.OpenGL.Buffer;
using BrokenEngine.SceneGraph;
using BrokenEngine.Utils.Attributes;
using OpenTK;
using OpenTK.Mathematics;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace BrokenEngine.Components
{
    public class MeshRenderer : Component, IRenderable
    {

        private Buffer<Vertex> vertexBuffer;
        private Buffer<ushort> indexBuffer;
        private VertexArray<Vertex> vertexArray;

        private SubmeshRenderer[] subMeshRenderers;


        public Model Model;
        public Material Material;


        [XmlConstructor]
        protected MeshRenderer()
        {
        }

        public MeshRenderer(Model model, Material shader)
        {
            this.Model = model;
            this.Material = shader;
        }

        public override void OnInitialize()
        {
            base.OnInitialize();
        }

        public override void OnStart()
        {
            base.OnStart();

            // TODO: turn back on
            // Reload();
        }

        public void Reload()
        {
            vertexBuffer = new StaticBuffer<Vertex>(Vertex.Size, Model.Mesh.Vertices, BufferTarget.ArrayBuffer);

            var indices = from face in Model.Mesh.Faces from index in face.Indices select index;
            indexBuffer = new StaticBuffer<ushort>(sizeof(ushort), indices.ToArray(), BufferTarget.ElementArrayBuffer);


            unsafe
            {
                vertexArray = new VertexArray<Vertex>(
                   vertexBuffer, Material.ShaderCompiler.Program,
                   new VertexAttribute("v_position", 3, VertexAttribPointerType.Float, Vertex.Size, 0),
                   new VertexAttribute("v_color", 4, VertexAttribPointerType.Float, Vertex.Size, sizeof(Vector3)),  // TODO: make builder, which calc size automaticly
                   new VertexAttribute("v_normal", 3, VertexAttribPointerType.Float, Vertex.Size, sizeof(Vector3) + sizeof(Color4)),
                   new VertexAttribute("v_uv", 3, VertexAttribPointerType.Float, Vertex.Size, sizeof(Vector3) + sizeof(Color4) + sizeof(Vector3))
                );
            }


            // create subs
            int submeshCnt = Model.Mesh.Submeshes.Count(e => e.Faces.Length != 0);
            if (subMeshRenderers == null)
                subMeshRenderers = new SubmeshRenderer[submeshCnt];

            int c = 0;
            foreach (var submesh in Model.Mesh.Submeshes.Where(e => e.Faces.Length != 0))
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

        public void Render(Matrix4 viewMatrix, Matrix4 viewProjectionMatrix)
        {
            SetDefaultMaterialParameter(Material, this.GameObject.LocalToWorldMatrix, viewMatrix, viewProjectionMatrix, GameObject.NormalMatrix);

            vertexBuffer.Bind();
            vertexBuffer.BufferData();

            vertexArray.Bind();

            indexBuffer.Bind();
            indexBuffer.BufferData();

            GL.DrawElements(BeginMode.Triangles, indexBuffer.Count, DrawElementsType.UnsignedShort, 0);

            foreach (var renderer in subMeshRenderers)
                renderer.Render(viewMatrix, viewProjectionMatrix);

            // reset state for potential further draw calls (optional, but good practice)
            vertexArray.Reset();
            vertexBuffer.Reset();
            indexBuffer.Reset();
            Material.CleanUp();
        }

        public static void SetDefaultMaterialParameter(Material shader, Matrix4 modelMatrix, Matrix4 viewMatrix, Matrix4 viewProjectionMatrix, Matrix4 normalMatrix)
        {
            // global variables
            shader.CameraPosition = Globals.CurrentCamera.GameObject.Position;

            // instance variables
            shader.ModelWorldMatrix = modelMatrix;
            shader.NormalMatrix = normalMatrix;
            shader.WorldViewMatrix = viewMatrix;
            shader.ModelViewProjMatrix = modelMatrix * viewProjectionMatrix;   // OpenTK matrices are transposed by default

            shader.Apply();
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
