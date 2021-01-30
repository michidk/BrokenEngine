using OpenTK;
using OpenTK.Mathematics;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace BrokenEngine.Materials
{
    public class BlinnPhongShader : GenericPhongShader
    {

        public bool Blinn { get; set; }     // use blinn highlights?


        public BlinnPhongShader(string shaderFileName, Color4 albedoColor, Vector3 lightDirection, Color4 ambientColor, bool blinn = true) : base(shaderFileName, albedoColor, lightDirection, ambientColor)
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
