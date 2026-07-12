using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using System.Collections.Generic;

namespace Nivaes.App.Cross.WinUI
{
    public class MediaService
        : IMediaService
    {
        //private static readonly IEnumerable<string> SupportedVideoFileTypes = new List<string> { ".mp4", ".wmv", ".avi" };
        private static readonly IEnumerable<string> SupportedImageFileTypes = new List<string> { ".jpeg", ".jpg", ".png", ".gif", ".bmp" };
        private bool initialized = false;

        public MediaService()
        {
            //watcher = DeviceInformation.CreateWatcher(DeviceClass.VideoCapture);
            //watcher.Added += OnDeviceAdded;
            //watcher.Updated += OnDeviceUpdated;
            //watcher.Removed += OnDeviceRemoved;
            //watcher.Start();

            //var info = DeviceInformation.FindAllAsync(DeviceClass.VideoCapture).GetResults();
            //mIsCameraAvailable = info.Any(d => d.IsEnabled);
        }

        public async ValueTask Initialize()
        {
            //if (initialized)
            //    return true;

            //try
            //{
            //    var info = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture).AsTask().ConfigureAwait(false);
            //    lock (devices)
            //    {
            //        foreach (var device in info)
            //        {
            //            if (device.IsEnabled)
            //                devices.Add(device.Id);
            //        }

            //        isCameraAvailable = (devices.Count > 0);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine("Unable to detect cameras: " + ex);
            //}

            initialized = true;

            var info = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
            mIsCameraAvailable = info.Any(d => d.IsEnabled);
        }

        private bool mIsCameraAvailable;

        public bool IsCameraAvailable
        {
            get
            {
                if (!initialized)
                    throw new InvalidOperationException("You must call Initialize() before calling any properties.");

                return mIsCameraAvailable;
            }
        }

        public bool IsTakePhotoSupported => true;

        public bool IsPickPhotoSupported => true;

        async ValueTask<string> IMediaService.TakePhoto(StoreField storeField, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return string.Empty;

            var capture = new CameraCaptureUI();
            capture.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            capture.PhotoSettings.AllowCropping = true;

            StorageFile fileResult = await capture.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (fileResult != null)
            {
                var drafDirectory = FullDraftDirectory(storeField);

                await DeleteDrafFiles(drafDirectory);

                var folderStore = await ApplicationData.Current.LocalCacheFolder.CreateFolderAsync(drafDirectory, CreationCollisionOption.OpenIfExists);

                var file = await folderStore.CreateFileAsync(Guid.NewGuid().ToString() + fileResult.FileType, CreationCollisionOption.ReplaceExisting);

                await fileResult.MoveAndReplaceAsync(file);

                return file.Path;
            }

            return string.Empty;
        }

        async ValueTask<string> IMediaService.PickImage(StoreField storeField, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return string.Empty;

            FileOpenPicker openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
            };

            openPicker.FileTypeFilter.AddRange(SupportedImageFileTypes);

            var storageFile = await openPicker.PickSingleFileAsync();

            if (storageFile != null)
            {
                var draftDirectory = FullDraftDirectory(storeField);

                await DeleteDrafFiles(draftDirectory);

                var folderStore = await ApplicationData.Current.LocalCacheFolder.CreateFolderAsync(draftDirectory, CreationCollisionOption.OpenIfExists);
                var file = await folderStore.CreateFileAsync(Guid.NewGuid().ToString() + storageFile.FileType, CreationCollisionOption.ReplaceExisting);
                await storageFile.CopyAndReplaceAsync(file);

                return file.Path;
            }

            return string.Empty;
        }

        async ValueTask<IEnumerable<string>> IMediaService.PickImages(StoreField storeField, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return null;

            FileOpenPicker openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
            };

            openPicker.FileTypeFilter.AddRange(SupportedImageFileTypes);

            var storageFiles = await openPicker.PickMultipleFilesAsync();

            if (storageFiles != null)
            {
                var files = new List<string>();
                var directory = FullDirectory(storeField);
                var folderStore = await ApplicationData.Current.LocalFolder.CreateFolderAsync(directory, CreationCollisionOption.OpenIfExists);

                foreach (var storageFile in storageFiles)
                {
                    var file = await folderStore.CreateFileAsync(Guid.NewGuid().ToString() + storageFile.FileType, CreationCollisionOption.ReplaceExisting);
                    await storageFile.CopyAndReplaceAsync(file);
                    files.Add(file.Path);
                }

                return files;
            }

            return new string[] { };
        }

        async ValueTask<string> IMediaService.ConsolideImage(StoreField storeField)
        {
            var draftDirectory = FullDraftDirectory(storeField);
            var directory = FullDirectory(storeField);

            await DeleteFiles(directory);

            var storeFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(directory, CreationCollisionOption.OpenIfExists);
            var draftStorageFolder = await ApplicationData.Current.LocalCacheFolder.GetFolderAsync(draftDirectory);

            var storageFiles = await draftStorageFolder.GetFilesAsync();
            var storageFile = storageFiles.FirstOrDefault();

            if (storageFile != null)
            {
                var file = await storeFolder.CreateFileAsync(storageFile.Name, CreationCollisionOption.ReplaceExisting);
                await storageFile.CopyAndReplaceAsync(file);

                await draftStorageFolder.DeleteAsync();

                return file?.Path ?? string.Empty;
            }

            return string.Empty;
        }

        ValueTask IMediaService.UndoImage(StoreField storeField)
        {
            var draftDirectory = FullDraftDirectory(storeField);
            return DeleteDrafFiles(draftDirectory);
        }

        async ValueTask<string> IMediaService.GetImage(StoreField storeField)
        {
            var directory = FullDirectory(storeField);
            var folderStore = await ApplicationData.Current.LocalFolder.CreateFolderAsync(directory, CreationCollisionOption.OpenIfExists);

            var files = await folderStore.GetFilesAsync();
            var file = files.FirstOrDefault();

            return file?.Path ?? string.Empty;
        }

        ValueTask<string> IMediaService.GetImagePath(StoreField storeField, string file)
        {
            var directory = FullDirectory(storeField);
            var filePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, directory, file);

            return new ValueTask<string>(filePath);
        }

        async ValueTask<Stream> IMediaService.ImageStreamForWrite(StoreField storeField, string file)
        {
            var directory = FullDirectory(storeField);
            var folderStore = await ApplicationData.Current.LocalFolder.CreateFolderAsync(directory, CreationCollisionOption.OpenIfExists);
            var fileStore = await folderStore.CreateFileAsync(file, CreationCollisionOption.ReplaceExisting);

            return await fileStore.OpenStreamForWriteAsync();
        }

        async ValueTask<IEnumerable<string>> IMediaService.GetImages(StoreField storeField)
        {
            var directory = FullDirectory(storeField);

            var folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(directory, CreationCollisionOption.OpenIfExists);

            var files = await folder.GetFilesAsync();
            return files.Select(f => f.Path);
        }

        ValueTask IMediaService.DeleteImage(StoreField storeField, string file)
        {
            var directory = FullDirectory(storeField);
            return DeleteFile(directory, file);
        }

        ValueTask IMediaService.DeleteImages(StoreField storeField)
        {
            var directory = FullDirectory(storeField);
            return DeleteFiles(directory);
        }

        private async ValueTask DeleteFile(string directory, string file)
        {
            var folderStore = await ApplicationData.Current.LocalFolder.CreateFolderAsync(directory, CreationCollisionOption.OpenIfExists);
            var fileStore = await folderStore.GetFileAsync(file);
            await fileStore.DeleteAsync();
        }

        private async ValueTask DeleteFiles(string directory)
        {
            var folderStore = await ApplicationData.Current.LocalFolder.CreateFolderAsync(directory, CreationCollisionOption.OpenIfExists);
            var files = await folderStore.GetFilesAsync();

            foreach (var fileStore in files)
            {
                await fileStore.DeleteAsync();
            }
        }

        private async ValueTask DeleteDrafFiles(string directory)
        {
            var folderStore = await ApplicationData.Current.LocalCacheFolder.CreateFolderAsync(directory, CreationCollisionOption.OpenIfExists);
            var files = await folderStore.GetFilesAsync();

            foreach (var fileStore in files)
            {
                await fileStore.DeleteAsync();
            }
        }

        private string FullDirectory(StoreField storeField) => Path.Combine("MediaStore", storeField.Container, storeField.Id.ToString(), storeField.Field);

        private string FullDraftDirectory(StoreField storeField) => Path.Combine("MediaDraft", storeField.Container, storeField.Id.ToString(), storeField.Field);
    }
}
