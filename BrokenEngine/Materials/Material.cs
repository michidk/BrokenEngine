
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGLTest
{
    public class Material
    {

        public class MaterialProperties
        {
            public Matrix4 ModelViewProjMatrix;
        }

        public readonly string Name;

        public ShaderProgram ShaderProgram { get { return shaderProgram; } }

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
            var vertTxt = System.Text.Encoding.Default.GetString(Properties.Resources.unlit_vert);
            var fragTxt = System.Text.Encoding.Default.GetString(Properties.Resources.unlit_frag);

            var vert = new Shader(ShaderType.VertexShader, vertTxt);
            var frag = new Shader(ShaderType.FragmentShader, fragTxt);

            shaderProgram = new ShaderProgram(vert, frag);
        }

        public void Apply(MaterialProperties properties)
        {
            shaderProgram.Use();

            SetMatrixUniform("modelViewProjMatrix", properties.ModelViewProjMatrix, GL.UniformMatrix4);
        }

        public void CleanUp()
        {
            GL.UseProgram(0);
        }

        #region Helpers
        protected delegate void GLMatrix<T>(int location, bool transpose, ref T matrix);
        protected void SetMatrixUniform<T>(string name, T matrix, GLMatrix<T> glMethod)
        {
            var i = shaderProgram.GetUniformLocation(name);
            glMethod(i, false, ref matrix);
        }
        #endregion

    }
}