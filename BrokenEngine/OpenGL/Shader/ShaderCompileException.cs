using System;

namespace BrokenEngine.OpenGL.Shader
{
    [Serializable]
    public class ShaderCompileException : Exception
    {

        public ShaderCompileException(CompiledShader shader, string log) : base($"ShaderCompiler Error ({shader.Type}):{Environment.NewLine}{log}")
        {

        }

    }
}