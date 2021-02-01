using OpenTK;
using OpenTK.Mathematics;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using BrokenEngine.Utils.Attributes;

namespace BrokenEngine.Materials
{
    public class BlinnPhongMaterial : GenericPhongMaterial
    {

        public bool Blinn { get; set; }     // use blinn highlights?


        [XmlConstructor]
        protected BlinnPhongMaterial() {

        }

        public BlinnPhongMaterial(string shaderFileName, Color4 albedoColor, Vector3 lightDirection, Color4 ambientColor, bool blinn = true) : base(shaderFileName, albedoColor, lightDirection, ambientColor)
        {
            this.Blinn = blinn;
        }

        public override void Apply()
        {
            base.Apply();

            SetBoolUniform("u_blinn", Blinn);
        }

    }
}
