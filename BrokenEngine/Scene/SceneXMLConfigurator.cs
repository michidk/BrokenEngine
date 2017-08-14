using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer.ExtensionModel.Types;
using ExtendedXmlSerializer.ExtensionModel.Xml;
using OpenTK;
using OpenTK.Graphics;

namespace BrokenEngine.Scene
{
    public static class SceneXMLConfigurator
    {

        private static IExtendedXmlSerializer serializer = null;

        public static IExtendedXmlSerializer GetSerializer()
        {
            if (serializer != null)
                return serializer;

            var container = new ConfigurationContainer();

            // define common types, so we don't have to use xml namespaces; exmls will add functionality to add whole namespaces as default at once.
            container.ConfigureType<Scene>().Name("Scene");
            container.ConfigureType<GameObject>().Name("GameObject");

            // define custom names
            container.ConfigureType<Color4>().Name("Color");
            container.ConfigureType<Vector3>().Name("Position");

            // create and return serializer
            serializer = container.Create();
            return serializer;
        }

    }
}
