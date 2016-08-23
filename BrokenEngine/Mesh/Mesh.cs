using System;
using System.Collections.Generic;
using OpenTK;

namespace BrokenEngine.Mesh
{
    public class Mesh
    {

        public string Name;
        public string Comments;

        public Vertex[] Vertices;
        public Face[] Faces;

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