using OpenTK;

namespace BrokenEngine.Utils
{
    public static class MatrixUtils
    {

        public static Vector3 TransformPoint(Matrix4 matrix, Vector3 pos)
        {
            return Vector3.TransformPosition(pos, matrix);
        }

        // not affected by scale and translation
        public static Vector3 TransformDirection(Matrix4 matrix, Vector3 dir)
        {
            return Vector3.TransformVector(dir, matrix);
        }

        public static Quaternion TransformRotation(Matrix4 matrix, Quaternion rot)
        {
            return (matrix * Matrix4.CreateFromQuaternion(rot)).ExtractRotation();
        }

        public static Vector3 TransformScale(Matrix4 matrix, Vector3 pos)
        {
            var vec4 = Matrix4.Identity;
            vec4.Row3 = VectorUtils.CreatePositionVector(pos);
            return (matrix * Matrix4.CreateTranslation(pos)).ExtractScale();
        }

        public static Matrix4 CreateTRS(Vector3 translation, Quaternion rotation, Vector3 scale)
        {
            return Matrix4.CreateTranslation(translation) * Matrix4.CreateFromQuaternion(rotation) * Matrix4.CreateScale(scale);
            //return Matrix4.CreateScale(scale) * Matrix4.CreateFromQuaternion(rotation) * Matrix4.CreateTranslation(translation);
        }

    }
}