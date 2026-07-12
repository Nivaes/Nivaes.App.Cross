using Android.Graphics;
using Android.Media;
using Android.Runtime;
using global::Android.Content.PM;
using Nivaes.App.Droid;
using File = Java.IO.File;
using Orientation = Android.Media.Orientation;

namespace Nivaes.App.Cross.Droid
{
    [Activity(Name = "com.nivaes.MediaTakeActivity"
        , Label = "@string/application_name"
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize
        )]
    [Register("com.nivaes.MediaTakeActivity")]
    internal class MediaTakeActivity
        : BaseMediaActivity
    {
        private File mFile;
        private int mRequestCodeId;

        internal const string ExtraId = "nivaes.app.MediaTake.id";

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //var bundle = (savedInstanceState ?? base.Intent.Extras);
            //mRequestCodeId = bundle.GetInt(ExtraId, 0);

            TakePhoto();
        }

        private void TakePhoto()
        {
            //using (Intent takeIntent = new Intent(MediaStore.ActionImageCapture))
            //{
            //    try
            //    {
            //        mFile = new File(base.GetExternalFilesDir(Android.OS.Environment.DirectoryPictures), $"{Guid.NewGuid()}.jpg");

            //        takeIntent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(mFile));
            //        takeIntent.PutExtra("android.intent.extra.quickCapture", true);

            //        base.StartActivityForResult(Intent.CreateChooser(takeIntent, MediaSelectorLocalizationString.TakePhotoChooser), mRequestCodeId);
            //        //base.StartActivityForResult(takeIntent, mRequestCodeId);
            //    }
            //    catch (Exception ex)
            //    {
            //        OnMediaTaked(new MediaTakeEventArgs(mRequestCodeId, ex));
            //        base.Finish();
            //    }
            //}
        }

        //protected override async void OnActivityResult(int requestCode, Result resultCode, Intent data)
        //{
        //    base.OnActivityResult(requestCode, resultCode, data);

        //    if (resultCode == Result.Canceled)
        //    {
        //        mFile?.Delete();

        //        base.Finish();
        //        OnMediaTaked(new MediaTakeEventArgs(requestCode, isCanceled: true));
        //    }
        //    else if (resultCode == Result.Ok)
        //    {
        //        var filePath = await AdjustTaskPhoto().ConfigureAwait(false);
        //        await LoadImage(filePath).ConfigureAwait(false);
        //    }
        //}

        private async Task LoadImage(string path)
        {
            BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = false };
            var imageBitmap = await BitmapFactory.DecodeFileAsync(path, options).ConfigureAwait(false);

            base.SetImageBitmap(imageBitmap);
        }

        #region Events
        internal static event EventHandler<MediaTakeEventArgs> MediaTaked;

        private static void OnMediaTaked(MediaTakeEventArgs e) => MediaTaked?.Invoke(null, e);
        #endregion

        private async Task<string> AdjustTaskPhoto()
        {
            BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = false };

            var adjustFile = new Java.IO.File(mFile.Parent, $"{Guid.NewGuid()}.{System.IO.Path.GetExtension(mFile.Name)}");

            try
            {
                using (Bitmap originalBitmap = await BitmapFactory.DecodeFileAsync(mFile.Path, options).ConfigureAwait(false))
                {
                    Bitmap resizedBitmap = null;
                    try
                    {
                        float rotate = CalculateRotate(mFile.Path);
                        if (rotate == 0)
                        {
                            resizedBitmap = Bitmap.CreateScaledBitmap(originalBitmap, options.OutWidth, options.OutHeight, false);
                        }
                        else
                        {
                            var matrix = new Matrix();
                            matrix.PostRotate(rotate);
                            resizedBitmap = Bitmap.CreateBitmap(originalBitmap, 0, 0, options.OutWidth, options.OutHeight, matrix, true);
                        }

                        using (var fileStream = System.IO.File.OpenWrite(adjustFile.Path))
                        {
                            await resizedBitmap.CompressAsync(Bitmap.CompressFormat.Jpeg, 90, fileStream).ConfigureAwait(false);
                        }
                    }
                    finally
                    {
                        originalBitmap?.Recycle();
                        resizedBitmap?.Recycle();
                        resizedBitmap?.Dispose();
                    }
                }

                return adjustFile.Path;
            }
            finally
            {
                mFile?.Delete();
                mFile?.Dispose();
                mFile = adjustFile;

                GC.Collect();
            }
        }

        private float CalculateRotate(string imagePath)
        {
            using (var originalMetadata = new ExifInterface(imagePath))
            {
                var orientation = (Orientation)originalMetadata.GetAttributeInt(ExifInterface.TagOrientation, (int)Orientation.Normal);

                switch (orientation)
                {
                    case Orientation.Rotate90:
                        return 90;
                    case Orientation.Rotate180:
                        return 180;
                    case Orientation.Rotate270:
                        return 270;
                    default:
                        return 0;
                }
            }
        }

        protected override void ButtonCancel(object sender, EventArgs e)
        {
            TakePhoto();
        }

        protected override async void ButtonDone(object sender, EventArgs e)
        {
            //base.Finish();

            using (var cropImage = base.GetCroppedImage())
            {
                try
                {
                    //using (var filePath = new Java.IO.File(GetExternalFilesDir(Android.OS.Environment.DirectoryPictures), $"{Guid.NewGuid()}.jpg"))
                    //{
                    //    using (var fileStream = System.IO.File.OpenWrite(filePath.Path))
                    //    {
                    //        using (Bitmap resizedBitmap = Bitmap.CreateScaledBitmap(cropImage, cropImage.Width, cropImage.Height, false))
                    //        {
                    //            await resizedBitmap.CompressAsync(Bitmap.CompressFormat.Jpeg, 90, fileStream).ConfigureAwait(false);

                    //            resizedBitmap?.Recycle();
                    //        }
                    //    }

                    //    OnMediaTaked(new MediaTakeEventArgs(mRequestCodeId, filePath.Path));
                    //}
                }
                finally
                {
                    mFile?.Delete();
                    mFile?.Dispose();
                }
            }
        }
    }
}
