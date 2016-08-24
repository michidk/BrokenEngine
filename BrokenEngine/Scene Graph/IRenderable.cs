using OpenTK;

namespace BrokenEngine.Scene_Graph
{
    public interface IRenderable
    {

        void Render(Matrix4 viewMatrix, Matrix4 projMatrix);

    }
}