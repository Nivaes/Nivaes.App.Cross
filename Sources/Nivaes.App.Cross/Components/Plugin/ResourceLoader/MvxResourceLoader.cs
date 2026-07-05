namespace Nivaes.App.Cross;

public abstract class MvxResourceLoader
    : ICrossResourceLoader
{
    #region Implementation of IMvxResourceLoader

    public string? GetTextResource(string resourcePath)
    {
        try
        {
            string? text = null;
            GetResourceStream(resourcePath, (stream) =>
                {
                    if (stream == null)
                        return;

                    using (var textReader = new StreamReader(stream))
                    {
                        text = textReader.ReadToEnd();
                    }
                });
            return text;
        }
        //#if !NETFX_CORE
        //            catch (ThreadAbortException)
        //            {
        //                throw;
        //            }
        //#endif
        catch (Exception ex)
        {
            throw new CrossException(ex, "Cannot load resource {0}", resourcePath);
        }
    }

    public abstract void GetResourceStream(string resourcePath, Action<Stream> streamAction);

    public virtual bool ResourceExists(string resourcePath)
    {
        try
        {
            var found = false;
            GetResourceStream(resourcePath, stream => { found = stream != null; });
            return found;
        }
        catch (Exception)
        {
            return false;
        }
    }

    #endregion Implementation of IMvxResourceLoader
}
