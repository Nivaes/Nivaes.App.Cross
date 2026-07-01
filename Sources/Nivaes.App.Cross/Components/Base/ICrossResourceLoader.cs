namespace Nivaes.App.Cross
{
    using System;
    using System.IO;

    public interface ICrossResourceLoader
    {
        bool ResourceExists(string resourcePath);

        string? GetTextResource(string resourcePath);

        void GetResourceStream(string resourcePath, Action<Stream> streamAction);
    }
}
