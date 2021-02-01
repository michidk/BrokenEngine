using BrokenEngine.Utils.Attributes;
using OpenTK;
using OpenTK.Mathematics;

namespace BrokenEngine.Materials
{

    public class VertexColorMaterial : Material
    {

        public Vector3 Test { get; set; }


        [XmlConstructor]
        protected VertexColorMaterial()
        {
        }

        public VertexColorMaterial(string shaderFilePath) : base(shaderFilePath)
        {

        }

    }

}
