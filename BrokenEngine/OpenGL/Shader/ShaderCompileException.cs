using System;

namespace BrokenEngine.OpenGL.Shader
{
    [Serializable]
    public class ShaderCompileException : Exception
    {

        public ShaderCompileException(CompiledShader shader, string log) : base($"Shader Error ({shader.Type}):{Environment.NewLine}{log}")
        {

        }

    }
}