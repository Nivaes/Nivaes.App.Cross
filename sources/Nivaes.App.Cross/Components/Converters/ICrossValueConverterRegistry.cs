namespace Nivaes.App.Cross
{
    [Obsolete("No compatible con AoT", true)]
    public interface ICrossValueConverterRegistry : 
        ICrossNamedInstanceRegistry<ICrossValueConverter>
    {
    }
}
