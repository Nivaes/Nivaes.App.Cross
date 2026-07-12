namespace Nivaes.App.Cross.Droid.GropImage
{
    using Android.Graphics;

    public class CenterHandleHelper : HandleHelper
    {
        // Constructor /////////////////////////////////////////////////////////////

        public CenterHandleHelper()
            : base(null, null)
        {
        }

        // HandleHelper Methods ////////////////////////////////////////////////////
        public override void UpdateCropWindow(float x,
            float y,
            Rect imageRect,
            float snapRadius)
        {
            float left = EdgeManager.Left.Coordinate;
            float top = EdgeManager.Top.Coordinate;
            float right = EdgeManager.Right.Coordinate;
            float bottom = EdgeManager.Bottom.Coordinate;

            float currentCenterX = (left + right) / 2;
            float currentCenterY = (top + bottom) / 2;

            float offsetX = x - currentCenterX;
            float offsetY = y - currentCenterY;

            // Adjust the crop window.
            EdgeManager.Left.Offset(offsetX);
            EdgeManager.Top.Offset(offsetY);
            EdgeManager.Right.Offset(offsetX);
            EdgeManager.Bottom.Offset(offsetY);

            // Check if we have gone out of bounds on the sides, and fix.
            if (EdgeManager.Left.IsOutsideMargin(imageRect, snapRadius))
            {
                float offset = EdgeManager.Left.SnapToRect(imageRect);
                EdgeManager.Right.Offset(offset);
            }
            else if (EdgeManager.Right.IsOutsideMargin(imageRect, snapRadius))
            {
                float offset = EdgeManager.Right.SnapToRect(imageRect);
                EdgeManager.Left.Offset(offset);
            }

            // Check if we have gone out of bounds on the top or bottom, and fix.
            if (EdgeManager.Top.IsOutsideMargin(imageRect, snapRadius))
            {
                float offset = EdgeManager.Top.SnapToRect(imageRect);
                EdgeManager.Bottom.Offset(offset);
            }
            else if (EdgeManager.Bottom.IsOutsideMargin(imageRect, snapRadius))
            {
                float offset = EdgeManager.Bottom.SnapToRect(imageRect);
                EdgeManager.Top.Offset(offset);
            }
        }

        public override void UpdateCropWindow(float x,
            float y,
            float targetAspectRatio,
            Rect imageRect,
            float snapRadius)
        {
            UpdateCropWindow(x, y, imageRect, snapRadius);
        }
    }
}
