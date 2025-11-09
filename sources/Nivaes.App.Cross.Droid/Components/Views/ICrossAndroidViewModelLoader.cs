namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Android.Content;

    public interface ICrossAndroidViewModelLoader
    {
        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming.")]
        ICrossViewModel? Load(Intent? intent, ICrossBundle? savedState);

        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming.")]
        ICrossViewModel? Load(
            Intent? intent,
            ICrossBundle? savedState,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type? viewModelTypeHint);
    }
}