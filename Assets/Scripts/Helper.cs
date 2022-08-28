using UnityEngine;
using Unity;

namespace Utility
{
    public class Helper
    {
        private const float yUpperScreenBounds = 5.5f;
        private const float xPositionBounds = 9.5f;
        private const float yLowerBounds = -3.5f;

        public static float GetXPositionBounds()
        {
            return xPositionBounds;
        }

        public static float GetYUpperScreenBounds()
        {
            return yUpperScreenBounds;
        }

        public static float GetYLowerBounds()
        {
            return yLowerBounds;
        }
    }
}


