namespace Nivaes.App.Cross
{
    public interface ICrossViewModelByNameLookup
    {
        bool TryLookupByName(string name, out Type? viewModelType);

        bool TryLookupByFullName(string name, out Type? viewModelType);
    }
}
