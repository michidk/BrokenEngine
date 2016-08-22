using OpenTK;

namespace OpenGLTest.Utils
{
    public static class MathUtils
    {
        public static Quaternion FromEuler(float yaw, float pitch, float roll)
        {
            Quaternion rotateX = Quaternion.FromAxisAngle(Vector3.UnitX, yaw);
            Quaternion rotateY = Quaternion.FromAxisAngle(Vector3.UnitY, pitch);
            Quaternion rotateZ = Quaternion.FromAxisAngle(Vector3.UnitZ, roll);
            Quaternion.Multiply(ref rotateZ, ref rotateY, out rotateY);
            Quaternion.Multiply(ref rotateX, ref rotateY, out rotateY);
            return rotateY;
        }

        public static Quaternion FromEuler(Vector3 rotation)
        {
            return FromEuler(rotation.X, rotation.Y, rotation.Z);
        }
    }
}