using OpenTK.Graphics.OpenGL4;

namespace BrokenEngine.Open_GL
{
    public class CompiledShader
    {

        private readonly ShaderType type;
        private readonly string code;
        private readonly int handle;
        
        public CompiledShader(ShaderType type, string code, bool compile = true)
        {
            this.type = type;
            this.code = code;
            this.handle = GL.CreateShader(type);

            GL.ShaderSource(this.handle, code);

            if (compile)
                Compile();
        }

        public bool Compile()
        {
            GL.CompileShader(this.handle);

            string log = GL.GetShaderInfoLog(this.handle);
            if (!string.IsNullOrEmpty(log))
            {
                Globals.Logger.Error($"Shader Error ({type}): {log}");
                return false;
            }

            return true;
        }

        #region Operators
        public static implicit operator int(CompiledShader shader)
        {
            return shader.handle;
        }
        #endregion

    }
}