namespace Nivaes.App.Cross.Droid.GropImage
{
    using Android.Graphics;

    internal class Handle
    {
        private readonly HandleHelper mHelper;
        public HandleType HandleType { get; private set; }

        public Handle(HandleHelper helper, HandleType handleType)
        {
            mHelper = helper;
            HandleType = handleType;
        }

        public void UpdateCropWindow(float x,
            float y,
            Rect imageRect,
            float snapRadius)
        {
            mHelper.UpdateCropWindow(x, y, imageRect, snapRadius);
        }

        public void UpdateCropWindow(float x,
            float y,
            float targetAspectRatio,
            Rect imageRect,
            float snapRadius)
        {
            mHelper.UpdateCropWindow(x, y, targetAspectRatio, imageRect, snapRadius);
        }
    }
}
