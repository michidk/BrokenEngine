using System;
using System.Xml;
using System.Xml.Linq;
using BrokenEngine.SceneGraph;
using ExtendedXmlSerializer.ExtensionModel.Xml;

namespace BrokenEngine.Serialization
{
    public class SceneSerializer : IExtendedXmlCustomSerializer<Scene>
    {

        public Scene Deserialize(XElement xElement)
        {
            Globals.Logger.Error("Deserialize");
            throw new NotImplementedException();
        }

        public void Serializer(XmlWriter xmlWriter, Scene obj)
        {
            Globals.Logger.Error("Serialize");
            throw new NotImplementedException();
        }

    }
}