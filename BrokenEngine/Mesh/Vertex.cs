using System;
using OpenTK;
using OpenTK.Graphics;

namespace OpenGLTest
{
    public struct Vertex
    {

        public static readonly unsafe int Size = sizeof(Vector3) + sizeof(Color4);

        public Vector3 Position;
        public Color4 Color;

        public Vertex(Vector3 position)
        {
            Position = position;
            Color = default(Color4);
        }

        public Vertex(Vector3 position, Color4 color)
        {
            Position = position;
            Color = color;
        }

    }
}