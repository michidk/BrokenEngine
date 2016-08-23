using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace BrokenEngine.Open_GL
{
    public class ShaderProgram
    {
        private readonly int handle;

        private readonly Dictionary<string, int> attributeLocations = new Dictionary<string, int>();
        private readonly Dictionary<string, int> uniformLocations = new Dictionary<string, int>();

        public ShaderProgram(params Shader[] shaders)
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

        public void SetAttribute(VertexAttribute attribute)
        {
            int index = GetAttributeLocation(attribute.Name);

            // enable and set attribute
            GL.EnableVertexAttribArray(index);
            GL.VertexAttribPointer(index, attribute.Size, attribute.Type, attribute.Normalize, attribute.Stride, attribute.Offset);
        }

        public void SetAttributes(VertexAttribute[] attributes)
        {
            foreach (var attribute in attributes)
                SetAttribute(attribute);
        }

        #region Operators
        public static implicit operator int(ShaderProgram program)
        {
            return program.handle;
        }
        #endregion
    }
}