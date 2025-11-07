namespace Nivaes.App.Cross
{
    public interface ICrossSingletonCache
    {
        ICrossSettings? Settings { get; }
        ICrossInpcInterceptor? InpcInterceptor { get; }
        ICrossStringToTypeParser? Parser { get; }
    }
}