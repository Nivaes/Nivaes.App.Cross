namespace Nivaes.App.Cross.WinUI3;

public interface ICrossWindowsViewModelRequestTranslator
{
    string GetRequestTextFor(CrossViewModelRequest request);

    // Important: if calling GetRequestTextWithKeyFor then you must later call RemoveSubViewModelWithKey on the returned key
    string GetRequestTextWithKeyFor(ICrossViewModel existingViewModelToUse);

    void RemoveSubViewModelWithKey(int key);

    int RequestTextGetKey(string requestText);
}
