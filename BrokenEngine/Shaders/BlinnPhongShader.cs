using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace BrokenEngine.Materials
{
    public class BlinnPhongShader : GenericPhongShader
    {

        public bool Blinn { get; set; }     // use blinn highlights?


        public BlinnPhongShader(Color4 albedoColor, Vector3 lightDirection, Color4 ambientColor, bool blinn = true) : base(albedoColor, lightDirection, ambientColor, "phong.glsl")
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