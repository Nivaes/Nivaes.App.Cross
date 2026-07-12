namespace Nivaes.App.Cross.Droid
{
    internal class MediaPickedEventArgs : EventArgs
    {
        public MediaPickedEventArgs(int id, Exception error)
        {
            RequestId = id;
            Error = error ?? throw new ArgumentNullException(nameof(error));
        }

        public MediaPickedEventArgs(int id, bool isCanceled)
        {
            RequestId = id;
            IsCanceled = isCanceled;
        }

        public MediaPickedEventArgs(int id, IEnumerable<string> imagesPath)
        {
            RequestId = id;
            ImagesUri = imagesPath;
        }

        public int RequestId
        {
            get;
            private set;
        }

        public bool IsCanceled
        {
            get;
            private set;
        }

        public Exception? Error
        {
            get;
            private set;
        }

        public IEnumerable<string>? ImagesUri
        {
            get;
            private set;
        }
    }
}
