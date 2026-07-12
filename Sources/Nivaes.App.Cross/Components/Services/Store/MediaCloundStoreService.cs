namespace Nivaes.App.Cross
{
    public class MediaCloundStoreService
        : IMediaCloundStoreService
    {
        private readonly AsyncTemporary<Uri> mSharedAccessSignature;
        private readonly IMediaService mMediaService;

        public MediaCloundStoreService(/*ICRMAccountCommunicationService crmAccountCommunicationService,*/ IMediaService mediaService)
        {
            //mSharedAccessSignature = new AsyncTemporary<Uri>(async () => new Uri(await crmAccountCommunicationService.GetMediaSharedAccessSignature().ConfigureAwait(false)), TimeSpan.FromMinutes(10));
            mMediaService = mediaService;
        }

        public async Task UploadStoreField(StoreField storeField, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
            //if (storeField == null) throw new ArgumentNullException(nameof(storeField));

            //var sharedAccessSignature = await mSharedAccessSignature.GetValue();

            //CloudBlobContainer cloudBlobContainer = new CloudBlobContainer(sharedAccessSignature);

            //var localImages = await mMediaService.GetImages(storeField);

            //var directory = FullDirectory(storeField);
            //var blobDirectory = cloudBlobContainer.GetDirectoryReference(directory);

            //var remoteOriginalBlobs = new List<string>();
            //foreach (var blob in blobDirectory.ListBlobs())
            //{
            //    if (blob is CloudBlockBlob cloundBockBlob)
            //    {
            //        string remoteFileName = Path.GetFileName(cloundBockBlob.Name);
            //        if (!localImages.Contains(remoteFileName))
            //        {
            //            await cloundBockBlob.DeleteIfExistsAsync(cancellationToken).ConfigureAwait(false);
            //        }
            //        else
            //        {
            //            remoteOriginalBlobs.Add(cloundBockBlob.Name);
            //        }
            //    }
            //}

            //foreach (var localImage in localImages)
            //{
            //    var filename = Path.GetFileName(localImage);
            //    if (!remoteOriginalBlobs.Contains(filename))
            //    {
            //        CloudBlockBlob blob = blobDirectory.GetBlockBlobReference(filename);
            //        await blob.UploadFromFileAsync(localImage, cancellationToken).ConfigureAwait(false);
            //    }
            //}
        }

        public async Task DownloadStoreField(StoreField storeField, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
            //if (storeField == null) throw new ArgumentNullException(nameof(storeField));

            //var sharedAccessSignature = await mSharedAccessSignature.GetValue();

            //CloudBlobContainer cloudBlobContainer = new CloudBlobContainer(sharedAccessSignature);

            //var directory = FullDirectory(storeField);
            //var blobDirectory = cloudBlobContainer.GetDirectoryReference(directory);

            //var remoteOriginalBlobs = new List<string>();

            //foreach (var blob in blobDirectory.ListBlobs())
            //{
            //    if (blob is CloudBlockBlob cloundBockBlob)
            //    {
            //        if (!cloundBockBlob.IsDeleted)
            //        {
            //            string fileName = Path.GetFileName(cloundBockBlob.Name);

            //            using (var fileStream = await mMediaService.ImageStreamForWrite(storeField, fileName))
            //            {
            //                await cloundBockBlob.DownloadToStreamAsync(fileStream, cancellationToken).ConfigureAwait(false);
            //            }
            //            remoteOriginalBlobs.Add(fileName);
            //        }
            //    }
            //}

            //var localImages = (await mMediaService.GetImages(storeField)).Select(f => Path.GetFileName(f));
            //foreach (var localImage in localImages)
            //{
            //    if (!remoteOriginalBlobs.Contains(localImage))
            //    {
            //        await mMediaService.DeleteImage(storeField, localImage);
            //    }
            //}
        }

        private string FullDirectory(StoreField storeField) => string.Join("/", storeField.Container, storeField.Id, storeField.Field);
    }
}
