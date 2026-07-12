namespace Nivaes.App.Cross.Droid.GropImage
{
    using System;
    using Android.Content;
    using Android.Graphics;
    using Android.Runtime;
    using Android.Util;
    using Android.Views;
    using Java.Lang;
    using Math = System.Math;

    [Register("com.nivaes.CropOverlayView")]
    public class CropOverlayView : View
    {
        // Private Constants ///////////////////////////////////////////////////////

        private static readonly int SNAP_RADIUS_DP = 6;
        private static readonly float DEFAULT_SHOW_GUIDELINES_LIMIT = 100;

        // Gets default values from PaintUtil, sets a bunch of values such that the
        // corners will draw correctly
        private static readonly float DEFAULT_CORNER_THICKNESS_DP = PaintUtil.GetCornerThickness();
        private static readonly float DEFAULT_LINE_THICKNESS_DP = PaintUtil.GetLineThickness();

        private static readonly float DEFAULT_CORNER_OFFSET_DP = (DEFAULT_CORNER_THICKNESS_DP / 2) -
                                                                 (DEFAULT_LINE_THICKNESS_DP / 2);

        private static readonly float DEFAULT_CORNER_EXTENSION_DP = DEFAULT_CORNER_THICKNESS_DP / 2 +
                                                                    DEFAULT_CORNER_OFFSET_DP;

        private static readonly float DEFAULT_CORNER_LENGTH_DP = 20;



        // Member Variables ////////////////////////////////////////////////////////

        // The Paint used to draw the white rectangle around the crop area.

        // Floats to save the current aspect ratio of the image
        private static int mAspectRatioX = CropImageView.DefaultAspectRatioX;
        private static int mAspectRatioY = CropImageView.DefaultAspectRatioY;

        // The aspect ratio that the crop area should maintain; this variable is
        // only used when mMaintainAspectRatio is true.

        // Whether the Crop View has been initialized for the first time
        private bool initializedCropWindow;
        private Paint mBackgroundPaint;

        // The bounding box around the Bitmap that we are cropping.
        private Rect mBitmapRect;
        private Paint mBorderPaint;

        // Instance variables for the corner values
        private float mCornerExtension;
        private float mCornerLength;
        private float mCornerOffset;
        private Paint mCornerPaint;
        private bool mFixAspectRatio = CropImageView.DefaultFixedAspectRatio;
        private Paint mGuidelinePaint;
        private CropGuidelines mGuidelines;
        private CropShapeSelector mShapeSelector;
        private float mHandleRadius;
        private Handle mPressedHandle;
        private float mSnapRadius;
        private float mTargetAspectRatio = ((float)mAspectRatioX) / mAspectRatioY;
        private Android.Util.Pair mTouchOffset;

        // Constructors ////////////////////////////////////////////////////////////

        protected CropOverlayView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        { }

        public CropOverlayView(Context context)
            : base(context)
        { }

        public CropOverlayView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            Init(context);
        }

        public CropOverlayView(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            Init(context);
        }

        // View Methods ///////////////////////////////////////////////////////////

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            // Initialize the crop window here because we need the size of the view
            // to have been determined.
            InitCropWindow(mBitmapRect);
        }

        protected override void OnDraw(Canvas canvas)
        {
            if (canvas == null) throw new ArgumentNullException(nameof(canvas));

            base.OnDraw(canvas);

            // Draw translucent background for the cropped area.
            DrawBackground(canvas, mBitmapRect);

            if (ShowGuidelines())
            {
                // Determines whether guidelines should be drawn or not
                if (mGuidelines == CropGuidelines.On)
                {
                    DrawRuleOfThirdsGuidelines(canvas);
                }
                else if (mGuidelines == CropGuidelines.OnTouch)
                {
                    // Draw only when resizing
                    if (mPressedHandle != null)
                        DrawRuleOfThirdsGuidelines(canvas);
                }
                else if (mGuidelines == CropGuidelines.Off)
                {
                    // Do nothing
                }
            }
            // Draw the circular border
            if (mShapeSelector == CropShapeSelector.Circle)
            {
                float cx = (EdgeManager.Left.Coordinate + EdgeManager.Right.Coordinate) / 2;
                float cy = (EdgeManager.Top.Coordinate + EdgeManager.Bottom.Coordinate) / 2;
                float radius = (EdgeManager.Right.Coordinate - EdgeManager.Left.Coordinate) / 2;

                canvas.DrawCircle(cx, cy, radius, mBorderPaint);
            }
            else
            {
                canvas.DrawRect(EdgeManager.Left.Coordinate, EdgeManager.Top.Coordinate, EdgeManager.Right.Coordinate, EdgeManager.Bottom.Coordinate, mBorderPaint);
            }
        }

        public override bool OnTouchEvent(MotionEvent ev)
        {
            if (ev == null) throw new ArgumentNullException(nameof(ev));

            // If this View is not enabled, don't allow for touch interactions.
            if (!Enabled)
            {
                return false;
            }

            switch (ev.Action)
            {
                case MotionEventActions.Down:
                    OnActionDown(ev.GetX(), ev.GetY());
                    return true;

                case MotionEventActions.Up:
                case MotionEventActions.Cancel:
                    Parent.RequestDisallowInterceptTouchEvent(false);
                    OnActionUp();
                    return true;

                case MotionEventActions.Move:
                    OnActionMove(ev.GetX(), ev.GetY());
                    Parent.RequestDisallowInterceptTouchEvent(true);
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Informs the CropOverlayView of the image's position relative to the
        /// ImageView. This is necessary to call in order to draw the crop window.
        /// </summary>
        /// <param name="bitmapRect">The image's bounding box</param>
        public void SetBitmapRect(Rect bitmapRect)
        {
            if (bitmapRect == null) throw new ArgumentNullException(nameof(bitmapRect));

            mBitmapRect = bitmapRect;
            InitCropWindow(mBitmapRect);
        }

        /// <summary>
        /// Resets the crop overlay view.
        /// </summary>
        public void ResetCropOverlayView()
        {
            if (initializedCropWindow)
            {
                InitCropWindow(mBitmapRect);
                Invalidate();
            }
        }

        /// <summary>
        /// Sets the guidelines for the CropOverlayView to be either on, off, or to
        /// show when resizing the application.
        /// </summary>
        /// <param name="guidelines">Integer that signals whether the guidelines should be on, off, or only showing when resizing.</param>
        public void SetGuidelines(CropGuidelines guidelines)
        {
            mGuidelines = guidelines;

            if (initializedCropWindow)
            {
                InitCropWindow(mBitmapRect);
                Invalidate();
            }
        }

        /// <summary>
        /// Sets whether the aspect ratio is fixed or not; true fixes the aspect ratio, while false allows it to be changed.
        /// </summary>
        /// <param name="fixAspectRatio">Bool that signals whether the aspect ratio should be maintained.</param>
        public void SetFixedAspectRatio(bool fixAspectRatio)
        {
            mFixAspectRatio = fixAspectRatio;

            if (initializedCropWindow)
            {
                InitCropWindow(mBitmapRect);
                Invalidate();
            }
        }

        /// <summary>
        /// Sets the X value of the aspect ratio; is defaulted to 1.
        /// </summary>
        /// <param name="aspectRatioX">That specifies the new X value of the aspect ratio</param>
        public void SetAspectRatioX(int aspectRatioX)
        {
            if (aspectRatioX <= 0)
                throw new IllegalArgumentException("Cannot set aspect ratio value to a number less than or equal to 0.");
            mAspectRatioX = aspectRatioX;
            mTargetAspectRatio = ((float)mAspectRatioX) / mAspectRatioY;

            if (initializedCropWindow)
            {
                InitCropWindow(mBitmapRect);
                Invalidate();
            }
        }

        /// <summary>
        /// Sets the Y value of the aspect ratio; is defaulted to 1.
        /// </summary>
        /// <param name="aspectRatioY">That specifies the new Y value of the aspect ratio</param>
        public void SetAspectRatioY(int aspectRatioY)
        {
            if (aspectRatioY <= 0)
                throw new IllegalArgumentException(
                    "Cannot set aspect ratio value to a number less than or equal to 0.");
            mAspectRatioY = aspectRatioY;
            mTargetAspectRatio = ((float)mAspectRatioX) / mAspectRatioY;

            if (initializedCropWindow)
            {
                InitCropWindow(mBitmapRect);
                Invalidate();
            }
        }

        /// <summary>
        /// Sets all initial values, but does not call InitCropWindow to reset the views. Used once at the very start to initialize the attributes.
        /// </summary>
        /// <param name="guidelines">Integer that signals whether the guidelines should be on, off, or only showing when resizing.</param>
        /// <param name="shapeSelector">Integer that signals whether the guidelines should be on, off, or only showing when resizing.</param>
        /// <param name="fixAspectRatio">bool that signals whether the aspect ratio should be maintained.</param>
        /// <param name="aspectRatioX">float that specifies the new X value of the aspect ratio</param>
        /// <param name="aspectRatioY">float that specifies the new Y value of the aspect ratio</param>
        public void SetInitialAttributeValues(CropGuidelines guidelines,
            CropShapeSelector shapeSelector,
            bool fixAspectRatio,
            int aspectRatioX,
            int aspectRatioY)
        {
            mGuidelines = guidelines;
            mShapeSelector = shapeSelector;

            mFixAspectRatio = fixAspectRatio;

            if (aspectRatioX <= 0)
                throw new IllegalArgumentException("Cannot set aspect ratio value to a number less than or equal to 0.");
            mAspectRatioX = aspectRatioX;
            mTargetAspectRatio = ((float)mAspectRatioX) / mAspectRatioY;

            if (aspectRatioY <= 0)
                throw new IllegalArgumentException("Cannot set aspect ratio value to a number less than or equal to 0.");
            mAspectRatioY = aspectRatioY;
            mTargetAspectRatio = ((float)mAspectRatioX) / mAspectRatioY;
        }

        private void Init(Context context)
        {
            DisplayMetrics displayMetrics = context.Resources.DisplayMetrics;
            mHandleRadius = HandleUtil.GetTargetRadius(context);
            mSnapRadius = TypedValue.ApplyDimension(ComplexUnitType.Dip, SNAP_RADIUS_DP, displayMetrics);
            mBorderPaint = PaintUtil.NewBorderPaint(context);
            mGuidelinePaint = PaintUtil.NewGuidelinePaint();
            mBackgroundPaint = PaintUtil.NewBackgroundPaint(context);
            mCornerPaint = PaintUtil.NewCornerPaint(context);
            // Sets the values for the corner sizes
            mCornerOffset = TypedValue.ApplyDimension(ComplexUnitType.Dip, DEFAULT_CORNER_OFFSET_DP, displayMetrics);
            mCornerExtension = TypedValue.ApplyDimension(ComplexUnitType.Dip, DEFAULT_CORNER_EXTENSION_DP, displayMetrics);
            mCornerLength = TypedValue.ApplyDimension(ComplexUnitType.Dip, DEFAULT_CORNER_LENGTH_DP, displayMetrics);
            // Sets guidelines to default until specified otherwise
            mGuidelines = CropImageView.DefaultGuidelines;
            mShapeSelector = CropImageView.DefaultShapeSelector;
        }

        /// <summary>
        ///  Set the initial crop window size and position. This is dependent on the size and position of the image being cropped.
        /// </summary>
        /// <param name="bitmapRect">The bounding box around the image being cropped</param>
        private void InitCropWindow(Rect bitmapRect)
        {
            // Tells the attribute functions the crop window has already been
            // initialized
            if (initializedCropWindow == false)
                initializedCropWindow = true;

            if (mFixAspectRatio)
            {
                // If the image aspect ratio is wider than the crop aspect ratio,
                // then the image height is the determining initial length. Else,
                // vice-versa.
                if (AspectRatioUtil.CalculateAspectRatio(bitmapRect) > mTargetAspectRatio)
                {
                    EdgeManager.Top.Coordinate = bitmapRect.Top;
                    EdgeManager.Bottom.Coordinate = bitmapRect.Bottom;

                    float centerX = Width / 2f;

                    // Limits the aspect ratio to no less than 40 wide or 40 tall
                    float cropWidth = Math.Max(Edge.MinCropLengthPx,
                        AspectRatioUtil.CalculateWidth(EdgeManager.Top.Coordinate,
                            EdgeManager.Bottom.Coordinate,
                            mTargetAspectRatio));

                    // Create new TargetAspectRatio if the original one does not fit
                    // the screen
                    if (cropWidth == Edge.MinCropLengthPx)
                        mTargetAspectRatio = (Edge.MinCropLengthPx) /
                                             (EdgeManager.Bottom.Coordinate - EdgeManager.Top.Coordinate);

                    float halfCropWidth = cropWidth / 2f;
                    EdgeManager.Left.Coordinate = (centerX - halfCropWidth);
                    EdgeManager.Right.Coordinate = (centerX + halfCropWidth);
                }
                else
                {
                    EdgeManager.Left.Coordinate = bitmapRect.Left;
                    EdgeManager.Right.Coordinate = bitmapRect.Right;

                    float centerY = Height / 2f;

                    // Limits the aspect ratio to no less than 40 wide or 40 tall
                    float cropHeight = Math.Max(Edge.MinCropLengthPx,
                        AspectRatioUtil.CalculateHeight(EdgeManager.Left.Coordinate,
                            EdgeManager.Right.Coordinate,
                            mTargetAspectRatio));

                    // Create new TargetAspectRatio if the original one does not fit
                    // the screen
                    if (cropHeight == Edge.MinCropLengthPx)
                        mTargetAspectRatio = (EdgeManager.Right.Coordinate - EdgeManager.Left.Coordinate) /
                                             Edge.MinCropLengthPx;

                    float halfCropHeight = cropHeight / 2f;
                    EdgeManager.Top.Coordinate = (centerY - halfCropHeight);
                    EdgeManager.Bottom.Coordinate = (centerY + halfCropHeight);
                }
            }
            else
            {
                // ... do not fix aspect ratio...

                // Initialize crop window to have 10% padding w/ respect to image.
                float horizontalPadding = 0.1f * bitmapRect.Width();
                float verticalPadding = 0.1f * bitmapRect.Height();

                EdgeManager.Left.Coordinate = (bitmapRect.Left + horizontalPadding);
                EdgeManager.Top.Coordinate = (bitmapRect.Top + verticalPadding);
                EdgeManager.Right.Coordinate = (bitmapRect.Right - horizontalPadding);
                EdgeManager.Bottom.Coordinate = (bitmapRect.Bottom - verticalPadding);
            }
        }

        /// <summary>
        /// Indicates whether the crop window is small enough that the guidelines
        /// should be shown. Public because this function is also used to determine
        /// if the center handle should be focused.
        /// </summary>
        /// <returns>Whether the guidelines should be shown or not</returns>
        public static bool ShowGuidelines()
        {
            if ((Math.Abs(EdgeManager.Left.Coordinate - EdgeManager.Right.Coordinate) < DEFAULT_SHOW_GUIDELINES_LIMIT)
                ||
                (Math.Abs(EdgeManager.Top.Coordinate - EdgeManager.Bottom.Coordinate) < DEFAULT_SHOW_GUIDELINES_LIMIT))
                return false;
            return true;
        }

        private void DrawRuleOfThirdsGuidelines(Canvas canvas)
        {
            float left = EdgeManager.Left.Coordinate;
            float top = EdgeManager.Top.Coordinate;
            float right = EdgeManager.Right.Coordinate;
            float bottom = EdgeManager.Bottom.Coordinate;

            var circleSelectionPath = new Path();
            if (mShapeSelector == CropShapeSelector.Circle)
            {
                float cx = (left + right) / 2;
                float cy = (top + bottom) / 2;
                float radius = (right - left) / 2;

                circleSelectionPath.AddCircle(cx, cy, radius, Path.Direction.Cw);
            }
            else
            {
                circleSelectionPath.AddRect(left, top, right, bottom, Path.Direction.Ccw);
            }

            canvas.ClipPath(circleSelectionPath, Region.Op.Replace);
            //canvas.ClipOutPath(circleSelectionPath);

            // Draw vertical guidelines.
            float oneThirdCropWidth = Edge.GetWidth() / 3;

            float x1 = left + oneThirdCropWidth;
            canvas.DrawLine(x1, top, x1, bottom, mGuidelinePaint);
            float x2 = right - oneThirdCropWidth;
            canvas.DrawLine(x2, top, x2, bottom, mGuidelinePaint);

            // Draw horizontal guidelines.
            float oneThirdCropHeight = Edge.GetHeight() / 3;

            float y1 = top + oneThirdCropHeight;
            canvas.DrawLine(left, y1, right, y1, mGuidelinePaint);
            float y2 = bottom - oneThirdCropHeight;
            canvas.DrawLine(left, y2, right, y2, mGuidelinePaint);
        }

        private void DrawBackground(Canvas canvas, Rect bitmapRect)
        {
            float left = EdgeManager.Left.Coordinate;
            float top = EdgeManager.Top.Coordinate;
            float right = EdgeManager.Right.Coordinate;
            float bottom = EdgeManager.Bottom.Coordinate;

            var fullCanvasPath = new Path();
            fullCanvasPath.AddRect(bitmapRect.Left, bitmapRect.Top, bitmapRect.Right, bitmapRect.Bottom,
                Path.Direction.Cw);

            var circleSelectionPath = new Path();
            if (mShapeSelector == CropShapeSelector.Circle)
            {
                float cx = (left + right) / 2;
                float cy = (top + bottom) / 2;
                float radius = (right - left) / 2;

                circleSelectionPath.AddCircle(cx, cy, radius, Path.Direction.Ccw);
            }
            else
            {
                circleSelectionPath.AddRect(left, top, right, bottom, Path.Direction.Ccw);
            }
            canvas.ClipPath(fullCanvasPath);
            canvas.ClipPath(circleSelectionPath, Region.Op.Difference);
            //canvas.ClipOutPath(circleSelectionPath);

            //Draw semi-transparent background
            canvas.DrawRect(bitmapRect.Left, bitmapRect.Top, bitmapRect.Right, bitmapRect.Bottom, mBackgroundPaint);
        }

        private void DrawCorners(Canvas canvas)
        {
            float left = EdgeManager.Left.Coordinate;
            float top = EdgeManager.Top.Coordinate;
            float right = EdgeManager.Right.Coordinate;
            float bottom = EdgeManager.Bottom.Coordinate;

            // Draws the corner lines

            // Top left
            canvas.DrawLine(left - mCornerOffset,
                top - mCornerExtension,
                left - mCornerOffset,
                top + mCornerLength,
                mCornerPaint);
            canvas.DrawLine(left, top - mCornerOffset, left + mCornerLength, top - mCornerOffset, mCornerPaint);

            // Top right
            canvas.DrawLine(right + mCornerOffset,
                top - mCornerExtension,
                right + mCornerOffset,
                top + mCornerLength,
                mCornerPaint);
            canvas.DrawLine(right, top - mCornerOffset, right - mCornerLength, top - mCornerOffset, mCornerPaint);

            // Bottom left
            canvas.DrawLine(left - mCornerOffset,
                bottom + mCornerExtension,
                left - mCornerOffset,
                bottom - mCornerLength,
                mCornerPaint);
            canvas.DrawLine(left,
                bottom + mCornerOffset,
                left + mCornerLength,
                bottom + mCornerOffset,
                mCornerPaint);

            // Bottom left
            canvas.DrawLine(right + mCornerOffset,
                bottom + mCornerExtension,
                right + mCornerOffset,
                bottom - mCornerLength,
                mCornerPaint);
            canvas.DrawLine(right,
                bottom + mCornerOffset,
                right - mCornerLength,
                bottom + mCornerOffset,
                mCornerPaint);
        }

        /// <summary>
        /// Handles a {@link MotionEvent#ACTION_DOWN} event.
        /// </summary>
        /// <param name="x">The x-coordinate of the down action</param>
        /// <param name="y">The y-coordinate of the down action</param>
        private void OnActionDown(float x, float y)
        {
            float left = EdgeManager.Left.Coordinate;
            float top = EdgeManager.Top.Coordinate;
            float right = EdgeManager.Right.Coordinate;
            float bottom = EdgeManager.Bottom.Coordinate;

            mPressedHandle = HandleUtil.GetPressedHandle(x, y, left, top, right, bottom, mHandleRadius);

            if (mPressedHandle == null)
                return;

            // Calculate the offset of the touch point from the precise location
            // of the handle. Save these values in a member variable since we want
            // to maintain this offset as we drag the handle.
            mTouchOffset = HandleUtil.GetOffset(mPressedHandle, x, y, left, top, right, bottom);

            Invalidate();
        }

        /// <summary>
        /// Handles a {@link MotionEvent#ACTION_UP} or {@link MotionEvent#ACTION_CANCEL} event.
        /// </summary>
        private void OnActionUp()
        {
            if (mPressedHandle == null)
                return;

            mPressedHandle = null;

            Invalidate();
        }

        /// <summary>
        /// Handles a {@link MotionEvent#ACTION_MOVE} event.
        /// </summary>
        /// <param name="x">The x-coordinate of the move event</param>
        /// <param name="y">The y-coordinate of the move event</param>
        private void OnActionMove(float x, float y)
        {
            if (mPressedHandle == null)
                return;

            // Adjust the coordinates for the finger position's offset (i.e. the
            // distance from the initial touch to the precise handle location).
            // We want to maintain the initial touch's distance to the pressed
            // handle so that the crop window size does not "jump".
            //TODO: FIX
            x += (float)mTouchOffset.First;
            y += (float)mTouchOffset.Second;

            // Calculate the new crop window size/position.
            if (mFixAspectRatio)
            {
                mPressedHandle.UpdateCropWindow(x, y, mTargetAspectRatio, mBitmapRect, mSnapRadius);
            }
            else
            {
                mPressedHandle.UpdateCropWindow(x, y, mBitmapRect, mSnapRadius);
            }
            Invalidate();
        }
    }
}
