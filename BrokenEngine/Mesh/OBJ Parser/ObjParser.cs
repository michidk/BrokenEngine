using System;
using System.IO;
using System.Text;
using OpenTK;
using OpenTK.Graphics;

namespace OpenGLTest.OBJ_Parser
{
    public class ObjParser
    {

        public static ObjMesh ParseFile(byte[] file)
        {
            ObjMesh mesh = new ObjMesh();

            bool header = true;
            FaceGroup currentFaceGroup = new FaceGroup("default");
            mesh.FaceGroups.Add(currentFaceGroup);

            using (Stream stream = new MemoryStream(file))
            using (StreamReader reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    if (!string.IsNullOrWhiteSpace(line))
                        ProcessLine(mesh, line, ref currentFaceGroup, ref header);
            }

            return mesh;
        }

        private static void ProcessLine(ObjMesh mesh, string line, ref FaceGroup currentFaceGroup, ref bool header)
        {
            string[] split = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string type = split[0];

            switch (type)
            {
                case "#":
                    string comment = Concat(split, 1) + Environment.NewLine;
                    if (header)
                        mesh.Comments += comment;
                    else
                        currentFaceGroup.Comments += comment;
                    break;
                case "mtllib":
                    mesh.Mtllib = split[1];
                    break;
                case "o":
                    mesh.Name = Concat(split, 1);
                    break;
                case "v":
                    var vert = new Vertex();
                    vert.Position = new Vector3(float.Parse(split[1]), float.Parse(split[2]), float.Parse(split[3]));
                    if (split.Length >= 7)
                        vert.Color = new Color4(float.Parse(split[4]), float.Parse(split[5]), float.Parse(split[6]), 1);
                    mesh.Vertices.Add(vert);
                    header = false;
                    break;
                case "vn":
                    var normal = new Vector3(float.Parse(split[1]), float.Parse(split[2]), float.Parse(split[3]));
                    mesh.Normals.Add(normal);
                    header = false;
                    break;
                case "vt":
                    var uv = new Vector2(float.Parse(split[1]), float.Parse(split[2]));
                    mesh.UVs.Add(uv);
                    header = false;
                    break;
                case "g":
                    currentFaceGroup = new FaceGroup(Concat(split, 1));
                    mesh.FaceGroups.Add(currentFaceGroup);
                    header = false;
                    break;
                case "usemtl":
                    currentFaceGroup.Material = Concat(split, 1);
                    break;
                case "s":
                    // we dont need smoothing groups, as this data is saved in the normals (its used to calculate them)
                    break;
                case "f":
                    int length = split.Length - 1;
                    var face = new Face();
                    currentFaceGroup.Faces.Add(face);
                    for (int i = 0; i < length; i++)
                    {
                        var fvert = new Face.FaceVertex();
                        string[] parts = split[i + 1].Split('/');
                        fvert.VertexIndex = Convert.ToUInt16(ushort.Parse(parts[0]) - 1);

                        if (parts.Length > 1 && !string.IsNullOrEmpty(parts[1]))
                            fvert.UVIndex = Convert.ToUInt16(ushort.Parse(parts[1]) - 1);

                        if (parts.Length > 2 && !string.IsNullOrEmpty(parts[2]))
                            fvert.NormalIndex = Convert.ToUInt16(ushort.Parse(parts[2]) - 1);

                        if (i < 3)
                        {
                            face[i] = fvert;
                        }
                        else
                        {   // polygon, triangulate
                            var newFace = new Face();
                            newFace[0] = face[0];
                            newFace[1] = face[2];
                            newFace[2] = fvert;
                            face = newFace;
                            currentFaceGroup.Faces.Add(face);
                        }
                    }
                    header = false;
                    break;
            }
        }

        private static string Concat(string[] split, int idx)
        {
            StringBuilder result = new StringBuilder(split.Length - 1);
            for (int i = idx; i < split.Length; i++)
            {
                result.Append(split[i]);
                result.Append(" ");
            }
            return result.ToString();
        }
         
    }
}