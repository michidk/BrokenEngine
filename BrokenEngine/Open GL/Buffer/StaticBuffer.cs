using OpenTK.Graphics.OpenGL;

namespace BrokenEngine.Open_GL.Buffer
{
    public class StaticBuffer<T> : Buffer<T> where T : struct
    {

        public T[] Data;

        public override int Count { get { return Data.Length; } }

        public StaticBuffer(int elementSize, T[] data, BufferTarget target) : base(elementSize, target)
        {
            Data = data;
        }

        public override int BufferData()
        {
            return BufferData(Data);
        }

    }
}