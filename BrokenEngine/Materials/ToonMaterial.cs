using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace BrokenEngine.Materials
{
    public class ToonMaterial : GenericPhongMaterial
    {

        public float Shades { get; set; }
        public float OutlineThickness { get; set; }


        public ToonMaterial()
        {
            
        }

        public ToonMaterial(Color4 albedoColor, Vector3 lightDirection, Color4 ambientColor, float shades = 5.0f, float outlineThickness = 0.25f) : base(albedoColor, lightDirection, ambientColor, "toon.glsl")
        {
            this.Shades = shades;
            this.OutlineThickness = outlineThickness;
        }

        public override void Apply()
        {
            base.Apply();

            SetValueUniform("u_shades", Shades, GL.Uniform1);
            SetValueUniform("u_outlineThickness", Shades, GL.Uniform1);
        }

    }
}