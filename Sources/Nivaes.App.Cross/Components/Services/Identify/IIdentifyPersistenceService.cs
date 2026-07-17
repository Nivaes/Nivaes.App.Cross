namespace Nivaes.App
{
    public interface IIdentifyPersistenceService
    {
        void SaveIdentityCredencials(string identify);

        string LoadIdentityCredencials();

        void DeleteIdentityCredencials();
    }
}
