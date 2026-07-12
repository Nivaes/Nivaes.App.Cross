namespace Nivaes.App.Cross.Droid.GropImage
{
    using System;
    using Android.Content;
    using Android.Util;

    internal class HandleUtil
    {
        // Private Constants ///////////////////////////////////////////////////////

        // The radius (in dp) of the touchable area around the handle. We are basing
        // this value off of the recommended 48dp Rhythm. See:
        // http://developer.android.com/design/style/metrics-grids.html#48dp-rhythm
        private static readonly int TargetRadiusDP = 24;

        // Public Methods //////////////////////////////////////////////////////////

        /**
         * Gets the default target radius (in pixels). This is the radius of the
         * circular area that can be touched in order to activate the handle.
         * 
         * @param context the Context
         * @return the target radius (in pixels)
         */

        public static float GetTargetRadius(Context context)
        {
            float targetRadius = TypedValue.ApplyDimension(ComplexUnitType.Dip, TargetRadiusDP,
                context.Resources.DisplayMetrics);
            return targetRadius;
        }

        /**
         * Determines which, if any, of the handles are pressed given the touch
         * coordinates, the bounding box, and the touch radius.
         * 
         * @param x the x-coordinate of the touch point
         * @param y the y-coordinate of the touch point
         * @param left the x-coordinate of the left bound
         * @param top the y-coordinate of the top bound
         * @param right the x-coordinate of the right bound
         * @param bottom the y-coordinate of the bottom bound
         * @param targetRadius the target radius in pixels
         * @return the Handle that was pressed; null if no Handle was pressed
         */

        public static Handle GetPressedHandle(float x,
            float y,
            float left,
            float top,
            float right,
            float bottom,
            float targetRadius)
        {
            Handle pressedHandle = null;

            // Note: corner-handles take precedence, then side-handles, then center.

            if (IsInCornerTargetZone(x, y, left, top, targetRadius))
            {
                pressedHandle = HandleManager.TopLeft;
            }
            else if (IsInCornerTargetZone(x, y, right, top, targetRadius))
            {
                pressedHandle = HandleManager.TopRight;
            }
            else if (IsInCornerTargetZone(x, y, left, bottom, targetRadius))
            {
                pressedHandle = HandleManager.BottomLeft;
            }
            else if (IsInCornerTargetZone(x, y, right, bottom, targetRadius))
            {
                pressedHandle = HandleManager.BottomRight;
            }
            else if (IsInCenterTargetZone(x, y, left, top, right, bottom) && FocusCenter())
            {
                pressedHandle = HandleManager.Center;
            }
            else if (IsInHorizontalTargetZone(x, y, left, right, top, targetRadius))
            {
                pressedHandle = HandleManager.Top;
            }
            else if (IsInHorizontalTargetZone(x, y, left, right, bottom, targetRadius))
            {
                pressedHandle = HandleManager.Bottom;
            }
            else if (IsInVerticalTargetZone(x, y, left, top, bottom, targetRadius))
            {
                pressedHandle = HandleManager.Left;
            }
            else if (IsInVerticalTargetZone(x, y, right, top, bottom, targetRadius))
            {
                pressedHandle = HandleManager.Right;
            }
            else if (IsInCenterTargetZone(x, y, left, top, right, bottom) && !FocusCenter())
            {
                pressedHandle = HandleManager.Center;
            }

            return pressedHandle;
        }

        /**
         * Calculates the offset of the touch point from the precise location of the
         * specified handle.
         * 
         * @return the offset as a Pair where the x-offset is the first value and
         *         the y-offset is the second value; null if the handle is null
         */

        public static Pair GetOffset(Handle handle,
            float x,
            float y,
            float left,
            float top,
            float right,
            float bottom)
        {
            if (handle == null)
            {
                return null;
            }

            float touchOffsetX = 0;
            float touchOffsetY = 0;

            // Calculate the offset from the appropriate handle.
            switch (handle.HandleType)
            {
                case HandleType.TopLeft:
                    touchOffsetX = left - x;
                    touchOffsetY = top - y;
                    break;
                case HandleType.TopRight:
                    touchOffsetX = right - x;
                    touchOffsetY = top - y;
                    break;
                case HandleType.BottomLeft:
                    touchOffsetX = left - x;
                    touchOffsetY = bottom - y;
                    break;
                case HandleType.BottomRight:
                    touchOffsetX = right - x;
                    touchOffsetY = bottom - y;
                    break;
                case HandleType.Left:
                    touchOffsetX = left - x;
                    touchOffsetY = 0;
                    break;
                case HandleType.Top:
                    touchOffsetX = 0;
                    touchOffsetY = top - y;
                    break;
                case HandleType.Right:
                    touchOffsetX = right - x;
                    touchOffsetY = 0;
                    break;
                case HandleType.Bottom:
                    touchOffsetX = 0;
                    touchOffsetY = bottom - y;
                    break;
                case HandleType.Center:
                    float centerX = (right + left) / 2;
                    float centerY = (top + bottom) / 2;
                    touchOffsetX = centerX - x;
                    touchOffsetY = centerY - y;
                    break;
            }

            var result = new Pair(touchOffsetX, touchOffsetY);
            return result;
        }

        // Private Methods /////////////////////////////////////////////////////////

        /**
         * Determines if the specified coordinate is in the target touch zone for a
         * corner handle.
         * 
         * @param x the x-coordinate of the touch point
         * @param y the y-coordinate of the touch point
         * @param handleX the x-coordinate of the corner handle
         * @param handleY the y-coordinate of the corner handle
         * @param targetRadius the target radius in pixels
         * @return true if the touch point is in the target touch zone; false
         *         otherwise
         */

        private static bool IsInCornerTargetZone(float x,
            float y,
            float handleX,
            float handleY,
            float targetRadius)
        {
            if (Math.Abs(x - handleX) <= targetRadius && Math.Abs(y - handleY) <= targetRadius)
            {
                return true;
            }
            return false;
        }

        /**
         * Determines if the specified coordinate is in the target touch zone for a
         * horizontal bar handle.
         * 
         * @param x the x-coordinate of the touch point
         * @param y the y-coordinate of the touch point
         * @param handleXStart the left x-coordinate of the horizontal bar handle
         * @param handleXEnd the right x-coordinate of the horizontal bar handle
         * @param handleY the y-coordinate of the horizontal bar handle
         * @param targetRadius the target radius in pixels
         * @return true if the touch point is in the target touch zone; false
         *         otherwise
         */

        private static bool IsInHorizontalTargetZone(float x,
            float y,
            float handleXStart,
            float handleXEnd,
            float handleY,
            float targetRadius)
        {
            if (x > handleXStart && x < handleXEnd && Math.Abs(y - handleY) <= targetRadius)
            {
                return true;
            }
            return false;
        }

        /**
         * Determines if the specified coordinate is in the target touch zone for a
         * vertical bar handle.
         * 
         * @param x the x-coordinate of the touch point
         * @param y the y-coordinate of the touch point
         * @param handleX the x-coordinate of the vertical bar handle
         * @param handleYStart the top y-coordinate of the vertical bar handle
         * @param handleYEnd the bottom y-coordinate of the vertical bar handle
         * @param targetRadius the target radius in pixels
         * @return true if the touch point is in the target touch zone; false
         *         otherwise
         */

        private static bool IsInVerticalTargetZone(float x,
            float y,
            float handleX,
            float handleYStart,
            float handleYEnd,
            float targetRadius)
        {
            if (Math.Abs(x - handleX) <= targetRadius && y > handleYStart && y < handleYEnd)
            {
                return true;
            }
            return false;
        }

        /**
         * Determines if the specified coordinate falls anywhere inside the given
         * bounds.
         * 
         * @param x the x-coordinate of the touch point
         * @param y the y-coordinate of the touch point
         * @param left the x-coordinate of the left bound
         * @param top the y-coordinate of the top bound
         * @param right the x-coordinate of the right bound
         * @param bottom the y-coordinate of the bottom bound
         * @return true if the touch point is inside the bounding rectangle; false
         *         otherwise
         */

        private static bool IsInCenterTargetZone(float x,
            float y,
            float left,
            float top,
            float right,
            float bottom)
        {
            if (x > left && x < right && y > top && y < bottom)
            {
                return true;
            }
            return false;
        }

        /**
         * Determines if the cropper should focus on the center handle or the side
         * handles. If it is a small image, focus on the center handle so the user
         * can move it. If it is a large image, focus on the side handles so user
         * can grab them. Corresponds to the appearance of the
         * RuleOfThirdsGuidelines.
         * 
         * @return true if it is small enough such that it should focus on the
         *         center; less than show_guidelines limit
         */

        private static bool FocusCenter()
        {
            return (!CropOverlayView.ShowGuidelines());
        }
    }
}
