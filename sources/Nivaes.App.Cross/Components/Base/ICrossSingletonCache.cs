namespace Nivaes.App.Cross
{
    [Obsolete()]
    public interface ICrossSingletonCache
    {
        ICrossSettings? Settings { get; }
        ICrossInpcInterceptor? InpcInterceptor { get; }
        ICrossStringToTypeParser? Parser { get; }
    }
}