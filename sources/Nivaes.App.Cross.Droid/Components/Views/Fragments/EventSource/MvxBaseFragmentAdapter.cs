using Android.Content;

namespace MvvmCross.Platforms.Android.Views.Fragments.EventSource
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.Base;
    using Nivaes.App.Cross;
    using Fragment = AndroidX.Fragment.App.Fragment;

    public class MvxBaseFragmentAdapter
    {
        private readonly ICrossEventSourceFragment _eventSource;

        protected Fragment? Fragment => _eventSource as Fragment;

        [RequiresUnreferencedCode("This class uses reflection which may not be preserved during trimming.")]
        protected MvxBaseFragmentAdapter(ICrossEventSourceFragment eventSource)
        {
            if (eventSource is null)
                throw new ArgumentException("eventSource should not be null", nameof(eventSource));

            if (eventSource is not AndroidX.Fragment.App.Fragment)
                throw new ArgumentException("eventSource should be a Fragment", nameof(eventSource));

            _eventSource = eventSource;
            _eventSource.DisposeCalled += HandleDisposeCalled;
            _eventSource.CreateViewCalled += HandleCreateViewCalled;
            _eventSource.DestroyViewCalled += HandleDestroyViewCalled;
            _eventSource.AttachCalled += HandleAttachCalled;
            _eventSource.CreateCalled += HandleCreateCalled;
            _eventSource.StartCalled += HandleStartCalled;
            _eventSource.StopCalled += HandleStopCalled;
            _eventSource.PauseCalled += HandlePauseCalled;
            _eventSource.ResumeCalled += HandleResumeCalled;
            _eventSource.DetachCalled += HandleDetachCalled;
            _eventSource.SaveInstanceStateCalled += HandleSaveInstanceStateCalled;
        }

        protected virtual void HandleSaveInstanceStateCalled(object? sender, CrossValueEventArgs<Bundle> e)
        {
        }

        protected virtual void HandleDetachCalled(object? sender, EventArgs e)
        {
        }

        protected virtual void HandleResumeCalled(object? sender, EventArgs e)
        {
        }

        protected virtual void HandlePauseCalled(object? sender, EventArgs e)
        {
        }

        protected virtual void HandleStopCalled(object? sender, EventArgs e)
        {
        }

        protected virtual void HandleStartCalled(object? sender, EventArgs e)
        {
        }

        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming.")]
        protected virtual void HandleCreateCalled(object? sender, CrossValueEventArgs<Bundle> e)
        {
        }

        protected virtual void HandleAttachCalled(object? sender, CrossValueEventArgs<Context> e)
        {
        }

        protected virtual void HandleDisposeCalled(object? sender, EventArgs e)
        {
        }

        protected virtual void HandleDestroyViewCalled(object? sender, EventArgs e)
        {
        }

        protected virtual void HandleCreateViewCalled(
            object? sender, CrossValueEventArgs<MvxCreateViewParameters> e)
        {
        }
    }
}