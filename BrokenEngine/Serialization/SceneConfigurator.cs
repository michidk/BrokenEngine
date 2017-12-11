using BrokenEngine.Models;
using BrokenEngine.SceneGraph;
using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer.ExtensionModel.Types;
using ExtendedXmlSerializer.ExtensionModel.Xml;
using OpenTK;
using OpenTK.Graphics;

namespace BrokenEngine.Serialization
{
    public static class SceneConfigurator
    {

        private static IExtendedXmlSerializer serializer = null;

        public static IExtendedXmlSerializer GetSerializer()
        {
            if (serializer != null)
                return serializer;

            var container = new ConfigurationContainer();

            // define custom serializer
            //container.Type<Scene>().CustomSerializer(new SceneSerializer());
            container.Type<Mesh>().CustomSerializer(new MeshSerializer());

            // define custom names
            container.ConfigureType<Color4>().Name("Color");
            container.ConfigureType<Vector3>().Name("Position");

            // create and return serializer
            serializer = container.Create();
            return serializer;
        }

    }
}
