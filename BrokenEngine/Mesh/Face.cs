using System.Text;

namespace OpenGLTest
{
    public class Face
    {

        public struct FaceVertex
        {
            public ushort VertexIndex;
            public ushort UVIndex;
            public ushort NormalIndex;

            public FaceVertex(ushort vertexIndex, ushort uvIndex = default(ushort), ushort normalIndex = default(ushort))
            {
                VertexIndex = vertexIndex;
                UVIndex = uvIndex;
                NormalIndex = normalIndex;
            }

            public override string ToString()
            {
                return $"VertexIndex: {VertexIndex}, UvIndex: {UVIndex}, NormalIndex: {NormalIndex}";
            }
        }

        public FaceVertex[] Vertices;
        
        public FaceVertex this[int idx]
        {
            get { return Vertices[idx]; }
            set { Vertices[idx] = value; }
        }


        public Face(FaceVertex[] Vertices)
        {
            this.Vertices = Vertices;
        }

        public Face() : this(new FaceVertex[3])
        {

        }

        public Face(ushort vert1, ushort vert2, ushort vert3) : this (new[] { new Face.FaceVertex(vert1), new Face.FaceVertex(vert2), new Face.FaceVertex(vert3) })
        {
            
        }

        public override string ToString()
        {
            StringBuilder vertices = new StringBuilder();
            StringBuilder uvs = new StringBuilder();
            StringBuilder normals = new StringBuilder();
            foreach (var vert in Vertices)
            {
                vertices.Append(vert.VertexIndex);
                vertices.Append(", ");
                uvs.Append(vert.UVIndex);
                uvs.Append(", ");
                normals.Append(vert.NormalIndex);
                normals.Append(", ");
            }
            return $"Vertices: {vertices} UVs: {uvs} Normals: {normals}";
        }
    }
}