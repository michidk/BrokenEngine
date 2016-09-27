using OpenTK.Graphics.OpenGL;

namespace BrokenEngine.Open_GL
{
    public class VertexArray<TVertex> where TVertex : struct
    {
        private readonly int handle;

        public VertexArray(Buffer<TVertex> vertexBuffer, ShaderProgram program, params VertexAttribute[] attributes)
        {
            GL.GenVertexArrays(1, out this.handle);

            Bind();

            vertexBuffer.Bind();

            program.SetVertexAttributes(attributes);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void Bind()
        {
            GL.BindVertexArray(this.handle);
        }

        public void Reset()
        {
            GL.BindVertexArray(0);
        }

    }
}