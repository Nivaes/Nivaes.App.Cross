namespace Nivaes.App.Cross;

internal class CombinersContainers
{
    public Dictionary<string, ICrossValueCombiner> NameCombiners { get; } = new Dictionary<string, ICrossValueCombiner>();

    public Dictionary<Type, ICrossValueCombiner> Combiners { get; } = new Dictionary<Type, ICrossValueCombiner>();
}
