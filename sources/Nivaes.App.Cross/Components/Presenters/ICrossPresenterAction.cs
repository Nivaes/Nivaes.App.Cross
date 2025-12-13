namespace Nivaes.App.Cross
{
    using System;

    public interface ICrossPresenterAction
    {
        public Type ViewType { get; }
    }
}
