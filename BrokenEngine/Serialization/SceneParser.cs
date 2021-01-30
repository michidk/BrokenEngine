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
using BrokenEngine.Assets;
using BrokenEngine.Components;
using BrokenEngine.SceneGraph;
using OpenTK;
using OpenTK.Mathematics;

namespace BrokenEngine.Serilization
{
    public static class SceneParser
    {

        public static Scene Read(String input)
        {
            var scene = new Scene();

            var xml = new XmlDocument();
            xml.LoadXml(input);

            var root = xml["Scene"];
            scene.Meta = (Scene.MetaData) DeserializeToObject<Scene.MetaData>(root?["MetaData"]);

            var assets = root["Assets"].ChildNodes;
            Dictionary<string, Asset> assetdb = new Dictionary<string, Asset>(assets.Count);

            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(Asset).IsAssignableFrom(p));

            // build assetdb
            foreach (XmlNode asset in assets)
            {
                foreach (Type type in types)
                {
                    if (asset.Name.Equals(type.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        if (DeserializeToObject(type, asset) is Asset assetObj)
                        {
                            assetdb.Add(assetObj.Name, assetObj);
                            assetObj.Load();
                        }

                    }
                }
            }

            var graph = root["SceneGraph"].ChildNodes;
            foreach (XmlNode elem in graph)
            {
                DeserializeGameObject(xml, elem, assetdb, scene.SceneRoot);
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

            // assets
            var assets = xml.CreateElement("Assets");
            root.AppendChild(assets);

            // scene graph
            var graph = xml.CreateElement("SceneGraph");
            foreach (var go in scene.SceneRoot)
            {
                graph.AppendChild(SerializeGameObject(xml, assets, go));
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

        private static XmlElement SerializeGameObject(XmlDocument xml, XmlElement assets, GameObject go)
        {
            var elem = xml.CreateElement("GameObject");
            elem.AppendChild(CreateText(xml, "Name", go.Name));
            elem.AppendChild(CreateObject(xml, "Position", go.LocalPosition));
            elem.AppendChild(CreateObject(xml, "Rotation", go.LocalEulerRotation));
            elem.AppendChild(CreateObject(xml, "Scale", go.LocalScale));

            var components = xml.CreateElement("Components");
            foreach (var comp in go.Components)
            {
                //var xmlComp = xml.CreateElement(comp.GetType().Name);
                var xmlComp = xml.ImportNode(SerializeToXmlDocument(comp), true);

                foreach (var prop in comp.GetType().GetProperties())
                {
                    var value = prop.GetValue(comp);
                    if (value is Asset)
                    {
                        var asset = value as Asset;
                        xmlComp.AppendChild(CreateText(xml, prop.Name, asset.Name));
                        assets.AppendChild(CreateObject(xml, prop.Name, asset));
                    }
                }

                components.AppendChild(xmlComp);
            }

            elem.AppendChild(components);

            var xmlChild = xml.CreateElement("Children");
            foreach (var child in go.Children)
            {
                xmlChild.AppendChild(SerializeGameObject(xml, assets, child));

            }

            elem.AppendChild(xmlChild);

            return elem;
        }

        private static GameObject DeserializeGameObject(XmlDocument doc, XmlNode xml, Dictionary<string, Asset> assetdb, GameObject parent = null)
        {
            string name = xml["Name"]?.InnerText;
            var position = (Vector3)DeserializeToObject<Vector3>(xml["Position"]);
            var rotation = (Vector3)DeserializeToObject<Vector3>(xml["Rotation"]);
            var scale = (Vector3)DeserializeToObject<Vector3>(xml["Scale"]);
            GameObject go = new GameObject(name, position, parent);
            go.LocalEulerRotation = rotation;
            go.LocalScale = scale;


            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(Component).IsAssignableFrom(p));

            foreach (XmlNode component in xml["Components"].ChildNodes)
            {
                foreach (Type type in types)
                {
                    if (component.Name.Equals(type.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        if (DeserializeToObject(type, component) is Component comp)
                        {

                            foreach (var prop in comp.GetType().GetProperties())
                            {

                                if (prop.PropertyType.BaseType == (typeof(Asset)))
                                {
                                    // get xml value, search in list for name; get reference & assign reference
                                    var id = component[prop.Name].InnerText;
                                    var reff = assetdb[id];

                                    prop.SetValue(comp, reff);
                                }
                            }

                            go.AddComponent(comp);
                        }
                    }
                }
            }

            // children
            foreach (XmlNode child in xml["Children"].ChildNodes)
            {
                DeserializeGameObject(doc, child, assetdb, go);
            }

            return go;
        }

        private static object DeserializeToObject<T>(XmlNode xml)
        {
            return DeserializeToObject(typeof(T), xml);
        }

        private static object DeserializeToObject(Type type, XmlNode xml)
        {
            XmlSerializer serializer = new XmlSerializer(type, new XmlRootAttribute(xml.Name));
            using (XmlReader reader = new XmlNodeReader(xml))
            {
                return serializer.Deserialize(reader);
            }
        }

        private static XmlElement SerializeToXmlDocument(object input)
        {
            var ser = new XmlSerializer(input.GetType());
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            XmlDocument xd = null;

            using (var memStm = new MemoryStream())
            {
                ser.Serialize(memStm, input, ns);

                memStm.Position = 0;

                var settings = new XmlReaderSettings {IgnoreWhitespace = true};

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
