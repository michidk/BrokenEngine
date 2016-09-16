using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace BrokenEngine.Materials
{
    public class ToonMaterial : Material
    {

        public Color4 AlbedoColor { get; set; }
        public Vector3 LightDirection { get; set; }
        public Color4 AmbientColor { get; set; }
        public float Shades { get; set; }


        public ToonMaterial(Color4 albedoColor, Vector3 lightDirection, Color4 ambientColor, float shades = 5) : base("toon")
        {
            AlbedoColor = albedoColor;
            LightDirection = lightDirection;
            AmbientColor = ambientColor;
            Shades = shades;
        }

        public override void Apply()
        {
            base.Apply();

            SetValueUniform("u_albedoColor", AlbedoColor, GL.Uniform4);
            SetValueUniform("u_lightDirection", LightDirection, GL.Uniform3);
            SetValueUniform("u_ambientColor", AmbientColor, GL.Uniform4);
            SetValueUniform("u_shades", Shades, GL.Uniform1);
        }

    }
}