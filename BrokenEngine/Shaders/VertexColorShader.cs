using OpenTK;
using OpenTK.Mathematics;

namespace BrokenEngine.Materials
{

    public class VertexColorShader : Shader
    {

        public Vector3 Test { get; set; }


        public VertexColorShader()
        {
        }

        public VertexColorShader(string shaderFilePath) : base(shaderFilePath)
        {

        }

    }

}
