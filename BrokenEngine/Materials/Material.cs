using System;
using BrokenEngine.Open_GL;
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

        public ShaderProgram ShaderProgram { get { return shaderProgram; } }

        // Material properties
        public Matrix4 ModelViewProjMatrix { get; set; }
        public Matrix4 ModelWorldMatrix { get; set; }
        public Matrix4 WorldViewMatrix { get; set; }
        public Matrix4 NormalMatrix { get; set; }
        public Vector3 CameraPosition { get; set; }

        protected ShaderProgram shaderProgram;
        private bool loaded = false;

        public Material(string name)
        {
            Name = name;

            LoadResources();
        }

        public void LoadResources()
        {
            loaded = true;

            LoadShaders();
        }

        protected void LoadShaders()
        {
            var vertUri = String.Format(VERTEX_SHADER_URI, Name);
            var fragUri = String.Format(FRAGMENT_SHADER_URI, Name);

            var vertTxt = ResourceManager.GetString(vertUri);
            var fragTxt = ResourceManager.GetString(fragUri);

            var vert = new Shader(ShaderType.VertexShader, vertTxt);
            var frag = new Shader(ShaderType.FragmentShader, fragTxt);

            try
            {
                vert.CompileShader();
                frag.CompileShader();
            }
            catch (ShaderCompileException e)
            {
                Globals.Logger.Error(e, $"Failed to compile shader {Name}");
            }

            shaderProgram = new ShaderProgram(vert, frag);
        }

        public virtual void Apply()
        {
            shaderProgram.Use();

            SetMatrixUniform("u_modelViewProjMatrix", ModelViewProjMatrix, GL.UniformMatrix4);
            SetMatrixUniform("u_modelWorldMatrix", ModelWorldMatrix, GL.UniformMatrix4);
            SetMatrixUniform("u_worldViewMatrix", WorldViewMatrix, GL.UniformMatrix4);
            SetMatrixUniform("u_normalMatrix", NormalMatrix, GL.UniformMatrix4);
            SetValueUniform("u_cameraPosition", CameraPosition, GL.Uniform3);
        }

        public void CleanUp()
        {
            GL.UseProgram(0);
        }

        #region Helpers
        protected delegate void GLValue<T>(int location, T value);
        protected void SetValueUniform<T>(string name, T value, GLValue<T> glMethod)
        {
            var i = shaderProgram.GetUniformLocation(name);
            glMethod(i, value);
        }

        protected delegate void GLMatrix<T>(int location, bool transpose, ref T matrix);
        protected void SetMatrixUniform<T>(string name, T matrix, GLMatrix<T> glMethod)
        {
            var i = shaderProgram.GetUniformLocation(name);
            glMethod(i, false, ref matrix);
        }
        #endregion

    }
}