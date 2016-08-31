using System;
using OpenTK;

namespace BrokenEngine.Utils
{
    // conversion formulas from http://de.mathworks.com/matlabcentral/fileexchange/20696-function-to-convert-between-dcm-euler-angles-quaternions-and-euler-vectors/content/SpinCalc.m
    public class QuaternionUtils
    {

        // angles are in degrees
        public static Quaternion FromEuler(Vector3 rotation)
        {
            // convert
            rotation = VectorUtils.DegToRad(rotation);

            Vector3 c = VectorUtils.Cos(rotation * 0.5f);
            Vector3 s = VectorUtils.Sin(rotation * 0.5f);

            float qw = c.X * c.Y * c.Z + s.X * s.Y * s.Z;
            float qx = s.X * c.Y * c.Z - c.X * s.Y * s.Z;
            float qy = c.X * s.Y * c.Z + s.X * c.Y * s.Z;
            float qz = c.X * c.Y * s.Z - s.X * s.Y * c.Z;

            return new Quaternion(qx, qy, qz, qw);
        }

        // angles are in degrees
        // TODO: cleanup
        public static Vector3 ToEuler(Quaternion quaternion)
        {
            float heading, attitude, bank;
            float sqw = quaternion.W * quaternion.W;
            float sqx = quaternion.X * quaternion.X;
            float sqy = quaternion.Y * quaternion.Y;
            float sqz = quaternion.Z * quaternion.Z;
            float unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
            float test = quaternion.X * quaternion.Y + quaternion.Z * quaternion.W;
            if (test > 0.499 * unit)
            { // singularity at north pole
                heading = 2 * (float) Math.Atan2(quaternion.X, quaternion.W);
                attitude = MathUtils.PI / 2;
                bank = 0;
            }
            else if (test < -0.499 * unit)
            { // singularity at south pole
                heading = -2 * (float) Math.Atan2(quaternion.X, quaternion.W);
                attitude = -MathUtils.PI / 2;
                bank = 0;

            }
            else
            {
                heading = (float)Math.Atan2(2 * quaternion.Y * quaternion.W - 2 * quaternion.X * quaternion.Z, sqx - sqy - sqz + sqw);
                attitude = (float)Math.Asin(2 * test / unit);
                bank = (float)Math.Atan2(2 * quaternion.X * quaternion.W - 2 * quaternion.Y * quaternion.Z, -sqx + sqy - sqz + sqw);
            }

            return new Vector3(bank, heading, attitude) * MathUtils.RAD_TO_DEG;
        }

    }
}