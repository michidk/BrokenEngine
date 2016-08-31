using System;

namespace BrokenEngine.Utils
{
    public static class MathUtils
    {

        public const float PI = (float) Math.PI;
        public const float RAD_TO_DEG = 180 / PI;
        public const float DEG_TO_RAD = PI / 180;

        public static float Clamp(float value, float min, float max)
        {
            return Math.Max(min, Math.Min(max, value));
        }

        public static float ClampAngle(float angle, float min, float max)
        {
            angle = angle % 360;
            if ((angle >= -360F) && (angle <= 360F))
            {
                if (angle < -360F)
                {
                    angle += 360F;
                }
                if (angle > 360F)
                {
                    angle -= 360F;
                }
            }
            return Clamp(angle, min, max);
        }

        public static float Lerp(float from, float to, float time)
        {
            return from * (1 - time) + to * time;
        }

    }
}