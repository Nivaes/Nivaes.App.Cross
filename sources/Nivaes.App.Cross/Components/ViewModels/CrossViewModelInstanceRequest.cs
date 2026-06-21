namespace Nivaes.App.Cross
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    public class CrossViewModelInstanceRequest(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType)
            : CrossViewModelRequest(viewModelType)
    {
        public CrossViewModelInstanceRequest(ICrossViewModel viewModelInstance)
            : this(viewModelInstance.GetType())
        {
            ViewModelInstance = viewModelInstance;
        }

        public ICrossViewModel? ViewModelInstance { [DebuggerHidden]get; [DebuggerHidden]set; }
    }
}
