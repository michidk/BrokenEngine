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
        
        public Shader Shader { get; set; }


        public Material(string name, Shader shader) : base(name)
        {
            Shader = shader;
        }
        
    }
}
