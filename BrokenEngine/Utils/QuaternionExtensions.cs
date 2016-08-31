using OpenTK;

namespace BrokenEngine.Utils
{
    public static class QuaternionExtensions
    {

        public static Vector3 ToEuler(this Quaternion quat)
        {
            return QuaternionUtils.ToEuler(quat);
        }

    }
}