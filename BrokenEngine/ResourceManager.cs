using System;
using System.IO;

namespace BrokenEngine
{
    public static class ResourceManager
    {

        public const string RESOURCE_FOLDER = "Resources/";

        // private static readonly string assemblyLocation = System.Reflection.Assembly.GetEntryAssembly().Location;


        public static byte[] GetBytes(string file)
        {
            string path = Path.Combine(RESOURCE_FOLDER, file);
            byte[] bytes = null;
            try
            {
                bytes = File.ReadAllBytes(path);
            }
            catch (IOException e)
            {
                throw new IOException($"Can't read file: {file}", e);
            }
            return bytes;
        }

        public static string GetString(string file)
        {
            string path = Path.Combine(RESOURCE_FOLDER, file);
            string text = null;
            try
            {
                text = File.ReadAllText(path);
            }
            catch (IOException e)
            {
                throw new IOException($"Can't read file: {file}", e);
            }
            return text;
        }

        public static StreamReader GetStream(string file)
        {
            string path = Path.Combine(RESOURCE_FOLDER, file);
            return new StreamReader(path);
        }

    }
}
