namespace Nivaes.App.Cross
{
    using System;

    [Obsolete]
    public interface ICrossPresenterAction
    {
        public Type ViewType { get; }
    }
}
