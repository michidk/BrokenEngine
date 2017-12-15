using System.Xml.Serialization;
using BrokenEngine.Assets;
using BrokenEngine.Models.MeshParser;
using BrokenEngine.Utils.Attributes;

namespace BrokenEngine.Models
{
    public class Model : Asset
    {

        public Mesh Mesh { get; set; }

        internal string meshFile;


        public Model(string name, Mesh mesh) : base(name)
        {
            Mesh = mesh;
        }

        public Model(string name, string meshFile) : base(name)
        {
            Mesh = ObjParser.ParseFile(meshFile);
        }

    }
}