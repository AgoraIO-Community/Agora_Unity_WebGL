using UnityEngine;
namespace agora_utilities
{
    public class AgoraUIUtils
    {
        internal static readonly Vector2 DefaultDimension = new Vector2(480, 360);

        /// <summary>
        ///   Get a scaled dimension for displaying window.
        /// </summary>
        /// <param name="width">original width</param>
        /// <param name="height">original height</param>
        /// <param name="WindowSideLength">a limiting length for setting the scale if larger</param>
        /// <returns></returns>
        public static Vector2 GetScaledDimension(float width, float height, float WindowSideLength)
        {
            if (width == 0 || height == 0)
            {
                return AgoraUIUtils.DefaultDimension;
            }

            float newWidth = width;
            float newHeight = height;
            float ratio = (float)height / (float)width;
            if (width > height)
            {
                newHeight = WindowSideLength;
                newWidth = WindowSideLength / ratio;
            }
            else
            {
                newHeight = WindowSideLength * ratio;
                newWidth = WindowSideLength;
            }
            return new Vector2(newWidth, newHeight);
        }

        /// <summary>
        ///    Get a random position on screen for placing a video display window
        /// </summary>
        /// <param name="percentage">percentage of screen offset to the center as the boundary</param>
        /// <returns></returns>
        public static Vector2 GetRandomPosition(float percentage = 50)
        {
            if (percentage < 0) percentage = 0;
            if (percentage > 100) percentage = 100;
            percentage = percentage / 100f;
            float xPos = UnityEngine.Random.Range(-Screen.width / 2f * percentage, Screen.width / 2f * percentage);
            float yPos = UnityEngine.Random.Range(-Screen.height / 2f * percentage, Screen.height / 2f * percentage);
            Debug.Log(string.Format("Random pos on Screen w:{0} h:{1} _offset:{2} => x:{3} y:{4}",
                Screen.width, Screen.height, percentage, xPos, yPos));
            return new Vector2(xPos, yPos);
        }
    }
}
