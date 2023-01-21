namespace Utility
{
    public class Helper
    {
        private const float _xPositionBounds = 9.5f;
        private const float _yLowerBounds = -3.5f;
        private const float _yUpperScreenBounds = 5.5f;

        public static float GetXPositionBounds()
        {
            return _xPositionBounds;
        }

        public static float GetYUpperScreenBounds()
        {
            return _yUpperScreenBounds;
        }

        public static float GetYLowerBounds()
        {
            return _yLowerBounds;
        }
    }
}


