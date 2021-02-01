
using BrokenEngine.Models.MeshParser;
using BrokenEngine.Utils.Attributes;

namespace BrokenEngine.Models
{
    public class Model
    {

        public string FilePath;
        public Mesh Mesh;
        private bool loaded = false;


        [XmlConstructor]
        public Model()
        {

        }

        public Model(string filePath)
        {
            this.FilePath = filePath;
        }

        public void LoadResources()
        {
            if (loaded)
                return;

            loaded = true;

            Mesh = ObjParser.ParseFile(FilePath);
        }

    }
}
