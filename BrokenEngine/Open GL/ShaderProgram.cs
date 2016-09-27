using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace BrokenEngine.Open_GL
{
    public class ShaderProgram
    {
        private readonly int handle;

        private readonly Dictionary<string, int> attributeLocations = new Dictionary<string, int>();
        private readonly Dictionary<string, int> uniformLocations = new Dictionary<string, int>();

        public ShaderProgram(params CompiledShader[] shaders)
        {
            this.handle = GL.CreateProgram();

            foreach (var shader in shaders)
                GL.AttachShader(this.handle, shader);

            GL.LinkProgram(this.handle);

            foreach (var shader in shaders)
                GL.DetachShader(this.handle, shader);
        }

        public void Use()
        {
            GL.UseProgram(this.handle);
        }

        public int GetAttributeLocation(string name)
        {
            int i;
            if (!this.attributeLocations.TryGetValue(name, out i))
            {
                i = GL.GetAttribLocation(this, name);
                this.attributeLocations.Add(name, i);
            }
            return i;
        }

        public int GetUniformLocation(string name)
        {
            int i;
            if (!this.uniformLocations.TryGetValue(name, out i))
            {
                i = GL.GetUniformLocation(this, name);
                this.uniformLocations.Add(name, i);
            }
            return i;
        }

        public void SetVertexAttribute(VertexAttribute attribute)
        {
            int index = GetAttributeLocation(attribute.Name);

            // enable and set attribute
            GL.EnableVertexAttribArray(index);
            GL.VertexAttribPointer(index, attribute.Size, attribute.Type, attribute.Normalize, attribute.Stride, attribute.Offset);
        }

        public void SetVertexAttributes(VertexAttribute[] attributes)
        {
            foreach (var attribute in attributes)
                SetVertexAttribute(attribute);
        }
        
        public void CleanUp()
        {
            GL.UseProgram(0);
        }

        #region Helpers
        public delegate void GLValue<T>(int location, T value);
        public void SetValueUniform<T>(string name, T value, GLValue<T> glMethod)
        {
            var i = GetUniformLocation(name);
            glMethod(i, value);
        }

        public delegate void GLMatrix<T>(int location, bool transpose, ref T matrix);
        public void SetMatrixUniform<T>(string name, T matrix, GLMatrix<T> glMethod)
        {
            var i = GetUniformLocation(name);
            glMethod(i, false, ref matrix);
        }
        #endregion

        #region Operators
        public static implicit operator int(ShaderProgram program)
        {
            return program.handle;
        }
        #endregion
    }
}