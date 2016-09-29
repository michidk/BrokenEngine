using OpenTK;

namespace BrokenEngine.Components
{
    public interface IRenderable
    {

        void Render(Matrix4 viewMatrix, Matrix4 viewProjectionMatrix);

    }
}