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

        public Shader Shader => shader;

        // Material properties
        public Matrix4 ModelViewProjMatrix { get; set; }
        public Matrix4 ModelWorldMatrix { get; set; }
        public Matrix4 WorldViewMatrix { get; set; }
        public Matrix4 NormalMatrix { get; set; }
        public Vector3 CameraPosition { get; set; }

        protected string shaderFileName;
        protected Shader shader;
        private bool loaded = false;


        public Material(string shaderFileName)
        {
            this.shaderFileName = shaderFileName;

            LoadResources();
        }

        public void LoadResources()
        {
            loaded = true;

            shader = Shader.LoadShaderFromName(shaderFileName);
        }

        public virtual void Apply()
        {
            shader.Program.Use();

            SetMatrixUniform("u_modelViewProjMatrix", ModelViewProjMatrix, GL.UniformMatrix4);
            SetMatrixUniform("u_modelWorldMatrix", ModelWorldMatrix, GL.UniformMatrix4);
            SetMatrixUniform("u_worldViewMatrix", WorldViewMatrix, GL.UniformMatrix4);
            SetMatrixUniform("u_normalMatrix", NormalMatrix, GL.UniformMatrix4);
            SetValueUniform("u_cameraPosition", CameraPosition, GL.Uniform3);
        }

        public void CleanUp()
        {
            shader.Program.CleanUp();
        }

        #region Helpers
        public delegate void GLValue<T>(int location, T value);
        public void SetValueUniform<T>(string name, T value, GLValue<T> glMethod)
        {
            glMethod(GetLocation(name), value);
        }

        public delegate void GLMatrix<T>(int location, bool transpose, ref T matrix);
        public void SetMatrixUniform<T>(string name, T matrix, GLMatrix<T> glMethod)
        {
            glMethod(GetLocation(name), false, ref matrix);
        }

        public void SetBoolUniform(string name, bool value)
        {
            GL.Uniform1(GetLocation(name), value ? 1 : 0);
        }

        public int GetLocation(string name)
        {
            return shader.Program.GetUniformLocation(name);
        }
        #endregion

    }
}
