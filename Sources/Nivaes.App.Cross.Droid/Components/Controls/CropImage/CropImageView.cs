using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Orientation = Android.Media.Orientation;

namespace Nivaes.App.Cross.Droid.GropImage
{
    // ToDo: Meter CropImageView en un componente a parte.
    [Register("com.nivaes.CropImageView")]
    public class CropImageView : FrameLayout
    {
        // Private Constants ///////////////////////////////////////////////////////
        private static readonly Rect EmptyRect = new Rect();

        // Sets the default image guidelines to show when resizing
        public static readonly CropGuidelines DefaultGuidelines = CropGuidelines.OnTouch;
        public static readonly CropShapeSelector DefaultShapeSelector = CropShapeSelector.Rectangel;
        public static bool DefaultFixedAspectRatio = true;
        public static int DefaultAspectRatioX = 1;
        public static int DefaultAspectRatioY = 1;

        private static readonly int DEFAULT_IMAGE_RESOURCE = 0;

        private static readonly string DEGREES_ROTATED = "DEGREES_ROTATED";

        private readonly bool mFixAspectRatio = DefaultFixedAspectRatio;
        private readonly CropGuidelines mGuidelines = DefaultGuidelines;
        private readonly CropShapeSelector mShapeSelector = DefaultShapeSelector;
        private readonly int mImageResource = DEFAULT_IMAGE_RESOURCE;
        private int mAspectRatioX = DefaultAspectRatioX;
        private int mAspectRatioY = DefaultAspectRatioY;
        private Bitmap mBitmap;
        private CropOverlayView mCropOverlayView;
        private int mDegreesRotated;
        private ImageView mImageView;
        private int mLayoutHeight;
        private int mLayoutWidth;

        // Constructors ////////////////////////////////////////////////////////////

        protected CropImageView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        { }

        public CropImageView(Context context)
            : base(context)
        {
            Init(context);
        }

        public CropImageView(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
            Init(context);
        }

        public CropImageView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            TypedArray ta = context.ObtainStyledAttributes(attrs, Resource.Styleable.CropImageView, 0, 0);
            try
            {
                mGuidelines = (CropGuidelines)ta.GetInteger(Resource.Styleable.CropImageView_guidelines, (int)DefaultGuidelines);
                mShapeSelector = (CropShapeSelector)ta.GetInteger(Resource.Styleable.CropImageView_shapeSelector, (int)DefaultShapeSelector);
                mFixAspectRatio = ta.GetBoolean(Resource.Styleable.CropImageView_fixAspectRatio, DefaultFixedAspectRatio);
                mAspectRatioX = ta.GetInteger(Resource.Styleable.CropImageView_aspectRatioX, DefaultAspectRatioX);
                mAspectRatioY = ta.GetInteger(Resource.Styleable.CropImageView_aspectRatioY, DefaultAspectRatioY);
                mImageResource = ta.GetResourceId(Resource.Styleable.CropImageView_imageResource, DEFAULT_IMAGE_RESOURCE);
            }
            finally
            {
                ta.Recycle();
            }

            Init(context);
        }

        protected override IParcelable OnSaveInstanceState()
        {
            var bundle = new Bundle();

            bundle.PutParcelable("instanceState", base.OnSaveInstanceState());
            bundle.PutInt(DEGREES_ROTATED, mDegreesRotated);

            return bundle;
        }

        protected override void OnRestoreInstanceState(IParcelable parcelable)
        {
            if (parcelable is Bundle bundle)
            {

                // Fixes the rotation of the image when orientation changes.
                mDegreesRotated = bundle.GetInt(DEGREES_ROTATED);
                int tempDegrees = mDegreesRotated;
                RotateImage(mDegreesRotated);
                mDegreesRotated = tempDegrees;
                //TODO: THIS SHOULD WORK, FIX
                base.OnRestoreInstanceState(bundle.GetParcelable("instanceState").JavaCast<IParcelable>());
            }
            else
            {
                base.OnRestoreInstanceState(parcelable);
            }
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            if (mBitmap != null)
            {
                Rect bitmapRect = ImageViewUtil.GetBitmapRectCenterInside(mBitmap, this);
                mCropOverlayView.SetBitmapRect(bitmapRect);
            }
            else
            {
                mCropOverlayView.SetBitmapRect(EmptyRect);
            }
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            var widthMode = (int)MeasureSpec.GetMode(widthMeasureSpec);
            int widthSize = MeasureSpec.GetSize(widthMeasureSpec);
            var heightMode = (int)MeasureSpec.GetMode(heightMeasureSpec);
            int heightSize = MeasureSpec.GetSize(heightMeasureSpec);

            if (mBitmap != null)
            {
                base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

                // Bypasses a baffling bug when used within a ScrollView, where
                // heightSize is set to 0.
                if (heightSize == 0)
                    heightSize = mBitmap.Height;

                int desiredWidth;
                int desiredHeight;

                double viewToBitmapWidthRatio = double.PositiveInfinity;
                double viewToBitmapHeightRatio = double.PositiveInfinity;

                // Checks if either width or height needs to be fixed
                if (widthSize < mBitmap.Width)
                {
                    viewToBitmapWidthRatio = widthSize / (double)mBitmap.Width;
                }
                if (heightSize < mBitmap.Height)
                {
                    viewToBitmapHeightRatio = heightSize / (double)mBitmap.Height;
                }

                // If either needs to be fixed, choose smallest ratio and calculate
                // from there
                if (viewToBitmapWidthRatio != double.PositiveInfinity ||
                    viewToBitmapHeightRatio != double.PositiveInfinity)
                {
                    if (viewToBitmapWidthRatio <= viewToBitmapHeightRatio)
                    {
                        desiredWidth = widthSize;
                        desiredHeight = (int)(mBitmap.Height * viewToBitmapWidthRatio);
                    }
                    else
                    {
                        desiredHeight = heightSize;
                        desiredWidth = (int)(mBitmap.Width * viewToBitmapHeightRatio);
                    }
                }
                else
                {
                    desiredWidth = mBitmap.Width;
                    desiredHeight = mBitmap.Height;
                }

                int width = GetOnMeasureSpec(widthMode, widthSize, desiredWidth);
                int height = GetOnMeasureSpec(heightMode, heightSize, desiredHeight);

                mLayoutWidth = width;
                mLayoutHeight = height;

                Rect bitmapRect = ImageViewUtil.GetBitmapRectCenterInside(mBitmap.Width,
                    mBitmap.Height,
                    mLayoutWidth,
                    mLayoutHeight);
                mCropOverlayView.SetBitmapRect(bitmapRect);

                // MUST CALL THIS
                SetMeasuredDimension(mLayoutWidth, mLayoutHeight);
            }
            else
            {
                mCropOverlayView.SetBitmapRect(EmptyRect);
                SetMeasuredDimension(widthSize, heightSize);
            }
        }

        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            base.OnLayout(changed, left, top, right, bottom);

            if (mLayoutWidth > 0 && mLayoutHeight > 0)
            {
                // Gets original parameters, and creates the new parameters
                ViewGroup.LayoutParams origparams = LayoutParameters;
                origparams.Width = mLayoutWidth;
                origparams.Height = mLayoutHeight;
                LayoutParameters = origparams;
            }
        }

        /// <summary>
        /// Returns the integer of the imageResource.
        /// </summary>
        /// <returns>The image resource id.</returns>
        public int GetImageResource()
        {
            return mImageResource;
        }

        /// <summary>
        /// Sets a Bitmap as the content of the CropImageView.
        /// </summary>
        /// <param name="bitmap">The Bitmap to set</param>
        public void SetImageBitmap(Bitmap bitmap)
        {
            mBitmap = bitmap;
            mImageView.SetImageBitmap(mBitmap);

            if (mCropOverlayView != null)
            {
                mCropOverlayView.ResetCropOverlayView();
            }
        }

        /// <summary>
        /// Sets a Bitmap and initializes the image rotation according to the EXIT data.
        /// <p>
        /// The EXIF can be retrieved by doing the following:
        /// <code>ExifInterface exif = new ExifInterface(path);</code>
        /// </summary>
        /// <param name="bitmap">The original bitmap to set; if null, this</param>
        /// <param name="exif">The EXIF information about this bitmap; may be null</param>
        public void SetImageBitmap(Bitmap bitmap, ExifInterface exif)
        {
            if (bitmap == null)
            {
                return;
            }

            if (exif == null)
            {
                SetImageBitmap(bitmap);
                return;
            }

            var matrix = new Matrix();
            int orientation = exif.GetAttributeInt(ExifInterface.TagOrientation, 1);
            int rotate = -1;
            //TODO CHECK THIS FIX
            switch (orientation)
            {
                case (int)Orientation.Rotate270:
                    rotate = 270;
                    break;
                case (int)Orientation.Rotate180:
                    rotate = 180;
                    break;
                case (int)Orientation.Rotate90:
                    rotate = 90;
                    break;
            }

            if (rotate == -1)
            {
                SetImageBitmap(bitmap);
            }
            else
            {
                matrix.PostRotate(rotate);
                Bitmap rotatedBitmap = Bitmap.CreateBitmap(bitmap,
                    0,
                    0,
                    bitmap.Width,
                    bitmap.Height,
                    matrix,
                    true);
                SetImageBitmap(rotatedBitmap);
                bitmap.Recycle();
            }
        }

        /// <summary>
        /// Sets a Drawable as the content of the CropImageView.
        /// </summary>
        /// <param name="resId">The drawable resource ID to set</param>
        public void SetImageResource(int resId)
        {
            if (resId != 0)
            {
                Bitmap bitmap = BitmapFactory.DecodeResource(Resources, resId);
                SetImageBitmap(bitmap);
            }
        }

        /// <summary>
        /// Gets the cropped image based on the current crop window.
        /// </summary>
        /// <returns>return a new Bitmap representing the cropped image</returns>
        public Bitmap GetCroppedImage()
        {
            using (Rect displayedImageRect = ImageViewUtil.GetBitmapRectCenterInside(mBitmap, mImageView))
            {
                // Get the scale factor between the actual Bitmap dimensions and the
                // displayed dimensions for width.
                float actualImageWidth = mBitmap.Width;
                float displayedImageWidth = displayedImageRect.Width();
                float scaleFactorWidth = actualImageWidth / displayedImageWidth;

                // Get the scale factor between the actual Bitmap dimensions and the
                // displayed dimensions for height.
                float actualImageHeight = mBitmap.Height;
                float displayedImageHeight = displayedImageRect.Height();
                float scaleFactorHeight = actualImageHeight / displayedImageHeight;

                // Get crop window position relative to the displayed image.
                float cropWindowX = EdgeManager.Left.Coordinate - displayedImageRect.Left;
                float cropWindowY = EdgeManager.Top.Coordinate - displayedImageRect.Top;
                float cropWindowWidth = Edge.GetWidth();
                float cropWindowHeight = Edge.GetHeight();

                // Scale the crop window position to the actual size of the Bitmap.
                float actualCropX = cropWindowX * scaleFactorWidth;
                float actualCropY = cropWindowY * scaleFactorHeight;
                float actualCropWidth = cropWindowWidth * scaleFactorWidth;
                float actualCropHeight = cropWindowHeight * scaleFactorHeight;

                // Crop the subset from the original Bitmap.
                Bitmap croppedBitmap = Bitmap.CreateBitmap(mBitmap,
                    (int)actualCropX,
                    (int)actualCropY,
                    (int)actualCropWidth,
                    (int)actualCropHeight);

                return croppedBitmap;
            }
        }

        /// <summary>
        /// Gets the cropped image based on the current crop selection.
        /// </summary>
        /// <returns>Return a new Circular Bitmap representing the cropped image</returns>
        public Bitmap GetCroppedCircleImage()
        {
            Bitmap bitmap = GetCroppedImage();
            Bitmap output = Bitmap.CreateBitmap(bitmap.Width,
                bitmap.Height, Bitmap.Config.Argb8888);
            var canvas = new Canvas(output);
            //TODO: FIX THIS
            //int color = 0xff424242;
            var paint = new Paint();
            var rect = new Rect(0, 0, bitmap.Width, bitmap.Height);

            paint.AntiAlias = true;
            canvas.DrawARGB(0, 0, 0, 0);
            //TODO: FIX THIS
            //paint.Color = color;
            canvas.DrawCircle(bitmap.Width / 2, bitmap.Height / 2,
                bitmap.Width / 2, paint);

            //canvas.DrawRect(0, 0, bitmap.Width, bitmap.Height, paint);

            paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcIn));
            canvas.DrawBitmap(bitmap, rect, rect, paint);
            //Bitmap _bmp = Bitmap.createScaledBitmap(output, 60, 60, false);
            //return _bmp;
            return output;
        }

        /// <summary>
        /// Gets the crop window's position relative to the source Bitmap (not the image
        /// displayed in the CropImageView).
        /// </summary>
        /// <returns>Return a RectF instance containing cropped area boundaries of the source Bitmap</returns>
        public RectF GetActualCropRect()
        {
            using (Rect displayedImageRect = ImageViewUtil.GetBitmapRectCenterInside(mBitmap, mImageView))
            {
                // Get the scale factor between the actual Bitmap dimensions and the
                // displayed dimensions for width.
                float actualImageWidth = mBitmap.Width;
                float displayedImageWidth = displayedImageRect.Width();
                float scaleFactorWidth = actualImageWidth / displayedImageWidth;

                // Get the scale factor between the actual Bitmap dimensions and the
                // displayed dimensions for height.
                float actualImageHeight = mBitmap.Height;
                float displayedImageHeight = displayedImageRect.Height();
                float scaleFactorHeight = actualImageHeight / displayedImageHeight;

                // Get crop window position relative to the displayed image.
                float displayedCropLeft = EdgeManager.Left.Coordinate - displayedImageRect.Left;
                float displayedCropTop = EdgeManager.Top.Coordinate - displayedImageRect.Top;
                float displayedCropWidth = Edge.GetWidth();
                float displayedCropHeight = Edge.GetHeight();

                // Scale the crop window position to the actual size of the Bitmap.
                float actualCropLeft = displayedCropLeft * scaleFactorWidth;
                float actualCropTop = displayedCropTop * scaleFactorHeight;
                float actualCropRight = actualCropLeft + displayedCropWidth * scaleFactorWidth;
                float actualCropBottom = actualCropTop + displayedCropHeight * scaleFactorHeight;

                // Correct for floating point errors. Crop rect boundaries should not
                // exceed the source Bitmap bounds.
                actualCropLeft = Math.Max(0f, actualCropLeft);
                actualCropTop = Math.Max(0f, actualCropTop);
                actualCropRight = Math.Min(mBitmap.Width, actualCropRight);
                actualCropBottom = Math.Min(mBitmap.Height, actualCropBottom);

                var actualCropRect = new RectF(actualCropLeft,
                    actualCropTop,
                    actualCropRight,
                    actualCropBottom);

                return actualCropRect;
            }
        }

        /// <summary>
        /// Sets whether the aspect ratio is fixed or not; true fixes the aspect ratio, while
        /// false allows it to be changed.
        /// </summary>
        /// <param name="fixAspectRatio">Bool that signals whether the aspect ratio should be maintained.</param>
        public void SetFixedAspectRatio(bool fixAspectRatio)
        {
            mCropOverlayView.SetFixedAspectRatio(fixAspectRatio);
        }

        /////**
        //// * Sets the guidelines for the CropOverlayView to be either on, off, or to show when
        //// * resizing the application.
        //// * 
        //// * @param guidelines Integer that signals whether the guidelines should be on, off, or
        //// *            only showing when resizing.
        //// */
        /// <summary>
        /// Sets the guidelines for the CropOverlayView to be either on, off, or to show when
        /// resizing the application.
        /// </summary>
        /// <param name="guidelines">Integer that signals whether the guidelines should be on, off, or only showing when resizing.</param>
        public void SetGuidelines(CropGuidelines guidelines)
        {
            mCropOverlayView.SetGuidelines(guidelines);
        }

        /// <summary>
        /// Sets the both the X and Y values of the aspectRatio.
        /// </summary>
        /// <param name="aspectRatioX">That specifies the new X value of the aspect ratio</param>
        /// <param name="aspectRatioY">That specifies the new Y value of the aspect ratio</param>
        public void SetAspectRatio(int aspectRatioX, int aspectRatioY)
        {
            mAspectRatioX = aspectRatioX;
            mCropOverlayView.SetAspectRatioX(mAspectRatioX);

            mAspectRatioY = aspectRatioY;
            mCropOverlayView.SetAspectRatioY(mAspectRatioY);
        }

        /// <summary>
        /// Rotates image by the specified number of degrees clockwise. Cycles from 0 to 360 degrees.
        /// </summary>
        /// <param name="degrees">Integer specifying the number of degrees to rotate.</param>
        public void RotateImage(int degrees)
        {
            var matrix = new Matrix();
            matrix.PostRotate(degrees);
            mBitmap = Bitmap.CreateBitmap(mBitmap, 0, 0, mBitmap.Width, mBitmap.Height, matrix, true);
            SetImageBitmap(mBitmap);

            mDegreesRotated += degrees;
            mDegreesRotated %= 360;
        }

        private void Init(Context context)
        {
            //LayoutInflater inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            LayoutInflater inflater = LayoutInflater.From(context);
            View v = inflater.Inflate(Resource.Layout.crop_image_view, this, true);
            if (v != null)
            {
                mImageView = v.FindViewById<ImageView>(Resource.Id.ImageView_image);
                if (mImageView != null)
                {
                    SetImageResource(mImageResource);
                }
                mCropOverlayView = v.FindViewById<CropOverlayView>(Resource.Id.CropOverlayView);
                if (mCropOverlayView != null)
                {
                    mCropOverlayView.SetInitialAttributeValues(mGuidelines, mShapeSelector, mFixAspectRatio, mAspectRatioX, mAspectRatioY);
                }
            }
        }

        /// <summary>
        /// Determines the specs for the onMeasure function. Calculates the width or height depending on the mode.
        /// </summary>
        /// <param name="measureSpecMode">The mode of the measured width or height.</param>
        /// <param name="measureSpecSize">The size of the measured width or height.</param>
        /// <param name="desiredSize">The desired size of the measured width or height.</param>
        /// <returns>The  size of the width or height.</returns>
        private static int GetOnMeasureSpec(int measureSpecMode, int measureSpecSize, int desiredSize)
        {
            // Measure Width
            int spec;
            if (measureSpecMode == (int)MeasureSpecMode.Exactly)
            {
                // Must be this size
                spec = measureSpecSize;
            }
            else if (measureSpecMode == (int)MeasureSpecMode.AtMost)
            {
                // Can't be bigger than...; match_parent value
                spec = Math.Min(desiredSize, measureSpecSize);
            }
            else
            {
                // Be whatever you want; wrap_content
                spec = desiredSize;
            }

            return spec;
        }
    }
}
