namespace BrokenEngine.Mesh
{
    public struct Face
    {
        public ushort[] Indices;

        public Face(ushort[] indices)
        {
            Indices = indices;
        }

        public Face(ushort i1, ushort i2, ushort i3) : this(new []{ i1, i2, i3 })
        {
        }

    }
}