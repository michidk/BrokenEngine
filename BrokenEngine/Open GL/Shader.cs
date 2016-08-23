using OpenTK.Graphics.OpenGL;

namespace BrokenEngine.Open_GL
{
    public class Shader
    {
        private readonly int handle;

        public Shader(ShaderType type, string code)
        {
            this.handle = GL.CreateShader(type);

            GL.ShaderSource(this.handle, code);
            GL.CompileShader(this.handle);

            string log = GL.GetShaderInfoLog(this.handle);
            if (!string.IsNullOrWhiteSpace(log))
                Globals.Logger.Error("Shader Error: " + log);
        }

        #region Operators
        public static implicit operator int(Shader shader)
        {
            return shader.handle;
        }
        #endregion
    }
}