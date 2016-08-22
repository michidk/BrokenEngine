using System.Collections.Generic;
using OpenTK;

namespace OpenGLTest
{
    public class Mesh
    {

        public string Name;
        public string Comments;

        public readonly List<Vertex> Vertices = new List<Vertex>();
        public readonly List<Vector3> Normals = new List<Vector3>();
        public readonly List<Vector2> UVs = new List<Vector2>(); 

        public readonly List<FaceGroup> FaceGroups = new List<FaceGroup>(); 

        public Mesh()
        {
            
        }

        public Mesh(string name)
        {
            Name = name;
        }

    }
}