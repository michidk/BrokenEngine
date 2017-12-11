using System.Xml.Serialization;
using BrokenEngine.Models.MeshParser;

namespace BrokenEngine.Models
{
    public class Model
    {

        public string Name { get; set; }

        [XmlIgnore]
        public Mesh Mesh { get; set; }

        internal string meshFile;

        //XML
        public Model()
        {
        }

        public Model(Mesh mesh)
        {
            Mesh = mesh;
        }

        public Model(string name, Mesh mesh)
        {
            Name = name;
            Mesh = mesh;
        }

        public Model(string name, string meshFile)
        {
            Name = name;

            Mesh = ObjParser.ParseFile(meshFile);
        }

    }
}