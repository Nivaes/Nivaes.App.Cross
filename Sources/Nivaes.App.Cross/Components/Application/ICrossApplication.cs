using Nivaes.App.Cross.Components.ViewModels;

namespace Nivaes.App.Cross;

public interface ICrossApplication
{
    void Setup();

    ICrossViewModelStar Initialize();

    void Startup();

    void Reset();
}
