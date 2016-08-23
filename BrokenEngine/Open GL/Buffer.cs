using System;
using System.Collections.Generic;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace BrokenEngine.Open_GL
{
    public class Buffer<TElem> : IDisposable where TElem : struct
    {

        public List<TElem> Data;

        private readonly int handle;
        private int elementSize;
        private BufferTarget bufferTarget;

        public int Count { get { return Data.Count; } }


        public Buffer(int elementSize, List<TElem> data, BufferTarget target)
        {
            this.elementSize = elementSize;
            this.Data = data;
            this.bufferTarget = target;

            this.handle = GL.GenBuffer();
        }

        public Buffer(int elementSize, BufferTarget target) : this(elementSize, new List<TElem>(), target)
        {

        }

        public void Bind()
        {
            GL.BindBuffer(bufferTarget, this.handle);
        }

        public void BufferData()
        {
            GL.BufferData(bufferTarget, (IntPtr) (elementSize * Data.Count), Data.ToArray(), BufferUsageHint.StaticDraw);
        }

        #region Operators
        // use this as int
        public static implicit operator int(Buffer<TElem> buffer)
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