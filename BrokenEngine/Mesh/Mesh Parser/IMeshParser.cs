namespace BrokenEngine.Mesh
{
    public interface IMeshParser<T> where T : Mesh
    {
        T GetMesh();
    }
}