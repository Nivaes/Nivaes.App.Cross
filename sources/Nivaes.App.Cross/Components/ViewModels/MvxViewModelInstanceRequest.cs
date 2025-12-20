namespace MvvmCross.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using Nivaes.App.Cross;

    public class MvxViewModelInstanceRequest(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType)
            : MvxViewModelRequest(viewModelType)
    {
        [UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "Runtime type of ViewModel instance is expected to have public constructors")]
        public MvxViewModelInstanceRequest(ICrossViewModel viewModelInstance)
            : this(viewModelInstance.GetType())
        {
            ViewModelInstance = viewModelInstance;
        }

        public ICrossViewModel? ViewModelInstance { get; set; }
    }
}
