using System;
using System.Xml;
using System.Xml.Linq;
using BrokenEngine.Models;
using BrokenEngine.SceneGraph;
using ExtendedXmlSerializer.ExtensionModel.Xml;

namespace BrokenEngine.Serialization
{
    public class MeshSerializer : IExtendedXmlCustomSerializer<Mesh>
    {

        public Mesh Deserialize(XElement xElement)
        {
            if (xElement.HasElements)
            {
                var type = xElement.Member("String");
                Console.WriteLine("test");
            }

            throw new NotImplementedException();
        }

        public void Serializer(XmlWriter xmlWriter, Mesh obj)
        {
            //throw new NotImplementedException();
        }

    }
}