using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using BrokenEngine.Assets;
using BrokenEngine.Components;
using BrokenEngine.Models;

namespace BrokenEngine.Serialization
{
    [XmlRoot("MeshRenderer")]
    public class SerialiableMeshRenderer : MeshRenderer
    {

        [XmlElement("Model")]
        public string ModelIndentifier { get; }
        [XmlElement("Material")]
        public string MaterialIdentifier { get; }


        public SerialiableMeshRenderer(string model, string material)
        {
            this.ModelIndentifier = model;
            this.MaterialIdentifier = material;
        }

        public override void OnInitialize()
        {
            this.Model = Globals.Engine.CurrentScene.Assets.Find(p => p.GetType() == typeof(Model) && p.Name.Equals(ModelIndentifier)) as Model;
            this.Material = Globals.Engine.CurrentScene.Assets.Find(p => p.GetType() == typeof(Material) && p.Name.Equals(MaterialIdentifier)) as Material;

            if (Model == null)
                Globals.Logger.Error("Can't find model " + ModelIndentifier);

            if (Material == null)
                Globals.Logger.Error("Can't find material " + MaterialIdentifier);
        }
    }
}
