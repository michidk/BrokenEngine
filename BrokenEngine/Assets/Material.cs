using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BrokenEngine.Materials;
using BrokenEngine.Models;
using BrokenEngine.Utils.Attributes;

namespace BrokenEngine.Assets
{
    public class Material : Asset
    {

        public string Type { get; set; }

        public Shader Shader { get; set; }


        [XmlConstructor]
        public Material()
        {
        }

        public Material(string name, string filePath, Shader shader) : base(name, filePath)
        {
            //Type = type;
            Shader = shader;
        }

        public override void Load()
        {
            /*
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(Shader).IsAssignableFrom(p));

            foreach (var type in types)
            {
                if (type.Name.Equals(Type))
                {
                    Shader = (Shader) Activator.CreateInstance(type, FilePath);
                    // Shader?.LoadResources(); -> moved to MeshRenderer.OnInitialize
                }
            }
            */
            // TODO: load shader file
        }

    }
}
