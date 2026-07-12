namespace Nivaes.App.Cross
{
    public interface IMediaCloundStoreService
    {
        Task UploadStoreField(StoreField storeField, CancellationToken cancellationToken = default);

        Task DownloadStoreField(StoreField storeField, CancellationToken cancellationToken = default);
    }
}
