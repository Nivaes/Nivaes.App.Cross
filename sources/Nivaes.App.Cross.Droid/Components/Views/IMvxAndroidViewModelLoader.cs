using Android.Content;

namespace MvvmCross.Platforms.Android.Views
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public interface IMvxAndroidViewModelLoader
    {
        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming.")]
        ICrossViewModel? Load(Intent? intent, IMvxBundle? savedState);

        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming.")]
        ICrossViewModel? Load(
            Intent? intent,
            IMvxBundle? savedState,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type? viewModelTypeHint);
    }
}