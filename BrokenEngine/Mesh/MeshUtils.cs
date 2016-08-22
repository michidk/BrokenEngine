using OpenTK;
using OpenTK.Graphics;

namespace OpenGLTest
{
    public static class MeshUtils
    {
        
        public static Mesh CreateTriangle()
        {
            var mesh = new Mesh("Triangle");

            mesh.Vertices.Add(new Vertex(new Vector3(-1, 1, 0), Color4.Lime));
            mesh.Vertices.Add(new Vertex(new Vector3(-1, -1, 0), Color4.Red));
            mesh.Vertices.Add(new Vertex(new Vector3(1, -1, 0), Color4.Blue));

            var fg = new FaceGroup("Face");
            fg.Faces.Add(new Face(0, 1, 2));
            mesh.FaceGroups.Add(fg);

            return mesh;
        }

        public static Mesh CreateQuad()
        {
            var mesh = new Mesh("Quad");

            mesh.Vertices.Add(new Vertex(new Vector3(-1, 1, 0)));
            mesh.Vertices.Add(new Vertex(new Vector3(-1, -1, 0)));
            mesh.Vertices.Add(new Vertex(new Vector3(1, -1, 0)));
            mesh.Vertices.Add(new Vertex(new Vector3(1, 1, 0)));

            var fg = new FaceGroup("Face");
            fg.Faces.Add(new Face(0, 1, 2));
            fg.Faces.Add(new Face(2, 3, 0));
            mesh.FaceGroups.Add(fg);

            return mesh;
        }

        public static Mesh CreateCoordinateOrigin()
        {
            var mesh = new Mesh("Coordinate Origin");

            mesh.Vertices.Add(new Vertex(new Vector3(-1, -1, -1), Color4.White));
            mesh.Vertices.Add(new Vertex(new Vector3(1, -1, -1), Color4.Red));
            mesh.Vertices.Add(new Vertex(new Vector3(-1, -1, 1), Color4.Blue));
            mesh.Vertices.Add(new Vertex(new Vector3(-1, 1, -1), Color4.Lime));

            var fg = new FaceGroup("Face");
            fg.Faces.Add(new Face(0, 2, 1));
            fg.Faces.Add(new Face(3, 0, 1));
            fg.Faces.Add(new Face(0, 3, 2));
            mesh.FaceGroups.Add(fg);

            return mesh;
        }

        public static Mesh CreateCube()
        {
            var mesh = new Mesh("Cube");

            mesh.Vertices.Add(new Vertex(new Vector3(-1, -1, 1)));
            mesh.Vertices.Add(new Vertex(new Vector3(1, -1, 1)));
            mesh.Vertices.Add(new Vertex(new Vector3(-1, 1, 1)));
            mesh.Vertices.Add(new Vertex(new Vector3(1, 1, 1)));

            mesh.Vertices.Add(new Vertex(new Vector3(-1, -1, -1)));
            mesh.Vertices.Add(new Vertex(new Vector3(1, -1, -1)));
            mesh.Vertices.Add(new Vertex(new Vector3(-1, 1, -1)));
            mesh.Vertices.Add(new Vertex(new Vector3(1, 1, -1)));

            var fg = new FaceGroup("Face");
            fg.Faces.Add(new Face(0, 1, 2));
            fg.Faces.Add(new Face(1, 3, 2));

            fg.Faces.Add(new Face(6, 2, 3));
            fg.Faces.Add(new Face(3, 7, 6));

            fg.Faces.Add(new Face(1, 5, 3));
            fg.Faces.Add(new Face(5, 7, 3));

            fg.Faces.Add(new Face(2, 6, 4));
            fg.Faces.Add(new Face(0, 2, 4));

            fg.Faces.Add(new Face(1, 0, 4));
            fg.Faces.Add(new Face(1, 4, 5));

            fg.Faces.Add(new Face(6, 7, 5));
            fg.Faces.Add(new Face(6, 5, 4));
            mesh.FaceGroups.Add(fg);

            return mesh;
        }
    }
}