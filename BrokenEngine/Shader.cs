using System;
using System.Collections.Generic;
using System.IO;
using BrokenEngine.Open_GL.Shader;
using OpenTK.Graphics.OpenGL4;

namespace BrokenEngine
{
    public class Shader
    {

        public struct MetaData
        {
            public string Name;
            public string Author;
            public string Description;
        }

        private const string DEFAULT_PATH = "Shaders/{0}.glsl";

        public MetaData Meta;
        public ShaderProgram Program;

        private string code;

        public Shader(string code, bool compile = true)
        {
            this.code = code;

            if (compile)
                Compile();
        }

        public bool Compile()
        {
            if (string.IsNullOrEmpty(code))
            {
                Globals.Logger.Error("Shader compilation failed: code empty.");
                return false;
            }

            List<CompiledShader> shaders = new List<CompiledShader>();
            using (var reader = new StringReader(code))
            {
                Meta = ParseMetaData(reader);

                string line;
                ShaderType? currentlyParsedType = null;
                string currentCode = "";
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("//# Type"))
                    {
                        var splitted = line.Split(' ');
                        if (splitted.Length < 2)
                            continue;

                        var compiledShader = CompileShader(currentlyParsedType, currentCode);
                        if (compiledShader != null)
                            shaders.Add(compiledShader);

                        currentCode = "";

                        string name = splitted[2];
                        currentlyParsedType = ParseShaderType(name);
                    }

                    if (currentlyParsedType == null)
                        continue;

                    currentCode += line + Environment.NewLine;
                }

                var compiledShader2 = CompileShader(currentlyParsedType, currentCode);
                if (compiledShader2 != null)
                    shaders.Add(compiledShader2);

                Program = new ShaderProgram(shaders.ToArray());
            }

            return true;
        }

        private static CompiledShader CompileShader(ShaderType? type, string code)
        {
            if (type.HasValue)
            {
                if (string.IsNullOrEmpty(code))
                {
                    Globals.Logger.Error($"Shader compilation warning: {type} shader empty");
                    return null;
                }
                else
                {
                    var shader = new CompiledShader(type.Value, code);
                    if (!shader.Compile())
                        return null;
                    return shader;
                }
            }
            return null;
        }

        private static MetaData ParseMetaData(StringReader reader)
        {
            MetaData data = new MetaData();

            string line;
            while ((line = reader.ReadLine()) != null && line.StartsWith("//#"))
            {
                var splitted = line.Split(' ');
                if (splitted.Length < 3)
                    continue;
                string content = "";
                for (int i = 2; i < splitted.Length; i++)
                    content += splitted[i] + " ";

                switch (splitted[1].ToLower())
                {
                    case "name":
                        data.Name = content;
                        break;
                    case "author":
                        data.Author = content;
                        break;
                    case "description":
                        data.Description = content;
                        break;
                }
            }

            return data;
        }

        private static ShaderType? ParseShaderType(string name)
        {
            switch (name.ToLower())
            {
                case "vert":
                case "vertex":
                    return ShaderType.VertexShader;
                case "frag":
                case "fragment":
                    return ShaderType.FragmentShader;
                case "geo":
                case "geometry":
                    return ShaderType.GeometryShader;
                case "tess evaluation":
                case "tess eval":
                case "eval":
                case "evaluation":
                    return ShaderType.TessEvaluationShader;
                case "tess control":
                case "control":
                    return ShaderType.TessControlShader;
                case "compute":
                    return ShaderType.ComputeShader;
                default:
                    return null;
            }
        }

        public static Shader LoadShaderFromPath(string path)
        {
            string code = ResourceManager.GetString(path);
            if (code == null)
                return null;
            return new Shader(code);
        }

        public static Shader LoadShaderFromName(string name)
        {
            return LoadShaderFromPath(string.Format(DEFAULT_PATH, name));
        }

    }
}