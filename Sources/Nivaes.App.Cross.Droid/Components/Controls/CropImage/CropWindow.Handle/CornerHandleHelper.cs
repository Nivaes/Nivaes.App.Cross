namespace Nivaes.App.Cross.Droid.GropImage
{
    using Android.Graphics;

    public class CornerHandleHelper : HandleHelper
    {
        public CornerHandleHelper(Edge horizontalEdge, Edge verticalEdge)
            : base(horizontalEdge, verticalEdge)
        {
        }

        // HandleHelper Methods ////////////////////////////////////////////////////

        public override void UpdateCropWindow(float x,
            float y,
            float targetAspectRatio,
            Rect imageRect,
            float snapRadius)
        {
            EdgePair activeEdges = GetActiveEdges(x, y, targetAspectRatio);
            Edge primaryEdge = activeEdges.Primary;
            Edge secondaryEdge = activeEdges.Secondary;

            primaryEdge.AdjustCoordinate(x, y, imageRect, snapRadius, targetAspectRatio);
            secondaryEdge.AdjustCoordinate(targetAspectRatio);

            if (secondaryEdge.IsOutsideMargin(imageRect, snapRadius))
            {
                secondaryEdge.SnapToRect(imageRect);
                primaryEdge.AdjustCoordinate(targetAspectRatio);
            }
        }
    }
}
