using OpenTK;
using OpenTK.Graphics;

namespace BrokenEngine.Mesh
{
    public static class MeshUtils
    {
        
        public static Mesh CreateTriangle()
        {
            var mesh = new Mesh("Triangle", 3, 1);

            mesh.Vertices[0] = new Vertex(new Vector3(-1, 1, 0), Color4.Lime);
            mesh.Vertices[1] = new Vertex(new Vector3(-1, -1, 0), Color4.Red);
            mesh.Vertices[2] = new Vertex(new Vector3(1, -1, 0), Color4.Blue);

            mesh.Faces[0] = new Face(0, 1, 2);

            return mesh;
        }

        public static Mesh CreateQuad()
        {
            var mesh = new Mesh("Quad", 4, 2);

            mesh.Vertices[0] = new Vertex(new Vector3(-1, 1, 0));
            mesh.Vertices[1] = new Vertex(new Vector3(-1, -1, 0));
            mesh.Vertices[2] = new Vertex(new Vector3(1, -1, 0));
            mesh.Vertices[3] = new Vertex(new Vector3(1, 1, 0));

            mesh.Faces[0] = new Face(0, 1, 2);
            mesh.Faces[1] = new Face(2, 3, 0);
            
            return mesh;
        }

        public static Mesh CreateCoordinateOrigin()
        {
            var mesh = new Mesh("Coordinate Origin", 4, 3);

            mesh.Vertices[0] = new Vertex(new Vector3(-1, -1, -1), Color4.White);
            mesh.Vertices[1] = new Vertex(new Vector3(1, -1, -1), Color4.Red);
            mesh.Vertices[2] = new Vertex(new Vector3(-1, -1, 1), Color4.Blue);
            mesh.Vertices[3] = new Vertex(new Vector3(-1, 1, -1), Color4.Lime);

            mesh.Faces[0] = new Face(0, 2, 1);
            mesh.Faces[1] = new Face(3, 0, 1);
            mesh.Faces[2] = new Face(0, 3, 2);

            return mesh;
        }

        public static Mesh CreateCube()
        {
            var mesh = new Mesh("Cube", 8, 12);

            mesh.Vertices[0] = new Vertex(new Vector3(-1, -1, 1));
            mesh.Vertices[1] = new Vertex(new Vector3(1, -1, 1));
            mesh.Vertices[2] = new Vertex(new Vector3(-1, 1, 1));
            mesh.Vertices[3] = new Vertex(new Vector3(1, 1, 1));

            mesh.Vertices[4] = new Vertex(new Vector3(-1, -1, -1));
            mesh.Vertices[5] = new Vertex(new Vector3(1, -1, -1));
            mesh.Vertices[6] = new Vertex(new Vector3(-1, 1, -1));
            mesh.Vertices[7] = new Vertex(new Vector3(1, 1, -1));

            mesh.Faces[0] = new Face(0, 1, 2);
            mesh.Faces[1] = new Face(1, 3, 2);

            mesh.Faces[2] = new Face(6, 2, 3);
            mesh.Faces[3] = new Face(3, 7, 6);

            mesh.Faces[4] = new Face(1, 5, 3);
            mesh.Faces[5] = new Face(5, 7, 3);

            mesh.Faces[6] = new Face(2, 6, 4);
            mesh.Faces[7] = new Face(0, 2, 4);

            mesh.Faces[8] = new Face(1, 0, 4);
            mesh.Faces[9] = new Face(1, 4, 5);

            mesh.Faces[10] = new Face(6, 7, 5);
            mesh.Faces[11] = new Face(6, 5, 4);

            return mesh;
        }
    }
}