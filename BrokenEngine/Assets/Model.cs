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



        [XmlConstructor]
        public Model()
        {
        }

        public Model(string name, string filePath) : base(name, filePath)
        {
        }

        public override void Load()
        {
            Mesh = ObjParser.ParseFile(FilePath);
        }

    }
}