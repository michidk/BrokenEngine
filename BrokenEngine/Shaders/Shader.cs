using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using BrokenEngine.Assets;
using BrokenEngine.OpenGL.Shader;
using BrokenEngine.Utils.Attributes;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace BrokenEngine.Materials
{
    public class Shader
    {

        // ShaderCompiler properties
        [XmlIgnore]
        public Matrix4 ModelViewProjMatrix { get; set; }
        [XmlIgnore]
        public Matrix4 ModelWorldMatrix { get; set; }
        [XmlIgnore]
        public Matrix4 WorldViewMatrix { get; set; }
        [XmlIgnore]
        public Matrix4 NormalMatrix { get; set; }
        [XmlIgnore]
        public Vector3 CameraPosition { get; set; }

        [XmlIgnore]
        public OpenGL.Shader.ShaderCompiler ShaderCompiler => Compiler;

        
        protected string shaderFilePath;
        protected OpenGL.Shader.ShaderCompiler Compiler;
        private bool loaded = false;


        [XmlConstructor]
        public Shader()
        {
        }

        public Shader(string shaderFilePath)
        {
            this.shaderFilePath = shaderFilePath;
        }

        public void LoadResources()
        {
            if (loaded)
                return;

            loaded = true;

            Compiler = ShaderCompiler.LoadShaderFromPath(shaderFilePath);
        }

        public virtual void Apply()
        {
            Compiler.Program.Use();

            SetMatrixUniform("u_modelViewProjMatrix", ModelViewProjMatrix, GL.UniformMatrix4);
            SetMatrixUniform("u_modelWorldMatrix", ModelWorldMatrix, GL.UniformMatrix4);
            SetMatrixUniform("u_worldViewMatrix", WorldViewMatrix, GL.UniformMatrix4);
            SetMatrixUniform("u_normalMatrix", NormalMatrix, GL.UniformMatrix4);
            SetValueUniform("u_cameraPosition", CameraPosition, GL.Uniform3);
        }

        public void CleanUp()
        {
            Compiler.Program.CleanUp();
        }

        #region Helpers
        public delegate void GLValue<T>(int location, T value);
        public void SetValueUniform<T>(string name, T value, GLValue<T> glMethod)
        {
            glMethod(GetLocation(name), value);
        }

        public delegate void GLMatrix<T>(int location, bool transpose, ref T matrix);
        public void SetMatrixUniform<T>(string name, T matrix, GLMatrix<T> glMethod)
        {
            glMethod(GetLocation(name), false, ref matrix);
        }

        public void SetBoolUniform(string name, bool value)
        {
            GL.Uniform1(GetLocation(name), value ? 1 : 0);
        }

        public int GetLocation(string name)
        {
            return Compiler.Program.GetUniformLocation(name);
        }
        #endregion

    }
}
