using System;
using BrokenEngine.Open_GL;
using BrokenEngine.Open_GL.Shader;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace BrokenEngine.Materials
{
    public class Material
    {

        private const string SHADER_DIRECTORY = "Shaders/";
        private const string VERTEX_SHADER_URI = SHADER_DIRECTORY + "{0}_vert.glsl";
        private const string FRAGMENT_SHADER_URI = SHADER_DIRECTORY + "{0}_frag.glsl";

        public readonly string Name;

        public Shader Shader => shader;

        // Material properties
        public Matrix4 ModelViewProjMatrix { get; set; }
        public Matrix4 ModelWorldMatrix { get; set; }
        public Matrix4 WorldViewMatrix { get; set; }
        public Matrix4 NormalMatrix { get; set; }
        public Vector3 CameraPosition { get; set; }

        protected Shader shader;
        private bool loaded = false;

        public Material(string name)
        {
            Name = name;

            LoadResources();
        }

        public void LoadResources()
        {
            loaded = true;

            shader = Shader.LoadShaderFromName(Name);
        }

        public virtual void Apply()
        {
            shader.Program.Use();

            shader.Program.SetMatrixUniform("u_modelViewProjMatrix", ModelViewProjMatrix, GL.UniformMatrix4);
            shader.Program.SetMatrixUniform("u_modelWorldMatrix", ModelWorldMatrix, GL.UniformMatrix4);
            shader.Program.SetMatrixUniform("u_worldViewMatrix", WorldViewMatrix, GL.UniformMatrix4);
            shader.Program.SetMatrixUniform("u_normalMatrix", NormalMatrix, GL.UniformMatrix4);
            shader.Program.SetValueUniform("u_cameraPosition", CameraPosition, GL.Uniform3);
        }

        public void CleanUp()
        {
            shader.Program.CleanUp();
        }

    }
}
