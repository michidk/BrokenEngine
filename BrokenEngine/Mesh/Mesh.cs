using System.Collections.Generic;
using OpenTK;

namespace BrokenEngine.Mesh
{
    public class Mesh
    {

        public string Name;
        public string Comments;

        public readonly List<Vertex> Vertices = new List<Vertex>();

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