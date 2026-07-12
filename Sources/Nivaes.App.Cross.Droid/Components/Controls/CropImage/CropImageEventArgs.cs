namespace Nivaes.App.Cross.Droid.GropImage
{
    internal class CropImageEventArgs : EventArgs
    {
        public CropImageEventArgs(int id, Exception error)
        {
            RequestId = id;
            Error = error ?? throw new ArgumentNullException(nameof(error));
        }

        public CropImageEventArgs(int id, bool isCanceled)
        {
            RequestId = id;
            IsCanceled = isCanceled;
        }

        public CropImageEventArgs(int id, string imagePath)
        {
            RequestId = id;
            ImageUri = imagePath;
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

        public string? ImageUri
        {
            get;
            private set;
        }
    }
}
