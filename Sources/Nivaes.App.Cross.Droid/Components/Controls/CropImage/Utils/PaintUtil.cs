namespace Nivaes.App.Cross.Droid.GropImage
{
    using System;
    using Android.Content;
    using Android.Graphics;
    using Android.Util;

    public class PaintUtil
    {
        //private static readonly int DefaultCornerColor = Color.White;
        private static readonly string SemiTransparent = "#AAFFFFFF";
        private static readonly string DefaultBackgroundColorId = "#B0000000";
        private static readonly float DefaultLineThicknessDp = 2;
        private static readonly float DefaultCornerThicknessDp = 5;
        private static readonly float DefaultGuidelineThicknessPx = 1;

        /**
         * Creates the Paint object for drawing the crop window border.
         * 
         * @param context the Context
         * @return new Paint object
         */

        public static Paint NewBorderPaint(Context context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            // Set the line thickness for the crop window border.
            float lineThicknessPx = TypedValue.ApplyDimension(ComplexUnitType.Dip, DefaultLineThicknessDp,
                context.Resources.DisplayMetrics);

            var borderPaint = new Paint
            {
                Color = Color.ParseColor(SemiTransparent),
                StrokeWidth = lineThicknessPx
            };
            borderPaint.SetStyle(Paint.Style.Stroke);
            borderPaint.AntiAlias = true;

            return borderPaint;
        }

        /**
         * Creates the Paint object for drawing the crop window guidelines.
         * 
         * @return the new Paint object
         */

        public static Paint NewGuidelinePaint()
        {
            var paint = new Paint
            {
                Color = Color.ParseColor(SemiTransparent),
                StrokeWidth = DefaultGuidelineThicknessPx
            };

            return paint;
        }

        /**
         * Creates the Paint object for drawing the translucent overlay outside the
         * crop window.
         * 
         * @param context the Context
         * @return the new Paint object
         */

        public static Paint NewBackgroundPaint(Context context)
        {
            var paint = new Paint
            {
                Color = Color.ParseColor(DefaultBackgroundColorId)
            };

            return paint;
        }

        /**
         * Creates the Paint object for drawing the corners of the border
         * 
         * @param context the Context
         * @return the new Paint object
         */

        public static Paint NewCornerPaint(Context context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            // Set the line thickness for the crop window border.
            float lineThicknessPx = TypedValue.ApplyDimension(ComplexUnitType.Dip,
                DefaultCornerThicknessDp,
                context.Resources.DisplayMetrics);

            var cornerPaint = new Paint
            {
                //TODO: FIX
                //cornerPaint.Color = DEFAULT_CORNER_COLOR;
                StrokeWidth = lineThicknessPx
            };
            cornerPaint.SetStyle(Paint.Style.Stroke);

            return cornerPaint;
        }

        /**
         * Returns the value of the corner thickness
         * 
         * @return Float equivalent to the corner thickness
         */

        public static float GetCornerThickness()
        {
            return DefaultCornerThicknessDp;
        }

        /**
         * Returns the value of the line thickness of the border
         * 
         * @return Float equivalent to the line thickness
         */

        public static float GetLineThickness()
        {
            return DefaultLineThicknessDp;
        }
    }
}
