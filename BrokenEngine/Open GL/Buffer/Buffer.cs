using System;
using BrokenEngine.Utils.Attributes;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace BrokenEngine.Open_GL.Buffer
{
    public abstract class Buffer<T> : IDisposable where T : struct
    {

        public abstract int Count { get; }

        private readonly int handle;
        private int elementSize;
        private BufferTarget bufferTarget;

        public Buffer(int elementSize, BufferTarget target)
        {
            this.elementSize = elementSize;
            this.bufferTarget = target;

            this.handle = GL.GenBuffer();
        }

        public void Bind()
        {
            GL.BindBuffer(bufferTarget, this.handle);
        }

        public abstract int BufferData();

        protected int BufferData(T[] data)
        {
            int size = elementSize * data.Length;
            GL.BufferData(bufferTarget, (IntPtr) size, data, BufferUsageHint.StaticDraw);
            return size;
        }

        [Untested]
        public int BufferSubData(T[] data, int offset)
        {
            int size = elementSize * data.Length;
            GL.BufferSubData(bufferTarget, (IntPtr) offset, (IntPtr) size, data);
            return size;
        }

        // reset state for potential further draw calls (optional, but good practice)
        public void Reset()
        {
            GL.BindBuffer(bufferTarget, 0);
        }

        #region Operators
        // use this as int
        public static implicit operator int(Buffer<T> buffer)
        {
            return buffer.handle;
        }
        #endregion

        #region Disposing
        private bool disposed;

        /// <summary>
        /// Disposes the vertex buffer and deletes the underlying GL object.
        /// </summary>
        public void Dispose()
        {
            this.dispose();
            // stop GC from executing a redundant garbage collection
            GC.SuppressFinalize(this);
        }

        private void dispose()
        {
            if (this.disposed)
                return;

            if (GraphicsContext.CurrentContext == null || GraphicsContext.CurrentContext.IsDisposed)
                return;

            GL.DeleteBuffer(this);

            this.disposed = true;
        }

        ~Buffer()
        {
            this.dispose();
        }
        #endregion
    }
}