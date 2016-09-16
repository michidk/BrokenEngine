using System;
using System.Runtime.Serialization;

namespace BrokenEngine.Open_GL
{
    [Serializable]
    public class ShaderCompileException : Exception
    {

        public ShaderCompileException(Shader shader, string log) : base($"Shader Error ({shader.Type}):{Environment.NewLine}{log}")
        {

        }

    }
}