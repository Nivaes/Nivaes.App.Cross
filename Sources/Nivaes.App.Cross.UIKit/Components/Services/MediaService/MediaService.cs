using AVFoundation;
using Foundation;
using UIKit;

namespace Nivaes.App.Cross.UIKitLib
{
    public class MediaService
        : IMediaService
    {
#if IOS || MACCATALYST
        public static UIStatusBarStyle StatusBarStyle { get; set; }
#endif

        private AVAuthorizationStatus mVideoAutorization;

        public const string TypeImage = "public.image";

        public MediaService()
        {
#if IOS || MACCATALYST
            StatusBarStyle = UIApplication.SharedApplication.StatusBarStyle;
#endif
            mVideoAutorization = AVCaptureDevice.GetAuthorizationStatus(AVAuthorizationMediaType.Video);

#if IOS || MACCATALYST
            IsCameraAvailable = UIImagePickerController.IsCameraDeviceAvailable(UIKit.UIImagePickerControllerCameraDevice.Front)
                                       | UIImagePickerController.IsCameraDeviceAvailable(UIKit.UIImagePickerControllerCameraDevice.Rear);

            var availableCameraMedia = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.Camera) ?? Array.Empty<string>();

            IsTakePhotoSupported = availableCameraMedia.Contains(TypeImage);

            var avaialbleLibraryMedia = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary) ?? Array.Empty<string>();
            IsPickPhotoSupported = avaialbleLibraryMedia.Contains(TypeImage);
#endif
        }

        ValueTask IMediaService.Initialize() => new ValueTask();

        public bool IsCameraAvailable { get; }

        public bool IsTakePhotoSupported { get; private set; }

        public bool IsPickPhotoSupported { get; private set; }

        #region TakePhoto
        private UIPopoverController? popover = null;
#if IOS || MACCATALYST
        private UIImagePickerControllerDelegate? pickerDelegate;
#endif


        async ValueTask<string> IMediaService.TakePhoto(StoreField storeField, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return string.Empty;

            if (mVideoAutorization == AVAuthorizationStatus.NotDetermined)
            {
                if (await AVCaptureDevice.RequestAccessForMediaTypeAsync(AVAuthorizationMediaType.Video).ConfigureAwait(false))
                {
                    mVideoAutorization = AVAuthorizationStatus.Authorized;
                    IsTakePhotoSupported = true;
                }
                else
                {
                    mVideoAutorization = AVAuthorizationStatus.Denied;
                    IsTakePhotoSupported = false;
                }
            }

            if (IsTakePhotoSupported)
            {
                var dirDraft = FullDraftDirectory(storeField);
                if (Directory.Exists(dirDraft))
                    Directory.Delete(dirDraft, true);
                Directory.CreateDirectory(dirDraft);

                var pathDraft = Path.Combine(dirDraft, $"{Guid.NewGuid()}.jpg");

                var takePath = await TakePhoto(pathDraft, token).ConfigureAwait(false);

                if (string.IsNullOrEmpty(takePath))
                    return string.Empty;

                return takePath;
            }
            else
            {
                return string.Empty;
            }
        }

        private Task<string?> TakePhoto(string pathDraft, CancellationToken token)
        {
            var viewController = GetHostViewController();

#if IOS || MACCATALYST
            var ndelegate = new MediaPickerDelegate(viewController, pathDraft, token);
            var od = Interlocked.CompareExchange(ref pickerDelegate, ndelegate, null);
            if (od != null)
                throw new InvalidOperationException("Only one operation can be active at a time");

            var picker = new MediaPickerController(ndelegate)
            {
                MediaTypes = new[] { TypeImage },
                SourceType = UIImagePickerControllerSourceType.Camera,
                //AllowsEditing = true,
                CameraDevice = UIImagePickerControllerCameraDevice.Rear,
                ModalPresentationStyle = UIModalPresentationStyle.FullScreen,
                CameraCaptureMode = UIImagePickerControllerCameraCaptureMode.Photo,
            };

            viewController.PresentViewController(picker, true, null);

            token.Register(() =>
            {
                if (picker == null)
                    return;

                NSRunLoop.Main.BeginInvokeOnMainThread(() =>
                {
                    picker.DismissModalViewController(true);
                    ndelegate.CancelTask();
                });
            });

            return ndelegate.Task.ContinueWith(t =>
            {
                Dismiss(popover, picker);

                return t.Result?.FirstOrDefault();
            });
#else
            return Task.FromResult<string?>(null);
#endif
        }

        private static UIViewController GetHostViewController()
        {
            UIViewController? viewController = null;
            var window = UIApplication.SharedApplication.KeyWindow;
            if (window == null)
                throw new InvalidOperationException("There's no current active window");

            if (window.WindowLevel == UIWindowLevel.Normal)
                viewController = window.RootViewController;

            if (viewController == null)
            {
                window = UIApplication.SharedApplication.Windows.OrderByDescending(w => w.WindowLevel).FirstOrDefault(w => w.RootViewController != null && w.WindowLevel == UIWindowLevel.Normal);
                if (window == null)
                    throw new InvalidOperationException("Could not find current view controller");
                else
                    viewController = window.RootViewController;
            }

            while (viewController!.PresentedViewController != null)
                viewController = viewController.PresentedViewController;

            return viewController;
        }

        //private static MediaPickerController SetupController(MediaPickerDelegate mpDelegate)
        //{
        //    var picker = new MediaPickerController(mpDelegate)
        //    {
        //        MediaTypes = new[] { TypeImage },
        //        SourceType = UIImagePickerControllerSourceType.Camera,
        //        //AllowsEditing = true,
        //        CameraDevice = UIImagePickerControllerCameraDevice.Front,
        //        ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen,
        //        CameraCaptureMode = UIImagePickerControllerCameraCaptureMode.Photo
        //    };

        //    //picker.AllowsEditing = true;
        //    //picker.CameraDevice = UIImagePickerControllerCameraDevice.Front;
        //    //picker.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;

        //    //if (options.OverlayViewProvider != null)
        //    //{
        //    //    var overlay = options.OverlayViewProvider();
        //    //    if (overlay is UIView)
        //    //    {
        //    //        picker.CameraOverlayView = overlay as UIView;
        //    //    }
        //    //}
        //    //if (mediaType == TypeImage)
        //    //{
        //    //picker.CameraCaptureMode = UIImagePickerControllerCameraCaptureMode.Photo;
        //    //}
        //    //else if (mediaType == TypeMovie)
        //    //{
        //    //    var voptions = (StoreVideoOptions)options;

        //    //    picker.CameraCaptureMode = UIImagePickerControllerCameraCaptureMode.Video;
        //    //    picker.VideoQuality = GetQuailty(voptions.Quality);
        //    //    picker.VideoMaximumDuration = voptions.DesiredLength.TotalSeconds;
        //    //}


        //    return picker;
        //}

        private void Dismiss(UIPopoverController popover, UIViewController picker)
        {
            popover?.Dispose();

            try
            {
                picker?.Dispose();
            }
            catch
            {

            }

#if IOS || MACCATALYST
            Interlocked.Exchange(ref pickerDelegate, null);
#endif
        }
        #endregion

        ValueTask<string> IMediaService.PickImage(StoreField storeField, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return new ValueTask<string>(string.Empty);

            //if (!IsPickPhotoSupported)
            //    throw new NotSupportedException();


            ////Does not need permission on iOS 11
            ////if (!UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            ////{
            ////    CheckUsageDescription(photoDescription);

            ////    await CheckPermissions(Permission.Photos);
            ////}

            //var cameraOptions = new StoreCameraMediaOptions
            //{
            //    PhotoSize = options?.PhotoSize ?? PhotoSize.Full,
            //    CompressionQuality = options?.CompressionQuality ?? 100,
            //    AllowCropping = false,
            //    CustomPhotoSize = options?.CustomPhotoSize ?? 100,
            //    MaxWidthHeight = options?.MaxWidthHeight,
            //    RotateImage = options?.RotateImage ?? true,
            //    SaveMetaData = options?.SaveMetaData ?? true,
            //    SaveToAlbum = false,
            //    ModalPresentationStyle = options?.ModalPresentationStyle ?? MediaPickerModalPresentationStyle.FullScreen,
            //};

            //return await GetMediaAsync(UIImagePickerControllerSourceType.PhotoLibrary, TypeImage, cameraOptions, token);

            return new ValueTask<string>(string.Empty);
        }

        ValueTask<string> IMediaService.ConsolideImage(StoreField storeField)
        {
            var drafFileDirectory = FullDraftDirectory(storeField);
            var fileDirectory = FullDirectory(storeField);

            if (Directory.Exists(fileDirectory))
                Directory.Delete(fileDirectory, true);

            var storageFiles = Directory.EnumerateFiles(drafFileDirectory);
            var storageFile = storageFiles.FirstOrDefault();

            if (storageFile != null)
            {
                if (!Directory.Exists(fileDirectory))
                    Directory.CreateDirectory(fileDirectory);

                var file = Path.Combine(fileDirectory, Path.GetFileName(storageFile));
                File.Move(storageFile, file);

                Directory.Delete(drafFileDirectory, true);

                return new ValueTask<string>(file);
            }

            return new ValueTask<string>(string.Empty);
        }

        ValueTask IMediaService.UndoImage(StoreField storeField)
        {
            var path = FullDraftDirectory(storeField);
            if (Directory.Exists(path))
                Directory.Delete(path, true);

            return new ValueTask();
        }

        ValueTask<IEnumerable<string>> IMediaService.PickImages(StoreField storeField, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return new ValueTask<IEnumerable<string>>();

            return new ValueTask<IEnumerable<string>>();
        }

        ValueTask<string> IMediaService.GetImage(StoreField storeField)
        {
            string directory = FullDirectory(storeField);
            string filePath = string.Empty;

            if (Directory.Exists(directory))
            {
                var files = Directory.GetFiles(directory);
                filePath = files.FirstOrDefault();
            }

            return new ValueTask<string>(filePath);
        }

        ValueTask<string> IMediaService.GetImagePath(StoreField storeField, string file)
        {
            string directory = FullDirectory(storeField);
            string filePath = Path.Combine(directory, file);

            return new ValueTask<string>(filePath);
        }

        ValueTask<Stream> IMediaService.ImageStreamForWrite(StoreField storeField, string file)
        {
            string directory = FullDirectory(storeField);
            string filePath = Path.Combine(directory, file);

            return new ValueTask<Stream>(File.OpenWrite(filePath));
        }


        ValueTask<IEnumerable<string>> IMediaService.GetImages(StoreField storeField)
        {
            string directory = FullDirectory(storeField);

            if (Directory.Exists(directory))
            {
                var files = Directory.GetFiles(directory);
                return new ValueTask<IEnumerable<string>>(files);
            }
            else
            {
                return new ValueTask<IEnumerable<string>>(Array.Empty<string>());
            }
        }

        ValueTask IMediaService.DeleteImage(StoreField storeField, string file)
        {
            return new ValueTask();
        }

        ValueTask IMediaService.DeleteImages(StoreField storeField)
        {
            return new ValueTask();
        }

        private string FullDirectory(StoreField storeField)
        {
            var directory = System.IO.Path.Combine("MediaStore", storeField.Container, storeField.Id.ToString(), storeField.Field);
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Library", directory);

            return path;
        }

        private string FullDraftDirectory(StoreField storeField)
        {
            var draftDirectory = System.IO.Path.Combine("MediaDraft", storeField.Container, storeField.Id.ToString(), storeField.Field);
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Library", draftDirectory);

            return path;
        }
    }
}
