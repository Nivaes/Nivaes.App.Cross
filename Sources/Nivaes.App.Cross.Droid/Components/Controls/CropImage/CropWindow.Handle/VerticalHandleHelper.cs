namespace Nivaes.App.Cross.Droid.GropImage
{
    using Android.Graphics;

    public class VerticalHandleHelper : HandleHelper
    {
        // Member Variables ////////////////////////////////////////////////////////

        private readonly Edge mEdgeType;

        // Constructor /////////////////////////////////////////////////////////////

        public VerticalHandleHelper(Edge edge)
            : base(null, edge)
        {
            mEdgeType = edge;
        }

        // HandleHelper Methods ////////////////////////////////////////////////////

        public override void UpdateCropWindow(float x,
            float y,
            float targetAspectRatio,
            Rect imageRect,
            float snapRadius)
        {
            // Adjust this EdgeType accordingly.
            mEdgeType.AdjustCoordinate(x, y, imageRect, snapRadius, targetAspectRatio);

            float left = EdgeManager.Left.Coordinate;
            float top = EdgeManager.Top.Coordinate;
            float right = EdgeManager.Right.Coordinate;
            float bottom = EdgeManager.Bottom.Coordinate;

            // After this EdgeType is moved, our crop window is now out of proportion.
            float targetHeight = AspectRatioUtil.CalculateHeight(left, right, targetAspectRatio);
            float currentHeight = bottom - top;

            // Adjust the crop window so that it maintains the given aspect ratio by
            // moving the adjacent EdgeTypes symmetrically in or out.
            float difference = targetHeight - currentHeight;
            float halfDifference = difference / 2;
            top -= halfDifference;
            bottom += halfDifference;

            EdgeManager.Top.Coordinate = top;
            EdgeManager.Bottom.Coordinate = bottom;

            // Check if we have gone out of bounds on the top or bottom, and fix.
            if (EdgeManager.Top.IsOutsideMargin(imageRect, snapRadius) &&
                !mEdgeType.IsNewRectangleOutOfBounds(EdgeManager.Top,
                    imageRect,
                    targetAspectRatio))
            {
                float offset = EdgeManager.Top.SnapToRect(imageRect);
                EdgeManager.Bottom.Offset(-offset);
                mEdgeType.AdjustCoordinate(targetAspectRatio);
            }
            if (EdgeManager.Bottom.IsOutsideMargin(imageRect, snapRadius) &&
                !mEdgeType.IsNewRectangleOutOfBounds(EdgeManager.Bottom,
                    imageRect,
                    targetAspectRatio))
            {
                float offset = EdgeManager.Bottom.SnapToRect(imageRect);
                EdgeManager.Top.Offset(-offset);
                mEdgeType.AdjustCoordinate(targetAspectRatio);
            }
        }
    }
}
