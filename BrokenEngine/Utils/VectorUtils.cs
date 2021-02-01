using System;
using OpenTK.Mathematics;

namespace BrokenEngine.Utils
{
    public static class VectorUtils
    {

        public static Vector4 CreatePositionVector(Vector3 pos)
        {
            return new Vector4(pos, 1.0f);
        }

        public static Vector4 CreateDirectionVector(Vector3 dir)
        {
            return new Vector4(dir, 0.0f);
        }

        public static Vector3 RadToDeg(Vector3 vec)
        {
            return vec * MathUtils.RAD_TO_DEG;
        }

        public static Vector3 DegToRad(Vector3 vec)
        {
            return vec * MathUtils.DEG_TO_RAD;
        }

        public static Vector3 Cos(Vector3 vec)
        {
            return new Vector3((float) Math.Cos(vec.X), (float) Math.Cos(vec.Y), (float) Math.Cos(vec.Z));
        }

        public static Vector3 Sin(Vector3 vec)
        {
            return new Vector3((float) Math.Sin(vec.X), (float) Math.Sin(vec.Y), (float) Math.Sin(vec.Z));
        }

    }
}
