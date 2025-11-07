namespace Nivaes.App.Cross
{
    using System;

    public interface ICrossSourceStepFactoryRegistry : ICrossSourceStepFactory
    {
        void AddOrOverwrite(Type type, ICrossSourceStepFactory factory);
    }
}
