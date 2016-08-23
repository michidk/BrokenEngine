using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace BrokenEngine.Materials
{
    public class PhongMaterial : Material
    {

        // parameters
        public Color4 Color;

        public PhongMaterial(Color4 color) : base("phong")
        {
            Color = color;
        }

        public override void Apply()
        {
            base.Apply();

            SetValueUniform("u_albedo", Color, GL.Uniform4);
        }
    }
}