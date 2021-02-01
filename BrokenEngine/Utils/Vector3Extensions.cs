using OpenTK;
using OpenTK.Mathematics;

namespace BrokenEngine.Utils
{
    public static class Vector3Extensions
    {

        public static void Mod(this Vector3 vec, float value)
        {
            vec.X = vec.X % value;
            vec.Y = vec.Y % value;
            vec.Z = vec.Z % value;
        }

    }
}
