#if IOS || MACCATALYST
namespace Nivaes.App.Cross.UIKitLib
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UIKit;

    /// <summary>
    /// Media Picker Controller
    /// </summary>
    public sealed class MediaPickerController
        : UIImagePickerController
    {
        internal MediaPickerController(MediaPickerDelegate mpDelegate) =>
            base.Delegate = mpDelegate;


        ///// <summary>
        ///// Deleage
        ///// </summary>
        //public override NSObject Delegate
        //{
        //    get => base.Delegate; 
        //    set
        //    {   
        //        if (value == null)
        //            base.Delegate = value;
        //        else
        //            throw new NotSupportedException();
        //    }
        //}

        /// <summary>
        /// Gets result of picker
        /// </summary>
        /// <returns></returns>

        public Task<IEnumerable<string>> GetResultAsync() =>
            ((MediaPickerDelegate)Delegate).Task;

        bool disposed;
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing && !disposed)
            {
                disposed = true;
                InvokeOnMainThread(() =>
                {
                    try
                    {
                        Delegate?.Dispose();
                        Delegate = null;
                    }
                    catch { }
                });
            }
        }
    }
}
#endif