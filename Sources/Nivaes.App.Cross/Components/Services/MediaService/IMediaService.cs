namespace Nivaes.App.Cross;

public interface IMediaService
{
    ValueTask Initialize();

    bool IsCameraAvailable { get; }

    bool IsTakePhotoSupported { get; }

    bool IsPickPhotoSupported { get; }

    ValueTask<string> TakePhoto(StoreField storeField, CancellationToken token = default);

    ValueTask<string> PickImage(StoreField storeField, CancellationToken token = default);

    ValueTask<string> ConsolideImage(StoreField storeField);

    ValueTask UndoImage(StoreField storeField);

    ValueTask<IEnumerable<string>> PickImages(StoreField storeField, CancellationToken token = default);

    ValueTask<string> GetImage(StoreField storeField);

    ValueTask<string> GetImagePath(StoreField storeField, string file);

    ValueTask<Stream> ImageStreamForWrite(StoreField storeField, string file);

    ValueTask<IEnumerable<string>> GetImages(StoreField storeField);

    ValueTask DeleteImage(StoreField storeField, string file);

    ValueTask DeleteImages(StoreField storeField);
}
