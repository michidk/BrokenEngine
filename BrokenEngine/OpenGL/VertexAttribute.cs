using OpenTK.Graphics.OpenGL;

namespace BrokenEngine.OpenGL
{
    public struct VertexAttribute
    {
        public readonly string Name;
        public readonly int Size;
        public readonly VertexAttribPointerType Type;
        public readonly bool Normalize;
        public readonly int Stride;
        public readonly int Offset;

        public VertexAttribute(string name, int size, VertexAttribPointerType type, int stride, int offset, bool normalize = false)
        {
            this.Name = name;
            this.Size = size;
            this.Type = type;
            this.Stride = stride;
            this.Offset = offset;
            this.Normalize = normalize;
        }
    }
}