using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BrokenEngine.Utils.Attributes;

namespace BrokenEngine.Assets
{
    public abstract class Asset
    {

        public string Name { get; set; }

        public string FilePath { get; set; }


        [XmlConstructor]
        protected Asset()
        {
        }

        public Asset(string name, string filePath)
        {
            Name = name;
            FilePath = filePath;
        }

        public abstract void Load();

    }
}
