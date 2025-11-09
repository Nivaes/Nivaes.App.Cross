namespace Nivaes.App.Cross
{
    [Obsolete("No compatible con AoT")]
    public interface ICrossValueConverterRegistry : 
        ICrossNamedInstanceRegistry<ICrossValueConverter>
    {
    }
}
