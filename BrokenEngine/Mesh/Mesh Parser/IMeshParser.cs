namespace BrokenEngine.Mesh.Mesh_Parser
{
    public interface IMeshParser<T> where T : Mesh
    {
        T GetMesh();
    }
}