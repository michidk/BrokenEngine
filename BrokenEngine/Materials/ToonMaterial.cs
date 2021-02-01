using OpenTK;
using OpenTK.Mathematics;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using BrokenEngine.Utils.Attributes;

namespace BrokenEngine.Materials
{
    public class ToonMaterial : GenericPhongMaterial
    {

        public float Shades { get; set; }
        public float OutlineThickness { get; set; }


        [XmlConstructor]
        protected ToonMaterial() {

        }

        public ToonMaterial(string shaderFileName, Color4 albedoColor, Vector3 lightDirection, Color4 ambientColor, float shades = 5.0f, float outlineThickness = 0.25f) : base(shaderFileName, albedoColor, lightDirection, ambientColor)
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
