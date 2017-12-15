using BrokenEngine.Assets;
using BrokenEngine.Components;
using BrokenEngine.Materials;
using BrokenEngine.Models;
using BrokenEngine.SceneGraph;
using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer.ExtensionModel.Content;
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
            //container.Type<Scene>().CustomSerializer(new ComponentSerializer());
            container.Type<Model>().CustomSerializer(new ModelSerializer());

            // define custom names
            container.ConfigureType<Color4>().Name("Color");
            container.ConfigureType<Vector3>().Name("Position");

            // define namespaces
            container.EnableImplicitTyping(typeof(Scene), typeof(Model), typeof(Material), typeof(Mesh), typeof(Shader), typeof(GameObject));

            container.UseOptimizedNamespaces(); // namespaces get placed at the root of the document
            container.EnableAllConstructors();  // enables use of private constructors

            // references
            //container.ConfigureType<Asset>().EnableReferences(asset => asset.Name);

            // call parameterized constructors instead of empty ones
            container.EnableParameterizedContent();

            // create and return serializer
            serializer = container.Create();
            return serializer;
        }

    }
}
