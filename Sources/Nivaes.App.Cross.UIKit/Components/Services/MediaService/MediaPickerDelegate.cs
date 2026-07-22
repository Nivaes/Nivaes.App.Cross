#if IOS || MACCATALYST
using NSAction = System.Action;

namespace Nivaes.App.Cross.UIKitLib
{

    internal class MediaPickerDelegate
        : UIImagePickerControllerDelegate
    {
        private UIDeviceOrientation? orientation;
        private NSObject observer;
        private readonly UIViewController mViewController;
        //private readonly UIImagePickerControllerSourceType source;
        private TaskCompletionSource<IEnumerable<string>> mTcs;
        //private readonly StoreCameraMediaOptions options;
        private readonly string mPath;

        internal MediaPickerDelegate(UIViewController viewController, string path, CancellationToken token)
        {
            mViewController = viewController;
            //source = sourceType;
            //this.options = options ?? new StoreCameraMediaOptions();
            mTcs = new TaskCompletionSource<IEnumerable<string>>();
            mPath = path;

            if (viewController != null)
            {
                UIDevice.CurrentDevice.BeginGeneratingDeviceOrientationNotifications();
                observer = NSNotificationCenter.DefaultCenter.AddObserver(UIDevice.OrientationDidChangeNotification, DidRotate);
            }
        }

        public UIPopoverController Popover
        {
            get;
            set;
        }

        public void CancelTask()
        {
            mTcs.SetResult(null);
        }

        public UIView View => mViewController.View;

        public Task<IEnumerable<string>> Task => mTcs.Task;

        public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForOperation(UINavigationController navigationController, UINavigationControllerOperation operation, UIViewController fromViewController, UIViewController toViewController)
        {
            var aa = base.GetAnimationControllerForOperation(navigationController, operation, fromViewController, toViewController);
            return aa;
        }

        public override IUIViewControllerInteractiveTransitioning GetInteractionControllerForAnimationController(UINavigationController navigationController, IUIViewControllerAnimatedTransitioning animationController)
        {
            var aa = base.GetInteractionControllerForAnimationController(navigationController, animationController);
            return aa;
        }

        public override void FinishedPickingMedia(UIImagePickerController picker, NSDictionary info)
        {
            RemoveOrientationChangeObserverAndNotifications();

            string filePath;
            //MediaFile mediaFile;
            switch ((NSString)info[UIImagePickerController.MediaType])
            {
                case MediaService.TypeImage:
                    filePath = GetPictureMediaFile(info);
                    break;

                //case MediaImplementation.TypeMovie:
                //    mediaFile = await GetMovieMediaFile(info);
                //    break;

                default:
                    throw new NotSupportedException();
            }

            //if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
            //{
            //    UIApplication.SharedApplication.SetStatusBarStyle(MediaImplementation.StatusBarStyle, false);
            //}

            Dismiss(picker, () =>
            {
                //if (mediaFile == null)
                //    tcs.SetException(new FileNotFoundException());
                //else
                //    tcs.TrySetResult(new List<MediaFile> { mediaFile });
                mTcs.TrySetResult(new[] { filePath });
            });
        }

        public void Canceled(UINavigationController picker)
        {
            RemoveOrientationChangeObserverAndNotifications();

            //if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
            //{
            //    UIApplication.SharedApplication.SetStatusBarStyle(MediaImplementation.StatusBarStyle, false);
            //}

            Dismiss(picker, () =>
            {
                mTcs.SetResult(null);
            });
        }

        public override void Canceled(UIImagePickerController picker)
        {
            RemoveOrientationChangeObserverAndNotifications();

            //if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
            //{
            //    UIApplication.SharedApplication.SetStatusBarStyle(MediaImplementation.StatusBarStyle, false);
            //}

            Dismiss(picker, () =>
            {
                mTcs.SetResult(null);
            });
        }

        public void DisplayPopover(bool hideFirst = false)
        {
            if (Popover == null)
                return;

            var swidth = UIScreen.MainScreen.Bounds.Width;
            var sheight = UIScreen.MainScreen.Bounds.Height;

            nfloat width = 400;
            nfloat height = 300;


            //if (orientation == null)
            //{
            //    if (IsValidInterfaceOrientation(UIDevice.CurrentDevice.Orientation))
            //        orientation = UIDevice.CurrentDevice.Orientation;
            //    else
            //        orientation = GetDeviceOrientation(viewController.InterfaceOrientation);
            //}

            double x, y;
            if (orientation == UIDeviceOrientation.LandscapeLeft || orientation == UIDeviceOrientation.LandscapeRight)
            {
                x = (Math.Max(swidth, sheight) - width) / 2;
                y = (Math.Min(swidth, sheight) - height) / 2;
            }
            else
            {
                x = (Math.Min(swidth, sheight) - width) / 2;
                y = (Math.Max(swidth, sheight) - height) / 2;
            }

            if (hideFirst && Popover.PopoverVisible)
                Popover.Dismiss(animated: false);

            //Popover.PresentFromRect(new CGRect(x, y, width, height), View, 0, animated: true);
        }



        //private bool IsCaptured =>
        //    source == UIImagePickerControllerSourceType.Camera;

        private void Dismiss(UINavigationController picker, NSAction onDismiss)
        {
            if (mViewController == null)
            {
                onDismiss();
                mTcs = new TaskCompletionSource<IEnumerable<string>>();
            }
            else
            {
                if (Popover != null)
                {
                    Popover.Dismiss(animated: true);
                    try
                    {
                        Popover.Dispose();
                    }
                    catch (Exception ex)
                    {

                    }
                    Popover = null;

                    onDismiss();
                }
                else
                {
                    picker.DismissViewController(true, onDismiss);
                }
            }
        }

        private void RemoveOrientationChangeObserverAndNotifications()
        {
            if (mViewController != null)
            {
                UIDevice.CurrentDevice.EndGeneratingDeviceOrientationNotifications();
                NSNotificationCenter.DefaultCenter.RemoveObserver(observer);
                observer.Dispose();
            }
        }

        private void DidRotate(NSNotification notice)
        {
            var device = (UIDevice)notice.Object;
            //if (!IsValidInterfaceOrientation(device.Orientation) || Popover == null)
            //    return;
            //if (orientation.HasValue && IsSameOrientationKind(orientation.Value, device.Orientation))
            //    return;

            if (UIDevice.CurrentDevice.CheckSystemVersion(6, 0))
            {
                if (!GetShouldRotate6(device.Orientation))
                    return;
            }
            else if (!GetShouldRotate(device.Orientation))
                return;

            var co = orientation;
            orientation = device.Orientation;

            if (co == null)
                return;

            DisplayPopover(hideFirst: true);
        }

        private bool GetShouldRotate(UIDeviceOrientation orientation)
        {
            var iorientation = UIInterfaceOrientation.Portrait;
            switch (orientation)
            {
                case UIDeviceOrientation.LandscapeLeft:
                    iorientation = UIInterfaceOrientation.LandscapeLeft;
                    break;

                case UIDeviceOrientation.LandscapeRight:
                    iorientation = UIInterfaceOrientation.LandscapeRight;
                    break;

                case UIDeviceOrientation.Portrait:
                    iorientation = UIInterfaceOrientation.Portrait;
                    break;

                case UIDeviceOrientation.PortraitUpsideDown:
                    iorientation = UIInterfaceOrientation.PortraitUpsideDown;
                    break;

                default: return false;
            }

            return mViewController.ShouldAutorotateToInterfaceOrientation(iorientation);
        }

        private bool GetShouldRotate6(UIDeviceOrientation orientation)
        {
            if (!mViewController.ShouldAutorotate())
                return false;

            var mask = UIInterfaceOrientationMask.Portrait;
            switch (orientation)
            {
                case UIDeviceOrientation.LandscapeLeft:
                    mask = UIInterfaceOrientationMask.LandscapeLeft;
                    break;

                case UIDeviceOrientation.LandscapeRight:
                    mask = UIInterfaceOrientationMask.LandscapeRight;
                    break;

                case UIDeviceOrientation.Portrait:
                    mask = UIInterfaceOrientationMask.Portrait;
                    break;

                case UIDeviceOrientation.PortraitUpsideDown:
                    mask = UIInterfaceOrientationMask.PortraitUpsideDown;
                    break;

                default: return false;
            }

            return mViewController.GetSupportedInterfaceOrientations().HasFlag(mask);
        }


        private string GetPictureMediaFile(NSDictionary info)
        {
            var image = (UIImage)info[UIImagePickerController.EditedImage] ?? (UIImage)info[UIImagePickerController.OriginalImage];

            if (image == null)
                return null;

            //var path = GetOutputPath();

            //var cgImage = image.CGImage;

            //var percent = 1.0f;
            //float newHeight = image.CGImage.Height;
            //float newWidth = image.CGImage.Width;

            //NSDictionary meta = null;

            //iOS quality is 0.0-1.0
            //var quality = (options.CompressionQuality / 100f);

            //{
            //    //var finalQuality = quality;
            using (var imageData = image.AsJPEG())
            {
                NSError error = null;
                try
                {
                    _ = imageData.Save(mPath, true, out error);
                    if (error != null && error.Code != 0)
                        throw new AppException(error.Description);
                }
                finally
                {
                    error?.Dispose();
                }
            }


            //string aPath = null;
            //if (source != UIImagePickerControllerSourceType.Camera)
            //{

            //try to get the album path's url
            //var url = (NSUrl)info[UIImagePickerController.ReferenceUrl];
            //var aPath = url?.AbsoluteString;
            //}
            //else
            //{
            //    if (options.SaveToAlbum)
            //    {
            //        try
            //        {
            //            var library = new ALAssetsLibrary();
            //            var albumSave = await library.WriteImageToSavedPhotosAlbumAsync(cgImage, meta);
            //            aPath = albumSave.AbsoluteString;
            //        }
            //        catch (Exception ex)
            //        {
            //            Console.WriteLine("unable to save to album:" + ex);
            //        }
            //    }
            //}

            //Func<Stream> getStreamForExternalStorage = () =>
            //{
            //    if (options.RotateImage)
            //        return RotateImage(image, options.CompressionQuality);
            //    else
            //        return File.OpenRead(path);
            //};

            //return new MediaFile(path, () => File.OpenRead(path), streamGetterForExternalStorage: () => getStreamForExternalStorage(), albumPath: aPath);

            return mPath;
        }

        //private string GetOutputPath()
        //{
        //    var aa = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        //    var bb = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        //    var cc = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        //    var dd = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        //    var ee = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        //    var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Library", $"{Guid.NewGuid()}.jpg");
        //    return path;
        //}

        //internal static NSDictionary SetGpsLocation(NSDictionary meta, Location location)
        //{
        //    var newMeta = new NSMutableDictionary();
        //    newMeta.SetValuesForKeysWithDictionary(meta);
        //    var newGpsDict = new NSMutableDictionary();
        //    newGpsDict.SetValueForKey(new NSNumber(Math.Abs(location.Latitude)), ImageIO.CGImageProperties.GPSLatitude);
        //    newGpsDict.SetValueForKey(new NSString(location.Latitude > 0 ? "N" : "S"), ImageIO.CGImageProperties.GPSLatitudeRef);
        //    newGpsDict.SetValueForKey(new NSNumber(Math.Abs(location.Longitude)), ImageIO.CGImageProperties.GPSLongitude);
        //    newGpsDict.SetValueForKey(new NSString(location.Longitude > 0 ? "E" : "W"), ImageIO.CGImageProperties.GPSLongitudeRef);
        //    newGpsDict.SetValueForKey(new NSNumber(location.Altitude), ImageIO.CGImageProperties.GPSAltitude);
        //    newGpsDict.SetValueForKey(new NSNumber(0), ImageIO.CGImageProperties.GPSAltitudeRef);
        //    newGpsDict.SetValueForKey(new NSNumber(location.Speed), ImageIO.CGImageProperties.GPSSpeed);
        //    newGpsDict.SetValueForKey(new NSString("K"), ImageIO.CGImageProperties.GPSSpeedRef);
        //    newGpsDict.SetValueForKey(new NSNumber(location.Direction), ImageIO.CGImageProperties.GPSImgDirection);
        //    newGpsDict.SetValueForKey(new NSString("T"), ImageIO.CGImageProperties.GPSImgDirectionRef);
        //    newGpsDict.SetValueForKey(new NSString(location.Timestamp.ToString("hh:mm:ss")), ImageIO.CGImageProperties.GPSTimeStamp);
        //    newGpsDict.SetValueForKey(new NSString(location.Timestamp.ToString("yyyy:MM:dd")), ImageIO.CGImageProperties.GPSDateStamp);
        //    newMeta[ImageIO.CGImageProperties.GPSDictionary] = newGpsDict;
        //    return newMeta;
        //}

        //internal static bool SaveImageWithMetadata(UIImage image, float quality, NSDictionary meta, string path)
        //{
        //    try
        //    {
        //        var finalQuality = quality;
        //        var imageData = image.AsJPEG(finalQuality);

        //        //continue to move down quality , rare instances
        //        while (imageData == null && finalQuality > 0)
        //        {
        //            finalQuality -= 0.05f;
        //            imageData = image.AsJPEG(finalQuality);
        //        }

        //        if (imageData == null)
        //            throw new NullReferenceException("Unable to convert image to jpeg, please ensure file exists or lower quality level");

        //        var dataProvider = new CGDataProvider(imageData);
        //        var cgImageFromJpeg = CGImage.FromJPEG(dataProvider, null, false, CGColorRenderingIntent.Default);
        //        var imageWithExif = new NSMutableData();
        //        var destination = CGImageDestination.Create(imageWithExif, UTType.JPEG, 1);
        //        var cgImageMetadata = new CGMutableImageMetadata();
        //        var destinationOptions = new CGImageDestinationOptions();

        //        if (meta.ContainsKey(ImageIO.CGImageProperties.Orientation))
        //            destinationOptions.Dictionary[ImageIO.CGImageProperties.Orientation] = meta[ImageIO.CGImageProperties.Orientation];

        //        if (meta.ContainsKey(ImageIO.CGImageProperties.DPIWidth))
        //            destinationOptions.Dictionary[ImageIO.CGImageProperties.DPIWidth] = meta[ImageIO.CGImageProperties.DPIWidth];

        //        if (meta.ContainsKey(ImageIO.CGImageProperties.DPIHeight))
        //            destinationOptions.Dictionary[ImageIO.CGImageProperties.DPIHeight] = meta[ImageIO.CGImageProperties.DPIHeight];


        //        if (meta.ContainsKey(ImageIO.CGImageProperties.ExifDictionary))
        //        {

        //            destinationOptions.ExifDictionary =
        //                                  new CGImagePropertiesExif(meta[ImageIO.CGImageProperties.ExifDictionary] as NSDictionary);

        //        }


        //        if (meta.ContainsKey(ImageIO.CGImageProperties.TIFFDictionary))
        //        {
        //            var newTiffDict = meta[ImageIO.CGImageProperties.TIFFDictionary] as NSDictionary;
        //            if (newTiffDict != null)
        //            {
        //                newTiffDict.SetValueForKey(meta[ImageIO.CGImageProperties.Orientation], ImageIO.CGImageProperties.TIFFOrientation);
        //                destinationOptions.TiffDictionary = new CGImagePropertiesTiff(newTiffDict);
        //            }

        //        }
        //        if (meta.ContainsKey(ImageIO.CGImageProperties.GPSDictionary))
        //        {
        //            destinationOptions.GpsDictionary =
        //                new CGImagePropertiesGps(meta[ImageIO.CGImageProperties.GPSDictionary] as NSDictionary);
        //        }
        //        if (meta.ContainsKey(ImageIO.CGImageProperties.JFIFDictionary))
        //        {
        //            destinationOptions.JfifDictionary =
        //                new CGImagePropertiesJfif(meta[ImageIO.CGImageProperties.JFIFDictionary] as NSDictionary);
        //        }
        //        if (meta.ContainsKey(ImageIO.CGImageProperties.IPTCDictionary))
        //        {
        //            destinationOptions.IptcDictionary =
        //                new CGImagePropertiesIptc(meta[ImageIO.CGImageProperties.IPTCDictionary] as NSDictionary);
        //        }
        //        destination.AddImageAndMetadata(cgImageFromJpeg, cgImageMetadata, destinationOptions);
        //        var success = destination.Close();
        //        if (success)
        //        {
        //            imageWithExif.Save(path, true);
        //            imageWithExif.Dispose();
        //            imageWithExif = null;
        //        }

        //        return success;

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Unable to save image with metadata: {ex}");
        //    }

        //    return false;
        //}


        //private async Task<MediaFile> GetMovieMediaFile(NSDictionary info)
        //{
        //    var url = info[UIImagePickerController.MediaURL] as NSUrl;
        //    if (url == null)
        //        return null;

        //    var path = GetOutputPath(MediaImplementation.TypeMovie,
        //              options?.Directory ?? ((IsCaptured) ? string.Empty : "temp"),
        //              options?.Name ?? Path.GetFileName(url.Path));


        //    File.Move(url.Path, path);

        //    string aPath = null;
        //    if (source != UIImagePickerControllerSourceType.Camera)
        //    {
        //        //try to get the album path's url
        //        var url2 = info[UIImagePickerController.ReferenceUrl] as NSUrl;
        //        aPath = url2?.AbsoluteString;
        //    }
        //    else
        //    {
        //        if (options?.SaveToAlbum ?? false)
        //        {
        //            try
        //            {
        //                var library = new ALAssetsLibrary();
        //                var albumSave = await library.WriteVideoToSavedPhotosAlbumAsync(new NSUrl(path));
        //                aPath = albumSave.AbsoluteString;
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine("unable to save to album:" + ex);
        //            }
        //        }
        //    }

        //    return new MediaFile(path, () => File.OpenRead(path), albumPath: aPath);
        //}

        //private static bool IsValidInterfaceOrientation(UIDeviceOrientation self)
        //{
        //    return (self != UIDeviceOrientation.FaceUp && self != UIDeviceOrientation.FaceDown && self != UIDeviceOrientation.Unknown);
        //}

        //private static bool IsSameOrientationKind(UIDeviceOrientation o1, UIDeviceOrientation o2)
        //{
        //    if (o1 == UIDeviceOrientation.FaceDown || o1 == UIDeviceOrientation.FaceUp)
        //        return (o2 == UIDeviceOrientation.FaceDown || o2 == UIDeviceOrientation.FaceUp);
        //    if (o1 == UIDeviceOrientation.LandscapeLeft || o1 == UIDeviceOrientation.LandscapeRight)
        //        return (o2 == UIDeviceOrientation.LandscapeLeft || o2 == UIDeviceOrientation.LandscapeRight);
        //    if (o1 == UIDeviceOrientation.Portrait || o1 == UIDeviceOrientation.PortraitUpsideDown)
        //        return (o2 == UIDeviceOrientation.Portrait || o2 == UIDeviceOrientation.PortraitUpsideDown);

        //    return false;
        //}

        //private static UIDeviceOrientation GetDeviceOrientation(UIInterfaceOrientation self)
        //{
        //    switch (self)
        //    {
        //        case UIInterfaceOrientation.LandscapeLeft:
        //            return UIDeviceOrientation.LandscapeLeft;
        //        case UIInterfaceOrientation.LandscapeRight:
        //            return UIDeviceOrientation.LandscapeRight;
        //        case UIInterfaceOrientation.Portrait:
        //            return UIDeviceOrientation.Portrait;
        //        case UIInterfaceOrientation.PortraitUpsideDown:
        //            return UIDeviceOrientation.PortraitUpsideDown;
        //        default:
        //            throw new InvalidOperationException();
        //    }
        //}

        //public static Stream RotateImage(UIImage image, int compressionQuality)
        //{
        //    UIImage imageToReturn = null;
        //    if (image.Orientation == UIImageOrientation.Up)
        //    {
        //        imageToReturn = image;
        //    }
        //    else
        //    {
        //        var transform = CGAffineTransform.MakeIdentity();

        //        switch (image.Orientation)
        //        {
        //            case UIImageOrientation.Down:
        //            case UIImageOrientation.DownMirrored:
        //                transform.Rotate((float)Math.PI);
        //                transform.Translate(image.Size.Width, image.Size.Height);
        //                break;

        //            case UIImageOrientation.Left:
        //            case UIImageOrientation.LeftMirrored:
        //                transform.Rotate((float)Math.PI / 2);
        //                transform.Translate(image.Size.Width, 0);
        //                break;

        //            case UIImageOrientation.Right:
        //            case UIImageOrientation.RightMirrored:
        //                transform.Rotate(-(float)Math.PI / 2);
        //                transform.Translate(0, image.Size.Height);
        //                break;
        //            case UIImageOrientation.Up:
        //            case UIImageOrientation.UpMirrored:
        //                break;
        //        }

        //        switch (image.Orientation)
        //        {
        //            case UIImageOrientation.UpMirrored:
        //            case UIImageOrientation.DownMirrored:
        //                transform.Translate(image.Size.Width, 0);
        //                transform.Scale(-1, 1);
        //                break;

        //            case UIImageOrientation.LeftMirrored:
        //            case UIImageOrientation.RightMirrored:
        //                transform.Translate(image.Size.Height, 0);
        //                transform.Scale(-1, 1);
        //                break;
        //            case UIImageOrientation.Up:
        //            case UIImageOrientation.Down:
        //            case UIImageOrientation.Left:
        //            case UIImageOrientation.Right:
        //                break;
        //        }

        //        using (var context = new CGBitmapContext(IntPtr.Zero,
        //                                                (int)image.Size.Width,
        //                                                (int)image.Size.Height,
        //                                                image.CGImage.BitsPerComponent,
        //                                                image.CGImage.BytesPerRow,
        //                                                image.CGImage.ColorSpace,
        //                                                image.CGImage.BitmapInfo))
        //        {
        //            context.ConcatCTM(transform);
        //            switch (image.Orientation)
        //            {
        //                case UIImageOrientation.Left:
        //                case UIImageOrientation.LeftMirrored:
        //                case UIImageOrientation.Right:
        //                case UIImageOrientation.RightMirrored:
        //                    context.DrawImage(new RectangleF(PointF.Empty, new SizeF((float)image.Size.Height, (float)image.Size.Width)), image.CGImage);
        //                    break;
        //                default:
        //                    context.DrawImage(new RectangleF(PointF.Empty, new SizeF((float)image.Size.Width, (float)image.Size.Height)), image.CGImage);
        //                    break;
        //            }

        //            using (var imageRef = context.ToImage())
        //            {
        //                imageToReturn = new UIImage(imageRef, 1, UIImageOrientation.Up);
        //            }
        //        }
        //    }

        //    var finalQuality = compressionQuality / 100f;
        //    var imageData = imageToReturn.AsJPEG(finalQuality);
        //    //continue to move down quality , rare instances
        //    while (imageData == null && finalQuality > 0)
        //    {
        //        finalQuality -= 0.05f;
        //        imageData = imageToReturn.AsJPEG(finalQuality);
        //    }

        //    if (imageData == null)
        //        throw new NullReferenceException("Unable to convert image to jpeg, please ensure file exists or lower quality level");

        //    var stream = new MemoryStream();
        //    imageData.AsStream().CopyTo(stream);
        //    stream.Position = 0;
        //    imageData.Dispose();
        //    image.Dispose();
        //    image = null;
        //    return stream;

        //}
    }
}
#endif