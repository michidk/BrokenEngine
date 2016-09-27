using BrokenEngine.Open_GL;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace BrokenEngine.Materials
{
    public class Material
    {

        public readonly string Name;

        public Shader Shader => shader;

        public dynamic Parameters = new System.Dynamic.ExpandoObject();

        protected Shader shader;
        private bool loaded = false;

        public Material(string name)
        {
            Name = name;

            LoadResources();
        }

        public void LoadResources()
        {
            loaded = true;

            shader = Shader.LoadShaderFromName(Name);
        }

        public virtual void Apply()
        {
            shader.Program.Use();

            shader.Program.SetMatrixUniform("u_modelViewProjMatrix", (Matrix4) Parameters.ModelViewProjMatrix, GL.UniformMatrix4);
            shader.Program.SetMatrixUniform("u_modelWorldMatrix", (Matrix4) Parameters.ModelWorldMatrix, GL.UniformMatrix4);
            shader.Program.SetMatrixUniform("u_worldViewMatrix", (Matrix4) Parameters.WorldViewMatrix, GL.UniformMatrix4);
            shader.Program.SetMatrixUniform("u_normalMatrix", (Matrix4) Parameters.NormalMatrix, GL.UniformMatrix4);
            shader.Program.SetValueUniform("u_cameraPosition", (Vector3) Parameters.CameraPosition, GL.Uniform3);
        }

        public void CleanUp()
        {
            shader.Program.CleanUp();
        }

    }
}