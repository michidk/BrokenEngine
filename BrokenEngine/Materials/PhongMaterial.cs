using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace BrokenEngine.Materials
{
    public class PhongMaterial : Material
    {

        public PhongMaterial(Color4 color) : base("phong")
        {
            Parameters.Color = color;
            Parameters.LightDir = new Vector3(1,1,1);
            Parameters.AmbientColor = Color4.AliceBlue;
        }

        public override void Apply()
        {
            base.Apply();

            SetValueUniform("u_albedo", (Color4) Parameters.Color, GL.Uniform4);
            SetValueUniform("u_lightDirection", (Vector3) Parameters.LightDir, GL.Uniform3);
            SetValueUniform("u_ambientColor", (Color4) Parameters.AmbientColor, GL.Uniform4);
        }

    }
}