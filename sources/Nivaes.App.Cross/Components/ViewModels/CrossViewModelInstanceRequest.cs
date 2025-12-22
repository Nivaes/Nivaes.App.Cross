namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;

    public class CrossViewModelInstanceRequest(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType)
            : CrossViewModelRequest(viewModelType)
    {
        [UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "Runtime type of ViewModel instance is expected to have public constructors")]
        public CrossViewModelInstanceRequest(ICrossViewModel viewModelInstance)
            : this(viewModelInstance.GetType())
        {
            ViewModelInstance = viewModelInstance;
        }

        public ICrossViewModel? ViewModelInstance { get; set; }
    }
}
