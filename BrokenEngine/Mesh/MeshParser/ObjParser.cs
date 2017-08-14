using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenTK;
using OpenTK.Graphics;

namespace BrokenEngine.Mesh.MeshParser
{
    public class ObjParser : IMeshParser<Mesh>
    {

        struct ObjVertex
        {
            public Vector3 Position;
            public Vector3 Color;
        }

        struct ObjFaceVertex
        {
            public ushort VertexIndex;
            public ushort NormalIndex;
            public ushort UVIndex;
        }

        struct ObjFace
        {
            public ObjFaceVertex[] Vertices;

            public ObjFace(int count)
            {
                Vertices = new ObjFaceVertex[count];
            }
        }

        class ObjFaceGroup
        {
            public string Name;
            public string Comments;
            public List<ObjFace> Faces = new List<ObjFace>();
        }

        private const string FILE_EXTENSION = ".obj";

        public string FileName { private set; get; }
        public Mesh Mesh { private set; get; }

        private string name;
        private string comments;
        private string mtllib;
        private List<ObjVertex> vertices = new List<ObjVertex>(); 
        private List<Vector3> normals = new List<Vector3>(); 
        private List<Vector2> uvs = new List<Vector2>(); 
        private List<ObjFaceGroup> faceGroups = new List<ObjFaceGroup>(); 

        private bool headerEnded = false;
        private int currentLine = 0;
        private ObjFaceGroup currentFaceGroup;

        public ObjParser(string name)
        {
            FileName = name;
        }

        public static Mesh ParseFile(string name)
        {
            var parser = new ObjParser(name);
            return parser.GetMesh();
        }

        public Mesh GetMesh()
        {
            if (Mesh != null)
                return Mesh;

            Parse();
            Mesh = Construct();

            return Mesh;
        }

        private void Parse()
        {
            currentFaceGroup = new ObjFaceGroup() { Name = "default", Comments = "all ungrouped elements are in here" };
            faceGroups.Add(currentFaceGroup);

            // read file & process line by line
            byte[] data = ResourceManager.GetBytes(FileName + FILE_EXTENSION);
            if (data == null || data.Length == 0)
                throw new FileLoadException($"No data found while parsing {FileName}");

            using (Stream stream = new MemoryStream(data))
            using (StreamReader reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        currentLine++;
                        ProcessLine(line);
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(name))
                name = FileName;
        }

        private Mesh Construct()
        {
            Mesh mesh = new Mesh(new Vertex[vertices.Count], new Face[0]) { Name = name, Comments = comments };

            // add vertex position & color
            for (int i = 0; i < vertices.Count; i++)
            {
                var vertex = vertices[i];
                var position = vertex.Position;
                var color = new Color4(vertex.Color.X, vertex.Color.Y, vertex.Color.Z, 1);

                mesh.Vertices[i] = new Vertex(position, color);
            }

            // facegroup (obj) = submesh (BE)
            foreach (var faceGroup in faceGroups)
            {
                var submesh = new Submesh(new Face[faceGroup.Faces.Count]) { Name = faceGroup.Name, Comments = faceGroup.Comments };

                for (int i = 0; i < faceGroup.Faces.Count; i++)
                {
                    // convert faces
                    var objFace = faceGroup.Faces[i];
                    submesh.Faces[i] = new Face(3);

                    for (int v = 0; v < 3; v++)
                    {
                        ObjFaceVertex faceVertex = objFace.Vertices[v];
                        ushort vertexId = faceVertex.VertexIndex;
                        submesh.Faces[i].Indices[v] = vertexId;

                        // add missing (indexed / referenced) data to vertex
                        if (normals.Count > 0)
                            mesh.Vertices[vertexId].Normal = normals[faceVertex.NormalIndex];
                        if (uvs.Count > 0)
                            mesh.Vertices[vertexId].UV = uvs[faceVertex.UVIndex];
                    }
                }

                mesh.Submeshes.Add(submesh);
            }

            return mesh;
        }

        delegate void ParseLine(string[] data);
        private void ProcessLine(string line)
        {
            string[] split = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string type = split[0];

            ParseLine method = null;
            switch (type)
            {
                case "#":
                    method = ParseHeader;
                    break;
                case "mtllib":
                    Globals.Logger.Warn($"Model { FileName } uses .mtl materials which are not supported.");
                    break;
                case "o":
                    method = ParseName;
                    headerEnded = true;
                    break;
                case "v":
                    method = ParseVertex;
                    headerEnded = true;
                    break;
                case "vn":
                    method = ParseNormal;
                    headerEnded = true;
                    break;
                case "vt":
                    method = ParseUV;
                    headerEnded = true;
                    break;
                case "g":
                    method = ParseGroup;
                    headerEnded = true;
                    break;
                case "usemtl":
                    // we dont support materials
                    break;
                case "s":
                    // we dont need smoothing groups, as this data is saved in the normals (its used to calculate them)
                    break;
                case "f":
                    method = ParseFace;
                    headerEnded = true;
                    break;
            }

            method?.Invoke(split);
        }

        private void ParseHeader(string[] data)
        {
            string comment = Concat(data, 1) + Environment.NewLine;
            if (!headerEnded)
                comments += comment;
        }

        private void ParseName(string[] data)
        {
            name = Concat(data, 1);
        }

        private void ParseVertex(string[] data)
        {
            var vert = new ObjVertex();
            vert.Position = ParseVector3(data, 1);
            if (data.Length >= 7)
                vert.Color = ParseVector3(data, 4);

            vertices.Add(vert);
        }

        private void ParseNormal(string[] data)
        {
            normals.Add(ParseVector3(data, 1));
        }

        private void ParseUV(string[] data)
        {
            uvs.Add(ParseVector2(data, 1));
        }

        private void ParseGroup(string[] data)
        {
            currentFaceGroup = new ObjFaceGroup();
            currentFaceGroup.Name = Concat(data, 1);
            faceGroups.Add(currentFaceGroup);
        }

        private void ParseFace(string[] data)
        {
            int length = data.Length - 1;

            var face = new ObjFace(3);
            currentFaceGroup.Faces.Add(face);

            for (int i = 0; i < length; i++)
            {
                string[] parts = data[i + 1].Split('/');

                var vert = new ObjFaceVertex();

                vert.VertexIndex = ParseIndex(parts, 0);

                if (parts.Length > 1 && !string.IsNullOrEmpty(parts[1]))
                    vert.UVIndex = ParseIndex(parts, 1);

                if (parts.Length > 2 && !string.IsNullOrEmpty(parts[2]))
                    vert.NormalIndex = ParseIndex(parts, 2);

                if (i < 3)
                {
                    face.Vertices[i] = vert;
                }
                else
                {   
                    // polygon > 3 vertices -> triangulate: add new face for every additional polygon
                    var newFace = new ObjFace(3);

                    newFace.Vertices[0] = face.Vertices[0];
                    newFace.Vertices[1] = face.Vertices[2];
                    newFace.Vertices[2] = vert;

                    face = newFace;
                    currentFaceGroup.Faces.Add(face);
                }
            }
        }

        private ushort ParseIndex(string[] data, int idx)
        {
            bool success = true;
            ushort result;

            if (!ushort.TryParse(data[idx], out result))
                success = false;

            if (!success)
                throw new InvalidDataException($"Can't read Index in { FileName } line { currentLine }");

            // obj starts counting at 1 -> subtract 1, because we start counting at 0
            try
            {
                result = Convert.ToUInt16(result - 1);
            }
            catch (OverflowException e)
            {
                throw new OverflowException($"Can't read Index in { FileName } line { currentLine }, because the index({ result }) is too small.", e);
            }

            return result;
        }

        private Vector2 ParseVector2(string[] data, int idx)
        {
            bool success = true;
            float x, y;

            if (!float.TryParse(data[idx], out x))
                success = false;
            idx++;
            if (!float.TryParse(data[idx], out y))
                success = false;

            if (!success)
                throw new InvalidDataException($"Can't read Vector2 in { FileName } line { currentLine }");

            return new Vector2(x, y);
        }

        private Vector3 ParseVector3(string[] data, int idx)
        {
            bool success = true;
            float x, y, z;

            if (!float.TryParse(data[idx], out x))
                success = false;
            idx++;
            if (!float.TryParse(data[idx], out y))
                success = false;
            idx++;
            if (!float.TryParse(data[idx], out z))
                success = false;

            if (!success)
                throw new InvalidDataException($"Can't read Vector3 in { FileName } line { currentLine }");

            return new Vector3(x, y, z);
        }

        private static string Concat(string[] split, int idx)
        {
            StringBuilder result = new StringBuilder(split.Length - 1);
            for (int i = idx; i < split.Length; i++)
            {
                result.Append(split[i]);
                if (i + 1 < split.Length)
                    result.Append(" ");
            }
            return result.ToString();
        }
         
    }
}