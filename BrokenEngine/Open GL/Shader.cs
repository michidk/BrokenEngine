using OpenTK.Graphics.OpenGL;

namespace OpenGLTest
{
    public class Shader
    {
        private readonly int handle;

        public Shader(ShaderType type, string code)
        {
            this.handle = GL.CreateShader(type);

            GL.ShaderSource(this.handle, code);
            GL.CompileShader(this.handle);
        }

        #region Operators
        public static implicit operator int(Shader shader)
        {
            return shader.handle;
        }
        #endregion
    }
}