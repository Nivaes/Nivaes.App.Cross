namespace Nivaes.App.Cross.Droid.GropImage
{
    using Android.Graphics;

    public class HorizontalHandleHelper : HandleHelper
    {
        // Member Variables ////////////////////////////////////////////////////////

        private readonly Edge mEdge;

        // Constructor /////////////////////////////////////////////////////////////

        public HorizontalHandleHelper(Edge edge) : base(edge, null)
        {
            mEdge = edge;
        }

        // HandleHelper Methods ////////////////////////////////////////////////////

        public override void UpdateCropWindow(float x,
            float y,
            float targetAspectRatio,
            Rect imageRect,
            float snapRadius)
        {
            // Adjust this Edge accordingly.
            mEdge.AdjustCoordinate(x, y, imageRect, snapRadius, targetAspectRatio);

            float left = EdgeManager.Left.Coordinate;
            float top = EdgeManager.Top.Coordinate;
            float right = EdgeManager.Right.Coordinate;
            float bottom = EdgeManager.Bottom.Coordinate;

            // After this Edge is moved, our crop window is now out of proportion.
            float targetWidth = AspectRatioUtil.CalculateWidth(top, bottom, targetAspectRatio);
            float currentWidth = right - left;

            // Adjust the crop window so that it maintains the given aspect ratio by
            // moving the adjacent edges symmetrically in or out.
            float difference = targetWidth - currentWidth;
            float halfDifference = difference / 2;
            left -= halfDifference;
            right += halfDifference;

            EdgeManager.Left.Coordinate = left;
            EdgeManager.Right.Coordinate = right;

            // Check if we have gone out of bounds on the sides, and fix.
            if (EdgeManager.Left.IsOutsideMargin(imageRect, snapRadius) &&
                !mEdge.IsNewRectangleOutOfBounds(EdgeManager.Left,
                    imageRect,
                    targetAspectRatio))
            {
                float offset = EdgeManager.Left.SnapToRect(imageRect);
                EdgeManager.Right.Offset(-offset);
                mEdge.AdjustCoordinate(targetAspectRatio);
            }
            if (EdgeManager.Right.IsOutsideMargin(imageRect, snapRadius) &&
                !mEdge.IsNewRectangleOutOfBounds(EdgeManager.Right,
                    imageRect,
                    targetAspectRatio))
            {
                float offset = EdgeManager.Right.SnapToRect(imageRect);
                EdgeManager.Left.Offset(-offset);
                mEdge.AdjustCoordinate(targetAspectRatio);
            }
        }
    }
}
