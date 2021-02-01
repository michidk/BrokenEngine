using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using BrokenEngine.Components;
using BrokenEngine.Materials;
using BrokenEngine.Models;
using BrokenEngine.Models.MeshParser;
using BrokenEngine.SceneGraph;
using OpenTK;
using OpenTK.Mathematics;

namespace BrokenEngine.Serilization
{
    public static class SceneParser
    {

        private static Type[] componentTypes;
        private static Type[] shaderTypes;
        private static Type[] extraTypes;


        static SceneParser()
        {
            var shaders = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(Material).IsAssignableFrom(p));

            var components = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(Component).IsAssignableFrom(p));

            shaderTypes = shaders.ToArray<Type>();
            componentTypes = components.ToArray<Type>();
            extraTypes = new List<Type>().Concat(shaders).ToArray<Type>();
        }

        public static Scene Read(String input)
        {
            var scene = new Scene();

            var xml = new XmlDocument();
            xml.LoadXml(input);

            var root = xml["Scene"];
            scene.Meta = (Scene.MetaData)DeserializeToObject<Scene.MetaData>(root?["MetaData"]);

            var graph = root["SceneGraph"].ChildNodes;
            foreach (XmlNode elem in graph)
            {
                DeserializeGameObject(xml, elem, scene.SceneRoot);
            }

            return scene;
        }

        public static String Write(Scene scene)
        {
            var xml = new XmlDocument();

            // xml declaration
            XmlDeclaration xmlDeclaration = xml.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement rootElem = xml.DocumentElement;
            xml.InsertBefore(xmlDeclaration, rootElem);

            var root = xml.CreateElement("Scene");
            xml.AppendChild(root);

            // meta
            root.AppendChild(xml.ImportNode(SerializeToXmlDocument(scene.Meta), true));

            // scene graph
            var graph = xml.CreateElement("SceneGraph");
            foreach (var go in scene.SceneRoot)
            {
                graph.AppendChild(SerializeGameObject(xml, go));
            }

            root.AppendChild(graph);

            // print
            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.NewLineOnAttributes = true;

            StringBuilder sb = new StringBuilder();
            using (var xmlWriter = XmlWriter.Create(sb, settings))
            {
                xml.Save(xmlWriter);
            }

            return sb.ToString();
        }

        private static XmlElement SerializeGameObject(XmlDocument xml, GameObject go)
        {
            var elem = xml.CreateElement("GameObject");
            elem.AppendChild(CreateText(xml, "Name", go.Name));
            elem.AppendChild(CreateObject(xml, "Position", go.LocalPosition));
            elem.AppendChild(CreateObject(xml, "Rotation", go.LocalEulerRotation));
            elem.AppendChild(CreateObject(xml, "Scale", go.LocalScale));

            var components = xml.CreateElement("Components");
            foreach (var comp in go.Components)
            {
                var xmlComp = xml.ImportNode(SerializeToXmlDocument(comp), true);
                components.AppendChild(xmlComp);
            }

            elem.AppendChild(components);

            var xmlChild = xml.CreateElement("Children");
            foreach (var child in go.Children)
            {
                xmlChild.AppendChild(SerializeGameObject(xml, child));

            }

            elem.AppendChild(xmlChild);

            return elem;
        }

        private static GameObject DeserializeGameObject(XmlDocument doc, XmlNode xml, GameObject parent = null)
        {
            string name = xml["Name"]?.InnerText;
            var position = (Vector3)DeserializeToObject<Vector3>(xml["Position"]);
            var rotation = (Vector3)DeserializeToObject<Vector3>(xml["Rotation"]);
            var scale = (Vector3)DeserializeToObject<Vector3>(xml["Scale"]);
            GameObject go = new GameObject(name, position, parent);
            go.LocalEulerRotation = rotation;
            go.LocalScale = scale;

            foreach (XmlNode component in xml["Components"].ChildNodes)
            {
                foreach (Type type in componentTypes)
                {
                    if (component.Name.Equals(type.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        if (DeserializeToObject(type, component) is Component comp)
                        {
                            go.AddComponent(comp);
                        }
                    }
                }
            }

            // children
            foreach (XmlNode child in xml["Children"].ChildNodes)
            {
                DeserializeGameObject(doc, child, go);
            }

            return go;
        }

        private static object DeserializeToObject<T>(XmlNode xml)
        {
            return DeserializeToObject(typeof(T), xml);
        }

        private static object DeserializeToObject(Type type, XmlNode xml)
        {
            XmlSerializer serializer = new XmlSerializer(type, null, extraTypes, new XmlRootAttribute(xml.Name), null);
            using (XmlReader reader = new XmlNodeReader(xml))
            {
                return serializer.Deserialize(reader);
            }
        }

        private static XmlElement SerializeToXmlDocument(object input)
        {

            var ser = new XmlSerializer(input.GetType(), extraTypes);
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            XmlDocument xd = null;

            using (var memStm = new MemoryStream())
            {
                ser.Serialize(memStm, input, ns);

                memStm.Position = 0;

                var settings = new XmlReaderSettings { IgnoreWhitespace = true };

                using (var xtr = XmlReader.Create(memStm, settings))
                {
                    xd = new XmlDocument();
                    xd.Load(xtr);
                }
            }

            return xd.DocumentElement;
        }

        private static XmlNode CreateText(XmlDocument xml, String label, String content)
        {
            var root = xml.CreateElement(label);
            root.AppendChild(xml.CreateTextNode(content));
            return root;
        }

        private static XmlNode CreateObject(XmlDocument xml, String label, object content)
        {
            var root = xml.CreateElement(label);
            var import = xml.ImportNode(SerializeToXmlDocument(content), true);

            while (import.ChildNodes.Count > 0)
                root.AppendChild(import.FirstChild);

            return root;
        }

    }
}
