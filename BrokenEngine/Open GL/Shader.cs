using System;
using OpenTK.Graphics.OpenGL;

namespace BrokenEngine.Open_GL
{
    public class Shader
    {

        private readonly int handle;

        public ShaderType Type;
        public string Source;

        public Shader(ShaderType type, string source)
        {
            this.Type = type;
            this.Source = source;

            this.handle = GL.CreateShader(type);
        }

        /// <exception cref="ShaderCompileException"></exception>
        public void CompileShader()
        {
            GL.ShaderSource(this.handle, Source);
            GL.CompileShader(this.handle);

            string log = GL.GetShaderInfoLog(this.handle);
            if (!string.IsNullOrWhiteSpace(log))
                throw new ShaderCompileException(this, log);
        }

        #region Operators
        public static implicit operator int(Shader shader)
        {
            return shader.handle;
        }
        #endregion
    }
}