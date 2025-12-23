namespace Nivaes.App.Cross
{
    using System;

    public interface IMvxSourceStepFactoryRegistry : IMvxSourceStepFactory
    {
        void AddOrOverwrite(Type type, IMvxSourceStepFactory factory);
    }
}
