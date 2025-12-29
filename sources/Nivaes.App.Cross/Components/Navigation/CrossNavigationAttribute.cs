using System;

namespace Nivaes.App.Cross;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public sealed class CrossNavigationAttribute : Attribute
{
    public Type ViewModelOrFacade { get; }

    public string UriRegex { get; }

    public CrossNavigationAttribute(Type viewModelOrFacade, string uriRegex)
    {
        ViewModelOrFacade = viewModelOrFacade;
        UriRegex = uriRegex;
    }
}
