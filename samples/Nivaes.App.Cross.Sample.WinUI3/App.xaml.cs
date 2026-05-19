using Microsoft.UI.Xaml;
using Nivaes.App.Cross.WinUI3;

namespace Nivaes.App.Cross.Sample.WinUI3;

public sealed partial class App 
    : CrossWinUIApplication
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow()
    {
        return new Window()
        {
            Title = "MvvmCross WinUI 3 Playground"
        };
    }

    protected override void RegisterSetup()
    {
        this.RegisterSetupType<WinUiSampleSetup>();
    }
}
