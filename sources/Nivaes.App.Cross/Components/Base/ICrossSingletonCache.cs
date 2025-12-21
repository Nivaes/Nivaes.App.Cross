namespace Nivaes.App.Cross
{
    using MvvmCross.Core;
    using MvvmCross.ViewModels;

    public interface ICrossSingletonCache
    {
        IMvxSettings? Settings { get; }
        ICrossInpcInterceptor? InpcInterceptor { get; }
        IMvxStringToTypeParser? Parser { get; }
    }
}