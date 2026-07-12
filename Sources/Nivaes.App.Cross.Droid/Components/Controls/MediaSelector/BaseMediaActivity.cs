using Android.Graphics;
using Android.Views;
using AndroidX.AppCompat.App;
using Nivaes.App.Cross.Droid.GropImage;

namespace Nivaes.App.Cross.Droid
{
    internal abstract class BaseMediaActivity
        : AppCompatActivity
    {
        private CropImageView mCropImageView;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            base.SetContentView(Resource.Layout.crop_image_activity);

            mCropImageView = base.FindViewById<CropImageView>(Resource.Id.crop_image_view);

            var buttonCancel = base.FindViewById<Button>(Resource.Id.but_cancel);
            var buttonDone = base.FindViewById<Button>(Resource.Id.but_done);

            buttonCancel.Click += ButtonCancel;
            buttonDone.Click += ButtonDone;
        }

        protected void SetImageBitmap(Bitmap bitmap)
        {
            mCropImageView.SetImageBitmap(bitmap);
            mCropImageView.Visibility = ViewStates.Visible;
        }

        protected Bitmap GetCroppedImage()
        {
            return mCropImageView.GetCroppedImage();
        }

        protected abstract void ButtonCancel(object? sender, EventArgs e);

        protected abstract void ButtonDone(object? sender, EventArgs e);
    }
}
