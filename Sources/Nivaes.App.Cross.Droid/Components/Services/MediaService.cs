using Android.Content;
using Android.Content.PM;
using Nivaes.App.Droid;

namespace Nivaes.App.Cross.Droid
{
    public class MediaService
        : IMediaService
    {
        private readonly Context mContext;

        public MediaService()
        {
            mContext = Android.App.Application.Context;
            IsCameraAvailable = mContext.PackageManager.HasSystemFeature(PackageManager.FeatureCamera)
                            | mContext.PackageManager.HasSystemFeature(PackageManager.FeatureCameraFront);
        }

        ValueTask IMediaService.Initialize() => new ValueTask();

        private int mRequestId;
        private TaskCompletionSource<string> mCompletionSource;

        public bool IsCameraAvailable { get; }

        public bool IsTakePhotoSupported => true; //await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();

        public bool IsPickPhotoSupported => true;
        #region TakePhoto
        async ValueTask<string> IMediaService.TakePhoto(StoreField storeField, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return string.Empty;

            if (!IsCameraAvailable)
                return string.Empty;

            var takePath = await TakePhoto(token).ConfigureAwait(false);

            if (string.IsNullOrEmpty(takePath))
                return string.Empty;

            using var fileDraf = FullDraftDirectory(storeField);
            fileDraf.DeleteOnExit();
            fileDraf.Mkdirs();

            using (var filePhoto = new Java.IO.File(fileDraf.Path, System.IO.Path.GetFileName(takePath)))
            {
                System.IO.File.Move(takePath, filePhoto.Path);
                return filePhoto.Path;
            }
        }

        private Task<string> TakePhoto(CancellationToken token)
        {
            int idRequest = mRequestId = ++mRequestId % int.MaxValue;

            var ntcs = new TaskCompletionSource<string>(idRequest);
            if (Interlocked.CompareExchange(ref mCompletionSource, ntcs, null) != null)
                return new ValueTask<string>().AsTask();

            Intent takePictureIntent = new Intent(mContext, typeof(MediaTakeActivity));
            takePictureIntent.PutExtra(MediaTakeActivity.ExtraId, idRequest);
            takePictureIntent.SetFlags(ActivityFlags.NewTask);
            Application.Context.StartActivity(takePictureIntent);

            void handler(object _, MediaTakeEventArgs e)
            {
                var tcs = Interlocked.Exchange(ref mCompletionSource, null);

                MediaTakeActivity.MediaTaked -= handler;

                if (e.RequestId != idRequest)
                    return;

                if (e.IsCanceled)
                    tcs.SetResult(null);
                else if (e.Error != null)
                    tcs.SetException(e.Error);
                else
                {
                    tcs.SetResult(e.ImagePath);
                }
            }

            token.Register(() =>
            {
                var tcs = Interlocked.Exchange(ref mCompletionSource, null);

                MediaTakeActivity.MediaTaked -= handler;

                tcs.SetResult(null);
            });

            MediaTakeActivity.MediaTaked += handler;

            return ntcs.Task;
        }
        #endregion

        #region PickImage
        async ValueTask<string> IMediaService.PickImage(StoreField storeField, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return string.Empty;

            if (!IsPickPhotoSupported)
                return string.Empty;

            var pickPath = await PickImage(token).ConfigureAwait(false);

            if (string.IsNullOrEmpty(pickPath))
                return string.Empty;

            using var fileDraf = FullDraftDirectory(storeField);
            fileDraf.DeleteOnExit();
            fileDraf.Mkdirs();

            using (var filePhoto = new Java.IO.File(fileDraf.Path, System.IO.Path.GetFileName(pickPath)))
            {
                System.IO.File.Move(pickPath, filePhoto.Path);
                return filePhoto.Path;
            }
        }

        private Task<string> PickImage(CancellationToken token)
        {
            int idRequest = mRequestId = ++mRequestId % int.MaxValue;

            var ntcs = new TaskCompletionSource<string>(idRequest);
            if (Interlocked.CompareExchange(ref mCompletionSource, ntcs, null) != null)
                return new ValueTask<string>().AsTask();

            Intent takePictureIntent = new Intent(mContext, typeof(MediaPickerActivity));
            takePictureIntent.PutExtra(MediaPickerActivity.ExtraId, idRequest);
            takePictureIntent.SetFlags(ActivityFlags.NewTask);
            Application.Context.StartActivity(takePictureIntent);

            void handler(object _, MediaPickedEventArgs e)
            {
                var tcs = Interlocked.Exchange(ref mCompletionSource, null);

                MediaPickerActivity.MediaPickerd -= handler;

                if (e.RequestId != idRequest)
                    return;

                if (e.IsCanceled)
                    tcs.SetResult(null);
                else if (e.Error != null)
                    tcs.SetException(e.Error);
                else
                {
                    tcs.SetResult(e.ImagesUri.Single());
                }
            }

            token.Register(() =>
            {
                var tcs = Interlocked.Exchange(ref mCompletionSource, null);

                MediaPickerActivity.MediaPickerd -= handler;

                tcs.SetResult(null);
            });

            MediaPickerActivity.MediaPickerd += handler;

            return ntcs.Task;
        }
        #endregion

        #region PickImages
        ValueTask<IEnumerable<string>> IMediaService.PickImages(StoreField storeField, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return new ValueTask<IEnumerable<string>>();

            //FileOpenPicker openPicker = new FileOpenPicker
            //{
            //    ViewMode = PickerViewMode.List,
            //    SuggestedStartLocation = PickerLocationId.PicturesLibrary,
            //};

            //openPicker.FileTypeFilter.AddRange(SupportedImageFileTypes);

            //var storageFiles = await openPicker.PickMultipleFilesAsync();

            //if (storageFiles != null)
            //{
            //    var files = new List<string>();
            //    var directory = FullDirectory(container, id, field);
            //    var folderStore = await ApplicationData.Current.LocalFolder.CreateFolderAsync(directory, CreationCollisionOption.OpenIfExists);

            //    foreach (var storageFile in storageFiles)
            //    {
            //        var file = await folderStore.CreateFileAsync(Guid.NewGuid().ToString() + storageFile.FileType, CreationCollisionOption.ReplaceExisting);
            //        await storageFile.CopyAndReplaceAsync(file);
            //        files.Add(file.Path);
            //    }

            //    return files;
            //}

            //return new string[] { };

            return new ValueTask<IEnumerable<string>>();
        }
        #endregion

        async ValueTask<string> IMediaService.ConsolideImage(StoreField storeField)
        {
            using Java.IO.File drafFileDirectory = FullDraftDirectory(storeField), fileDirectory = FullDirectory(storeField);

            if (Directory.Exists(fileDirectory.Path))
                Directory.Delete(fileDirectory.Path, true);

            var storageFiles = await drafFileDirectory.ListFilesAsync().ConfigureAwait(false);
            var storageFile = storageFiles.FirstOrDefault();

            if (storageFile != null)
            {
                fileDirectory.Mkdirs();

                var file = System.IO.Path.Combine(fileDirectory.Path, storageFile.Name);
                System.IO.File.Move(storageFile.Path, file);

                drafFileDirectory.DeleteOnExit();
                return file;
            }

            return string.Empty;
        }

        ValueTask IMediaService.UndoImage(StoreField storeField)
        {
            using var drafFileDirectory = FullDraftDirectory(storeField);

            if (Directory.Exists(drafFileDirectory.Path))
                Directory.Delete(drafFileDirectory.Path, true);

            return new ValueTask();
        }

        async ValueTask<string> IMediaService.GetImage(StoreField storeField)
        {
            using var fileDirectory = FullDirectory(storeField);

            var files = await fileDirectory.ListFilesAsync().ConfigureAwait(false);

            return files?.FirstOrDefault()?.Path;
        }

        ValueTask<string> IMediaService.GetImagePath(StoreField storeField, string file)
        {
            using var directory = FullDirectory(storeField);

            var filePath = System.IO.Path.Combine(directory.Path, file);

            return new ValueTask<string>(filePath);
        }

        ValueTask<Stream> IMediaService.ImageStreamForWrite(StoreField storeField, string file)
        {
            using var directory = FullDirectory(storeField);

            directory.Mkdirs();
            using var formatedFile = new Java.IO.File(directory, file);

            var fileStream = new FileStream(formatedFile.Path, FileMode.OpenOrCreate, FileAccess.Write);

            return new ValueTask<Stream>(fileStream);
        }

        async ValueTask<IEnumerable<string>> IMediaService.GetImages(StoreField storeField)
        {
            using var fileDirectory = FullDirectory(storeField);

            var files = await fileDirectory.ListFilesAsync().ConfigureAwait(false);
            return files?.Select(f => f.Path) ?? Array.Empty<string>();
        }

        async ValueTask IMediaService.DeleteImage(StoreField storeField, string file)
        {
            using var fileDirectory = FullDirectory(storeField);

            foreach (var fileObject in await fileDirectory.ListFilesAsync().ConfigureAwait(false))
            {
                if (fileObject.Name == file)
                {
                    fileObject.Delete();
                }
            }
        }

        ValueTask IMediaService.DeleteImages(StoreField storeField)
        {
            using var directory = FullDirectory(storeField);

            if (Directory.Exists(directory.Path))
                Directory.Delete(directory.Path, true);

            return new ValueTask();
        }

        private Java.IO.File FullDirectory(StoreField storeField)
        {
            var directory = System.IO.Path.Combine("MediaStore", storeField.Container, storeField.Id.ToString(), storeField.Field);
            return new Java.IO.File(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), directory);
        }

        private Java.IO.File FullDraftDirectory(StoreField storeField)
        {
            var draftDirectory = System.IO.Path.Combine("MediaDraft", storeField.Container, storeField.Id.ToString(), storeField.Field);
            return new Java.IO.File(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), draftDirectory);
        }
    }
}

