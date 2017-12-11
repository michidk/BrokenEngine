using System;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using BrokenEngine.Models;
using BrokenEngine.Models.MeshParser;
using BrokenEngine.SceneGraph;
using ExtendedXmlSerializer.ExtensionModel.Xml;

namespace BrokenEngine.Serialization
{
    public class ModelSerializer : IExtendedXmlCustomSerializer<Model>
    {

        public Model Deserialize(XElement xElement)
        {
            if (xElement.HasElements)
            {
                var name = xElement.Member("Name");
                var meshFile = xElement.Member("Mesh");

                if (name == null)
                    throw new SerializationException("No Name");
                
                if (meshFile == null)
                    throw new SerializationException("No Mesh");

                return new Model(name.Value, meshFile.Value);
            }

            throw new SerializationException("Empty Model");
        }

        public void Serializer(XmlWriter xmlWriter, Model model)
        {
            xmlWriter.WriteElementString("Name", model.Name);
            xmlWriter.WriteElementString("Mesh", model.meshFile);
        }

    }
}