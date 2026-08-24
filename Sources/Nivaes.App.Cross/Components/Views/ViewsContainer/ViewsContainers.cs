namespace Nivaes.App.Cross;

public class ViewsContainers
{
    public Dictionary<Type, Type> ViewViewModels { get; } = new Dictionary<Type, Type>();
    public Dictionary<Type, Type> ViewModelViews { get; } = new Dictionary<Type, Type>();
}
