using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using OpenTK;

namespace BrokenEngine.Models
{
    public class Mesh
    {

        public string Name;
        public string Comments;

        [XmlIgnore]
        public Vertex[] Vertices;
        [XmlIgnore]
        public Face[] Faces;

        [XmlIgnore]
        public List<Submesh> Submeshes = new List<Submesh>();

        public Mesh(string name, int vertices, int faces) : this (new Vertex[vertices], new Face[faces])
        {
            Name = name;
        }

        public Mesh(Vertex[] vertices, Face[] faces)
        {
            Vertices = vertices;
            Faces = faces;
        }

        public void RecalculateNormals()
        {
            // reset all normals
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].Normal = Vector3.Zero;
            }

            // calculate face normals; add normals to vertices
            var list = from submesh in Submeshes from face in submesh.Faces.Union(Faces) select face;
            foreach (var face in list)
            {
                var vertexA = Vertices[face.Indices[0]].Position;
                var vertexB = Vertices[face.Indices[1]].Position;
                var vertexC = Vertices[face.Indices[2]].Position;

                var faceNormal = Vector3.Cross(vertexB - vertexA, vertexC - vertexA);
                
                for (int i = 0; i < 3; i++)
                {
                    Vertices[face.Indices[i]].Normal += faceNormal;
                }
            }

            // normalize normals
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].Normal.Normalize();
            }
        }

    }

    // Submeshes are using the vertices from the main mesh, but they have their own face definitions. Also they will get their own gameobject.
    public class Submesh
    {
        public string Name;
        public string Comments;

        public Face[] Faces;

        public Submesh(Face[] faces)
        {
            Faces = faces;
        }
    }
}