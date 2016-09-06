using BrokenEngine.Open_GL;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace BrokenEngine.Materials
{
    public class Material
    {

        public readonly string Name;

        public ShaderProgram ShaderProgram { get { return shaderProgram; } }

        public dynamic Parameters = new System.Dynamic.ExpandoObject();

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
            // TODO: use consts
            var vertTxt = ResourceManager.GetString($"Shaders/{Name}_vert.glsl");
            var fragTxt = ResourceManager.GetString($"Shaders/{Name}_frag.glsl");

            var vert = new Shader(ShaderType.VertexShader, vertTxt);
            var frag = new Shader(ShaderType.FragmentShader, fragTxt);

            shaderProgram = new ShaderProgram(vert, frag);
        }

        public virtual void Apply()
        {
            shaderProgram.Use();

            SetMatrixUniform("u_modelViewProjMatrix", (Matrix4) Parameters.ModelViewProjMatrix, GL.UniformMatrix4);
            SetMatrixUniform("u_modelWorldMatrix", (Matrix4) Parameters.ModelWorldMatrix, GL.UniformMatrix4);
            SetMatrixUniform("u_worldViewMatrix", (Matrix4) Parameters.WorldViewMatrix, GL.UniformMatrix4);
            SetMatrixUniform("u_normalMatrix", (Matrix4) Parameters.NormalMatrix, GL.UniformMatrix4);
            SetValueUniform("u_cameraPosition", (Vector3) Parameters.CameraPosition, GL.Uniform3);
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