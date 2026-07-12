namespace Nivaes.App.Droid
{
    internal class MediaTakeEventArgs : EventArgs
    {
        public MediaTakeEventArgs(int id, Exception error)
        {
            RequestId = id;
            Error = error ?? throw new ArgumentNullException(nameof(error));
        }

        public MediaTakeEventArgs(int id, bool isCanceled)
        {
            RequestId = id;
            IsCanceled = isCanceled;
        }

        public MediaTakeEventArgs(int id, string imagePath)
        {
            RequestId = id;
            ImagePath = imagePath;
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

        public string? ImagePath
        {
            get;
            private set;
        }
    }
}
