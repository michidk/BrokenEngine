using System.Collections.Generic;

namespace BrokenEngine.Mesh
{
    public class FaceGroup
    {

        public string Name;
        public string Comments;

        public Face[] Faces;

        public FaceGroup(Face[] faces) : this (default(string), default(string), faces)
        {
        }

        public FaceGroup(string name, Face[] faces) : this(name, default(string), faces)
        {
        }

        public FaceGroup(string name, string comments, Face[] faces)
        {
            Name = name;
            Comments = comments;
            Faces = faces;
        }

    }
}