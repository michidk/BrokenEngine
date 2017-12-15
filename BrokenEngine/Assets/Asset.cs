using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrokenEngine.Utils.Attributes;

namespace BrokenEngine.Assets
{
    public abstract class Asset
    {
        public string Name { get; set; }


        [XmlConstructor]
        protected Asset()
        {
            //AssetRegistry.Instance.Register(this);
        }

        protected Asset(string name) : base()
        {
            Name = name;
        }
    }
}
