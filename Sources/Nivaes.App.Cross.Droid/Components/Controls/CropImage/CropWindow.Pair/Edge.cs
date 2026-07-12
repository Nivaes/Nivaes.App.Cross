namespace Nivaes.App.Cross.Droid.GropImage
{
    using System;
    using Android.Graphics;
    using Android.Views;

    public class Edge
    {
        public const int MinCropLengthPx = 40;

        public float Coordinate { get; set; }

        private readonly EdgeType mEdgeType;

        public Edge(EdgeType edgeType)
        {
            mEdgeType = edgeType;
        }

        /// <summary>
        /// Add the given number of pixels to the current coordinate position of this Edge.
        /// </summary>
        /// <param name="distance">The number of pixels to add.</param>
        public void Offset(float distance)
        {
            Coordinate += distance;
        }

        /// <summary>
        /// Sets the Edge to the given x-y coordinate but also adjusting for snapping
        /// to the image bounds and parent view border constraints.
        /// </summary>
        /// <param name="x">The x-coordinate</param>
        /// <param name="y">The y-coordinate</param>
        /// <param name="imageRect">The bounding rectangle of the image</param>
        /// <param name="imageSnapRadius">The radius (in pixels) at which the edge should snap to the image</param>
        /// <param name="aspectRatio">Aspect ratio</param>
        public void AdjustCoordinate(float x, float y, Rect imageRect, float imageSnapRadius, float aspectRatio)
        {
            if (imageRect == null) throw new ArgumentNullException(nameof(imageRect));

            switch (mEdgeType)
            {
                case GropImage.EdgeType.Left:
                    Coordinate = AdjustLeft(x, imageRect, imageSnapRadius, aspectRatio);
                    break;
                case GropImage.EdgeType.Top:
                    Coordinate = AdjustTop(y, imageRect, imageSnapRadius, aspectRatio);
                    break;
                case GropImage.EdgeType.Right:
                    Coordinate = AdjustRight(x, imageRect, imageSnapRadius, aspectRatio);
                    break;
                case GropImage.EdgeType.Bottom:
                    Coordinate = AdjustBottom(y, imageRect, imageSnapRadius, aspectRatio);
                    break;
            }
        }

        /// <summary>
        ///  Adjusts this Edge position such that the resulting window will have the given aspect ratio.
        /// </summary>
        /// <param name="aspectRatio">The aspect ratio to achieve</param>
        public void AdjustCoordinate(float aspectRatio)
        {
            float left = EdgeManager.Left.Coordinate;
            float top = EdgeManager.Top.Coordinate;
            float right = EdgeManager.Right.Coordinate;
            float bottom = EdgeManager.Bottom.Coordinate;

            switch (mEdgeType)
            {
                case GropImage.EdgeType.Left:
                    Coordinate = AspectRatioUtil.CalculateLeft(top, right, bottom, aspectRatio);
                    break;
                case GropImage.EdgeType.Top:
                    Coordinate = AspectRatioUtil.CalculateTop(left, right, bottom, aspectRatio);
                    break;
                case GropImage.EdgeType.Right:
                    Coordinate = AspectRatioUtil.CalculateRight(left, top, bottom, aspectRatio);
                    break;
                case GropImage.EdgeType.Bottom:
                    Coordinate = AspectRatioUtil.CalculateBottom(left, top, right, aspectRatio);
                    break;
            }
        }

        /// <summary>
        /// Returns whether or not you can re-scale the image based on whether any edge would be out of bounds.
        /// Checks all the edges for a possibility of jumping out of bounds.
        /// </summary>
        /// <param name="edge">The Edge that is about to be expanded</param>
        /// <param name="imageRect">The rectangle of the picture</param>
        /// <param name="aspectRatio">The desired aspectRatio of the picture.</param>
        /// <returns>Whether or not the new image would be out of bounds.</returns>
        public bool IsNewRectangleOutOfBounds(Edge edge, Rect imageRect, float aspectRatio)
        {
            if (edge == null) throw new ArgumentNullException(nameof(edge));

            float offset = edge.SnapOffset(imageRect);

            switch (mEdgeType)
            {
                case GropImage.EdgeType.Left:
                    if (edge.Equals(EdgeManager.Top))
                    {
                        float top = imageRect.Top;
                        float bottom = EdgeManager.Bottom.Coordinate - offset;
                        float right = EdgeManager.Right.Coordinate;
                        float left = AspectRatioUtil.CalculateLeft(top, right, bottom, aspectRatio);

                        return IsOutOfBounds(top, left, bottom, right, imageRect);

                    }
                    else if (edge.Equals(EdgeManager.Bottom))
                    {
                        float bottom = imageRect.Bottom;
                        float top = EdgeManager.Top.Coordinate - offset;
                        float right = EdgeManager.Right.Coordinate;
                        float left = AspectRatioUtil.CalculateLeft(top, right, bottom, aspectRatio);

                        return IsOutOfBounds(top, left, bottom, right, imageRect);
                    }
                    break;

                case GropImage.EdgeType.Top:
                    if (edge.Equals(EdgeManager.Left))
                    {
                        float left = imageRect.Left;
                        float right = EdgeManager.Right.Coordinate - offset;
                        float bottom = EdgeManager.Bottom.Coordinate;
                        float top = AspectRatioUtil.CalculateTop(left, right, bottom, aspectRatio);

                        return IsOutOfBounds(top, left, bottom, right, imageRect);

                    }
                    else if (edge.Equals(EdgeManager.Right))
                    {
                        float right = imageRect.Right;
                        float left = EdgeManager.Left.Coordinate - offset;
                        float bottom = EdgeManager.Bottom.Coordinate;
                        float top = AspectRatioUtil.CalculateTop(left, right, bottom, aspectRatio);

                        return IsOutOfBounds(top, left, bottom, right, imageRect);
                    }
                    break;

                case GropImage.EdgeType.Right:
                    if (edge.Equals(EdgeManager.Top))
                    {
                        float top = imageRect.Top;
                        float bottom = EdgeManager.Bottom.Coordinate - offset;
                        float left = EdgeManager.Left.Coordinate;
                        float right = AspectRatioUtil.CalculateRight(left, top, bottom, aspectRatio);

                        return IsOutOfBounds(top, left, bottom, right, imageRect);

                    }
                    else if (edge.Equals(EdgeManager.Bottom))
                    {
                        float bottom = imageRect.Bottom;
                        float top = EdgeManager.Top.Coordinate - offset;
                        float left = EdgeManager.Left.Coordinate;
                        float right = AspectRatioUtil.CalculateRight(left, top, bottom, aspectRatio);

                        return IsOutOfBounds(top, left, bottom, right, imageRect);
                    }
                    break;


                case GropImage.EdgeType.Bottom:
                    if (edge.Equals(EdgeManager.Left))
                    {
                        float left = imageRect.Left;
                        float right = EdgeManager.Right.Coordinate - offset;
                        float top = EdgeManager.Top.Coordinate;
                        float bottom = AspectRatioUtil.CalculateBottom(left, top, right, aspectRatio);

                        return IsOutOfBounds(top, left, bottom, right, imageRect);

                    }
                    else if (edge.Equals(EdgeManager.Right))
                    {
                        float right = imageRect.Right;
                        float left = EdgeManager.Left.Coordinate - offset;
                        float top = EdgeManager.Top.Coordinate;
                        float bottom = AspectRatioUtil.CalculateBottom(left, top, right, aspectRatio);

                        return IsOutOfBounds(top, left, bottom, right, imageRect);

                    }
                    break;
            }
            return true;
        }

        /**
           * Returns whether the new rectangle would be out of bounds.
           * 
           * @param top
           * @param left
           * @param bottom
           * @param right
           * @param imageRect the Image to be compared with.
           * @return whether it would be out of bounds
           */
        private bool IsOutOfBounds(float top, float left, float bottom, float right, Rect imageRect)
        {
            return (top < imageRect.Top || left < imageRect.Left || bottom > imageRect.Bottom || right > imageRect.Right);
        }

        /**
         * Snap this Edge to the given image boundaries.
         * 
         * @param imageRect the bounding rectangle of the image to snap to
         * @return the amount (in pixels) that this coordinate was changed (i.e. the
         *         new coordinate minus the old coordinate value)
         */
        public float SnapToRect(Rect imageRect)
        {
            if (imageRect == null) throw new ArgumentNullException(nameof(imageRect));

            float oldCoordinate = Coordinate;

            switch (mEdgeType)
            {
                case GropImage.EdgeType.Left:
                    Coordinate = imageRect.Left;
                    break;
                case GropImage.EdgeType.Top:
                    Coordinate = imageRect.Top;
                    break;
                case GropImage.EdgeType.Right:
                    Coordinate = imageRect.Right;
                    break;
                case GropImage.EdgeType.Bottom:
                    Coordinate = imageRect.Bottom;
                    break;
            }

            float offset = Coordinate - oldCoordinate;
            return offset;
        }

        /**
         * Returns the potential snap offset of snaptoRect, without changing the coordinate.
         * 
         * @param imageRect the bounding rectangle of the image to snap to
         * @return the amount (in pixels) that this coordinate was changed (i.e. the
         *         new coordinate minus the old coordinate value)
         */
        public float SnapOffset(Rect imageRect)
        {
            if (imageRect == null) throw new ArgumentNullException(nameof(imageRect));

            float oldCoordinate = Coordinate;
            float newCoordinate = oldCoordinate;

            switch (mEdgeType)
            {
                case GropImage.EdgeType.Left:
                    newCoordinate = imageRect.Left;
                    break;
                case GropImage.EdgeType.Top:
                    newCoordinate = imageRect.Top;
                    break;
                case GropImage.EdgeType.Right:
                    newCoordinate = imageRect.Right;
                    break;
                case GropImage.EdgeType.Bottom:
                    newCoordinate = imageRect.Bottom;
                    break;
            }

            float offset = newCoordinate - oldCoordinate;
            return offset;
        }

        /**
         * Snap this Edge to the given View boundaries.
         * 
         * @param view the View to snap to
         */
        public void SnapToView(View view)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));

            switch (mEdgeType)
            {
                case GropImage.EdgeType.Left:
                    Coordinate = 0;
                    break;
                case GropImage.EdgeType.Top:
                    Coordinate = 0;
                    break;
                case GropImage.EdgeType.Right:
                    Coordinate = view.Width;
                    break;
                case GropImage.EdgeType.Bottom:
                    Coordinate = view.Height;
                    break;
            }
        }

        /**
         * Gets the current width of the crop window.
         */
        public static float GetWidth()
        {
            return EdgeManager.Right.Coordinate - EdgeManager.Left.Coordinate;
        }

        /**
         * Gets the current height of the crop window.
         */
        public static float GetHeight()
        {
            return EdgeManager.Bottom.Coordinate - EdgeManager.Top.Coordinate;
        }

        /**
         * Determines if this Edge is outside the inner margins of the given bounding
         * rectangle. The margins come inside the actual frame by SNAPRADIUS amount; 
         * therefore, determines if the point is outside the inner "margin" frame.
         * 
         */
        public bool IsOutsideMargin(Rect rect, float margin)
        {
            if (rect == null) throw new ArgumentNullException(nameof(rect));

            bool result = false;

            switch (mEdgeType)
            {
                case GropImage.EdgeType.Left:
                    result = Coordinate - rect.Left < margin;
                    break;
                case GropImage.EdgeType.Top:
                    result = Coordinate - rect.Top < margin;
                    break;
                case GropImage.EdgeType.Right:
                    result = rect.Right - Coordinate < margin;
                    break;
                case GropImage.EdgeType.Bottom:
                    result = rect.Bottom - Coordinate < margin;
                    break;
            }
            return result;
        }

        /**
         * Determines if this Edge is outside the image frame of the given bounding
         * rectangle.
         */
        public bool IsOutsideFrame(Rect rect)
        {
            if (rect == null) throw new ArgumentNullException(nameof(rect));

            double margin = 0;
            bool result = false;

            switch (mEdgeType)
            {
                case GropImage.EdgeType.Left:
                    result = Coordinate - rect.Left < margin;
                    break;
                case GropImage.EdgeType.Top:
                    result = Coordinate - rect.Top < margin;
                    break;
                case GropImage.EdgeType.Right:
                    result = rect.Right - Coordinate < margin;
                    break;
                case GropImage.EdgeType.Bottom:
                    result = rect.Bottom - Coordinate < margin;
                    break;
            }
            return result;
        }


        // Private Methods /////////////////////////////////////////////////////////

        /**
         * Get the resulting x-position of the left edge of the crop window given
         * the handle's position and the image's bounding box and snap radius.
         * 
         * @param x the x-position that the left edge is dragged to
         * @param imageRect the bounding box of the image that is being cropped
         * @param imageSnapRadius the snap distance to the image edge (in pixels)
         * @return the actual x-position of the left edge
         */
        private static float AdjustLeft(float x, Rect imageRect, float imageSnapRadius, float aspectRatio)
        {
            float resultX = x;

            if (x - imageRect.Left < imageSnapRadius)
                resultX = imageRect.Left;

            else
            {
                // Select the minimum of the three possible values to use
                float resultXHoriz = float.PositiveInfinity;
                float resultXVert = float.PositiveInfinity;

                // Checks if the window is too small horizontally
                if (x >= EdgeManager.Right.Coordinate - MinCropLengthPx)
                    resultXHoriz = EdgeManager.Right.Coordinate - MinCropLengthPx;

                // Checks if the window is too small vertically
                if (((EdgeManager.Right.Coordinate - x) / aspectRatio) <= MinCropLengthPx)
                    resultXVert = EdgeManager.Right.Coordinate - (MinCropLengthPx * aspectRatio);

                resultX = Math.Min(resultX, Math.Min(resultXHoriz, resultXVert));
            }
            return resultX;
        }

        /**
         * Get the resulting x-position of the right edge of the crop window given
         * the handle's position and the image's bounding box and snap radius.
         * 
         * @param x the x-position that the right edge is dragged to
         * @param imageRect the bounding box of the image that is being cropped
         * @param imageSnapRadius the snap distance to the image edge (in pixels)
         * @return the actual x-position of the right edge
         */
        private static float AdjustRight(float x, Rect imageRect, float imageSnapRadius, float aspectRatio)
        {
            float resultX = x;

            // If close to the edge
            if (imageRect.Right - x < imageSnapRadius)
                resultX = imageRect.Right;

            else
            {
                // Select the maximum of the three possible values to use
                float resultXHoriz = float.NegativeInfinity;
                float resultXVert = float.NegativeInfinity;

                // Checks if the window is too small horizontally
                if (x <= EdgeManager.Left.Coordinate + MinCropLengthPx)
                    resultXHoriz = EdgeManager.Left.Coordinate + MinCropLengthPx;

                // Checks if the window is too small vertically
                if (((x - EdgeManager.Left.Coordinate) / aspectRatio) <= MinCropLengthPx)
                {
                    resultXVert = EdgeManager.Left.Coordinate + (MinCropLengthPx * aspectRatio);
                }

                resultX = Math.Max(resultX, Math.Max(resultXHoriz, resultXVert));

            }

            return resultX;
        }

        /**
         * Get the resulting y-position of the top edge of the crop window given the
         * handle's position and the image's bounding box and snap radius.
         * 
         * @param y the x-position that the top edge is dragged to
         * @param imageRect the bounding box of the image that is being cropped
         * @param imageSnapRadius the snap distance to the image edge (in pixels)
         * @return the actual y-position of the top edge
         */
        private static float AdjustTop(float y, Rect imageRect, float imageSnapRadius, float aspectRatio)
        {

            float resultY = y;

            if (y - imageRect.Top < imageSnapRadius)
                resultY = imageRect.Top;

            else
            {
                // Select the minimum of the three possible values to use
                float resultYVert = float.PositiveInfinity;
                float resultYHoriz = float.PositiveInfinity;

                // Checks if the window is too small vertically
                if (y >= EdgeManager.Bottom.Coordinate - MinCropLengthPx)
                    resultYHoriz = EdgeManager.Bottom.Coordinate - MinCropLengthPx;

                // Checks if the window is too small horizontally
                if (((EdgeManager.Bottom.Coordinate - y) * aspectRatio) <= MinCropLengthPx)
                    resultYVert = EdgeManager.Bottom.Coordinate - (MinCropLengthPx / aspectRatio);

                resultY = Math.Min(resultY, Math.Min(resultYHoriz, resultYVert));

            }

            return resultY;
        }

        /**
         * Get the resulting y-position of the bottom edge of the crop window given
         * the handle's position and the image's bounding box and snap radius.
         * 
         * @param y the x-position that the bottom edge is dragged to
         * @param imageRect the bounding box of the image that is being cropped
         * @param imageSnapRadius the snap distance to the image edge (in pixels)
         * @return the actual y-position of the bottom edge
         */
        private static float AdjustBottom(float y, Rect imageRect, float imageSnapRadius, float aspectRatio)
        {
            float resultY = y;

            if (imageRect.Bottom - y < imageSnapRadius)
                resultY = imageRect.Bottom;
            else
            {
                // Select the maximum of the three possible values to use
                float resultYVert = float.NegativeInfinity;
                float resultYHoriz = float.NegativeInfinity;

                // Checks if the window is too small vertically
                if (y <= EdgeManager.Top.Coordinate + MinCropLengthPx)
                    resultYVert = EdgeManager.Top.Coordinate + MinCropLengthPx;

                // Checks if the window is too small horizontally
                if (((y - EdgeManager.Top.Coordinate) * aspectRatio) <= MinCropLengthPx)
                    resultYHoriz = EdgeManager.Top.Coordinate + (MinCropLengthPx / aspectRatio);

                resultY = Math.Max(resultY, Math.Max(resultYHoriz, resultYVert));
            }

            return resultY;
        }
    }
}
