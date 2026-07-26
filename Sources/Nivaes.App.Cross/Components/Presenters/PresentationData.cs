namespace Nivaes.App.Cross.Components.Presenters;

public record PresentationData
{
    public Type ViewModelType
    {
        get;
        set;
    }

    public Type ViewType
    {
        get;
        set;
    }

    public IPresentationAttribute PresentationAttribute
    { 
        get;
        set;
    }
}
