namespace BrokenEngine.Mesh.MeshParser
{
    public interface IMeshParser<T> where T : Mesh
    {
        T GetMesh();
    }
}