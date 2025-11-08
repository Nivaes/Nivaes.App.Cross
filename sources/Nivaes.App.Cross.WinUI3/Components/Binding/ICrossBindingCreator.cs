namespace Nivaes.App.Cross.WinUI3
{
    using Microsoft.UI.Xaml;

    public interface ICrossBindingCreator
    {
        void CreateBindings(
            object sender,
            DependencyPropertyChangedEventArgs args,
            Func<string, IEnumerable<CrossBindingDescription>> parseBindingDescriptions);
    }
}
