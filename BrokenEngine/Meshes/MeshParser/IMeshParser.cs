namespace BrokenEngine.Models.MeshParser
{
    public interface IMeshParser<T> where T : Models.Mesh
    {
        T GetMesh();
    }
}