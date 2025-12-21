namespace Nivaes.App.Cross
{
    using MvvmCross.Core;
    using MvvmCross.ViewModels;

    public interface ICrossSingletonCache
    {
        ICrossSettings? Settings { get; }
        ICrossInpcInterceptor? InpcInterceptor { get; }
        ICrossStringToTypeParser? Parser { get; }
    }
}