namespace Nivaes.App.Cross
{
    public interface IMvxTextProviderBuilder
    {
        ICrossTextProvider TextProvider { get; }

        void LoadResources(string whichLocalizationFolder);
    }
}
