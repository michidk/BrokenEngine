using System.Xml.Serialization;
using BrokenEngine.Assets;
using BrokenEngine.Models.MeshParser;
using BrokenEngine.Utils.Attributes;

namespace BrokenEngine.Models
{
    public class Model : Asset
    {

        [XmlIgnore]
        public Mesh Mesh { get; set; }

        internal string meshFile;


        [XmlConstructor]
        private Model()
        {
            
        }

        public Model(Mesh mesh)
        {
            Mesh = mesh;
        }

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