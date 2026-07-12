using System.Globalization;
using Android.Content;
using Android.Content.PM;
using Android.Database;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Uri = Android.Net.Uri;

namespace Nivaes.App.Cross.Droid
{
    //ToDo: Mover a libreria
    //[Activity(Name = "com.nivaes.MediaPickerActivity"
    //    , Label = "@string/application_name"
    //    , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    [Register("com.nivaes.MediaPickerActivity")]
    internal class MediaPickerActivity
        : BaseMediaActivity
    {
        internal const string ExtraId = "nivaes.app.MediaPicker.id";
        internal const string MultiSelect = "nivaes.app.MediaPicker.MultiSelect";
        private int mRequestCodeId;
        private bool mMultiSelect;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var bundle = (savedInstanceState ?? base.Intent.Extras);
            mRequestCodeId = bundle.GetInt(ExtraId, 0);
            mMultiSelect = bundle.GetBoolean(MultiSelect, false);

            PickerPhoto();
        }

        private void PickerPhoto()
        {
            using (var pickIntent = new Intent(Intent.ActionGetContent))
            {
                try
                {
                    pickIntent.SetType("image/*");

                    pickIntent.AddFlags(ActivityFlags.GrantReadUriPermission);
                    pickIntent.AddFlags(ActivityFlags.GrantWriteUriPermission);

                    if (mMultiSelect)
                        pickIntent.PutExtra(Intent.ExtraAllowMultiple, true);

                    //base.StartActivityForResult(Intent.CreateChooser(pickIntent, MediaSelectorLocalizationString.PickerPhotoChooser), mRequestCodeId);
                }
                catch (Exception ex)
                {
                    OnMediaPicked(new MediaPickedEventArgs(mRequestCodeId, ex));
                    //base.Finish();
                }
            }
        }

        //protected override async void OnActivityResult(int requestCode, Result resultCode, Intent data)
        //{
        //    base.OnActivityResult(requestCode, resultCode, data);

        //    if (resultCode == Result.Canceled)
        //    {
        //        base.Finish();
        //        OnMediaPicked(new MediaPickedEventArgs(requestCode, isCanceled: true));
        //    }
        //    else
        //    {
        //        await LoadImages(requestCode, data).ConfigureAwait(false);
        //    }
        //}

        private async Task LoadImages(int requestCode, Intent data)
        {
            //if (mMultiSelect)
            //{
            //    base.Finish();
            //}

            if (data?.ClipData != null)
            {
                var files = new List<string>();

                var clipData = data.ClipData;
                for (var i = 0; i < clipData.ItemCount; i++)
                {
                    var item = clipData.GetItemAt(i);

                    string path = await GetRealPathFromUri(item.Uri).ConfigureAwait(false);

                    files.Add(path);
                }

                if (mMultiSelect)
                {
                    await LoadImages(requestCode, files).ConfigureAwait(false);
                }

                return;
            }
            else if (data?.Data != null)
            {
                string path = await GetRealPathFromUri(data.Data).ConfigureAwait(false);
                await LoadImages(requestCode, new[] { path }).ConfigureAwait(false);

                return;
            }

            OnMediaPicked(new MediaPickedEventArgs(requestCode, isCanceled: true));
        }

        private async Task LoadImages(int requestCode, IEnumerable<string> files)
        {
            if (mMultiSelect)
            {
                OnMediaPicked(new MediaPickedEventArgs(requestCode, files));
            }
            else
            {
                BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = false };
                var imageBitmap = await BitmapFactory.DecodeFileAsync(files.Single(), options).ConfigureAwait(false);

                base.SetImageBitmap(imageBitmap);
            }
        }

        public async Task<string> GetRealPathFromUri(Uri contentUri)
        {
            string[] proj = null;
            if ((int)Build.VERSION.SdkInt >= 22)
                proj = new[] { MediaStore.MediaColumns.Data };

            ICursor cursor = base.ContentResolver.Query(contentUri, proj, null, null, null);
            if (cursor == null || !cursor.MoveToNext())
            {
                return string.Empty;
            }
            else
            {
                int columnIndex = cursor.GetColumnIndex(MediaStore.MediaColumns.Data);

                string contentPath = null;

                if (columnIndex != -1)
                    contentPath = cursor.GetString(columnIndex);

                if (contentPath == null || !contentPath.StartsWith("file", StringComparison.InvariantCultureIgnoreCase))
                {
                    string fileName = null;
                    try
                    {
                        fileName = System.IO.Path.GetFileName(contentPath);
                    }
                    catch { }

                    var outputPath = GetOutputMediaFile("temp", fileName, false);

                    try
                    {
                        using (var input = base.ContentResolver.OpenInputStream(contentUri))
                        using (var output = System.IO.File.Create(outputPath.Path))
                            await input.CopyToAsync(output).ConfigureAwait(false);

                        contentPath = outputPath.Path;
                    }
                    catch (Java.IO.FileNotFoundException) { }
                }

                return contentPath;
            }
        }

        public Uri GetOutputMediaFile(string subdir, string name, bool saveToAlbum)
        {
            subdir = subdir ?? string.Empty;

            if (string.IsNullOrWhiteSpace(name))
            {
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);

                name = "IMG_" + timestamp + ".jpg";
            }

            var mediaType = Android.OS.Environment.DirectoryPictures;
            var directory = saveToAlbum ? Android.OS.Environment.GetExternalStoragePublicDirectory(mediaType) : base.GetExternalFilesDir(mediaType);
            using (var mediaStorageDir = new Java.IO.File(directory, subdir))
            {
                if (!mediaStorageDir.Exists())
                {
                    if (!mediaStorageDir.Mkdirs())
                        throw new Exception("Couldn't create directory, have you added the WRITE_EXTERNAL_STORAGE permission?");

                    if (!saveToAlbum)
                    {
                        using (var nomedia = new Java.IO.File(mediaStorageDir, ".nomedia"))
                            nomedia.CreateNewFile();
                    }
                }

                return Uri.FromFile(new Java.IO.File(GetUniquePath(mediaStorageDir.Path, name)));
            }
        }

        private string GetUniquePath(string folder, string name)
        {
            var ext = System.IO.Path.GetExtension(name);
            if (ext == string.Empty)
                ext = ".jpg";

            name = System.IO.Path.GetFileNameWithoutExtension(name);

            var nname = name + ext;
            var i = 1;
            while (System.IO.File.Exists(System.IO.Path.Combine(folder, nname)))
                nname = name + "_" + (i++) + ext;

            return System.IO.Path.Combine(folder, nname);
        }

        #region Events
        internal static event EventHandler<MediaPickedEventArgs> MediaPickerd;

        private static void OnMediaPicked(MediaPickedEventArgs e) => MediaPickerd?.Invoke(null, e);
        #endregion

        protected override void ButtonCancel(object sender, EventArgs e)
        {
            PickerPhoto();
        }

        protected override async void ButtonDone(object sender, EventArgs e)
        {
            base.Finish();

            var cropImage = base.GetCroppedImage();

            try
            {
                using (var filePath = new Java.IO.File(GetExternalFilesDir(Android.OS.Environment.DirectoryPictures), $"{Guid.NewGuid()}.jpg"))
                {
                    using (var fileStream = System.IO.File.OpenWrite(filePath.Path))
                    {
                        using (Bitmap resizedBitmap = Bitmap.CreateScaledBitmap(cropImage, cropImage.Width, cropImage.Height, false))
                        {
                            await resizedBitmap.CompressAsync(Bitmap.CompressFormat.Jpeg, 90, fileStream).ConfigureAwait(false);

                            resizedBitmap?.Recycle();
                        }
                    }

                    OnMediaPicked(new MediaPickedEventArgs(mRequestCodeId, new[] { filePath.Path }));
                }
            }
            finally
            {
                cropImage?.Recycle();
            }
        }
    }
}
