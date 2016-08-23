using OpenTK;
using OpenTK.Graphics;

namespace BrokenEngine.Mesh
{
    public struct Vertex
    {

        public static readonly unsafe int Size = sizeof(Vector3) + sizeof(Color4) + sizeof(Vector3) + sizeof(Vector2);

        public Vector3 Position;
        public Color4 Color;
        public Vector3 Normal;
        public Vector2 UV;

        public Vertex(Vector3 position, Color4 color = default(Color4), Vector3 normal = default(Vector3), Vector2 uv = default(Vector2))
        {
            Position = position;
            Color = color;
            Normal = normal;
            UV = uv;
        }

        public void SetNormal(Vector3 vec)
        {
            Normal = vec;
        }

        public void SetUV(Vector2 vec)
        {
            UV = vec;
        }

    }
}