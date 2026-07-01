namespace Nivaes.App.Cross
{
    [Obsolete("1")]
    public interface ICrossSingletonCache
    {
        ICrossSettings? Settings { get; }
        ICrossInpcInterceptor? InpcInterceptor { get; }
        ICrossStringToTypeParser? Parser { get; }
    }
}