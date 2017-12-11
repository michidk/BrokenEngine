namespace BrokenEngine.Models
{
    public struct Face
    {
        public ushort[] Indices;

        public Face(ushort[] indices)
        {
            Indices = indices;
        }

        public Face(int length) : this(new ushort[length])
        {
        }

        public Face(ushort i1, ushort i2, ushort i3) : this(new []{ i1, i2, i3 })
        {
        }

    }
}