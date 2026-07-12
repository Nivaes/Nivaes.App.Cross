namespace Nivaes.App.Cross.Droid.GropImage
{
    using System;
    using Android.Graphics;

    public static class AspectRatioUtil
    {
        /// <summary>
        /// Calculates the aspect ratio given a rectangle.
        /// </summary>
        public static float CalculateAspectRatio(float left, float top, float right, float bottom)
        {
            float width = right - left;
            float height = bottom - top;
            float aspectRatio = width / height;

            return aspectRatio;
        }

        /// <summary>
        /// Calculates the aspect ratio given a rectangle.
        /// </summary>
        public static float CalculateAspectRatio(Rect rect)
        {
            if (rect == null) throw new ArgumentNullException(nameof(rect));

            try
            {
                float aspectRatio = rect.Width() / (float)rect.Height();

                return aspectRatio;
            }
            catch (Java.Lang.Exception ex)
            {
                ex.ToString();
            }
            return 5;
        }

        /// <summary>
        /// Calculates the x-coordinate of the left edge given the other sides of the rectangle and an aspect ratio.
        /// </summary>
        public static float CalculateLeft(float top, float right, float bottom, float targetAspectRatio)
        {
            float height = bottom - top;
            // targetAspectRatio = width / height
            // width = targetAspectRatio * height
            // right - left = targetAspectRatio * height
            float left = right - (targetAspectRatio * height);

            return left;
        }

        /// <summary>
        /// Calculates the y-coordinate of the top edge given the other sides of the rectangle and an aspect ratio.
        /// </summary>
        public static float CalculateTop(float left, float right, float bottom, float targetAspectRatio)
        {
            float width = right - left;
            // targetAspectRatio = width / height
            // width = targetAspectRatio * height
            // height = width / targetAspectRatio
            // bottom - top = width / targetAspectRatio
            float top = bottom - (width / targetAspectRatio);

            return top;
        }

        /// <summary>
        /// Calculates the x-coordinate of the right edge given the other sides of the rectangle and an aspect ratio.
        /// </summary>
        public static float CalculateRight(float left, float top, float bottom, float targetAspectRatio)
        {
            float height = bottom - top;
            // targetAspectRatio = width / height
            // width = targetAspectRatio * height
            // right - left = targetAspectRatio * height
            float right = (targetAspectRatio * height) + left;

            return right;
        }

        /// <summary>
        /// Calculates the y-coordinate of the bottom edge given the other sides of the rectangle and an aspect ratio.
        /// </summary>
        public static float CalculateBottom(float left, float top, float right, float targetAspectRatio)
        {
            float width = right - left;
            // targetAspectRatio = width / height
            // width = targetAspectRatio * height
            // height = width / targetAspectRatio
            // bottom - top = width / targetAspectRatio
            float bottom = (width / targetAspectRatio) + top;

            return bottom;
        }

        /// <summary>
        /// Calculates the width of a rectangle given the top and bottom edges and an aspect ratio.
        /// </summary>
        public static float CalculateWidth(float top, float bottom, float targetAspectRatio)
        {
            float height = bottom - top;
            float width = targetAspectRatio * height;

            return width;
        }

        /// <summary>
        /// Calculates the height of a rectangle given the left and right edges and an aspect ratio.
        /// </summary>
        public static float CalculateHeight(float left, float right, float targetAspectRatio)
        {
            float width = right - left;
            float height = width / targetAspectRatio;

            return height;
        }
    }
}
