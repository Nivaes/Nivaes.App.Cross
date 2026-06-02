using Microsoft.UI.Xaml;
using Nivaes.App.Cross.Hosting;
using Nivaes.App.Cross.WinUI;

namespace Nivaes.App.Cross.Sample.WinUI;

public sealed partial class App 
    : CrossWinUIApplication
{
    public App()
    {
        InitializeComponent();
    }

    protected override CrossApp CreateCrossApp() => CrossProgram.CreateCrossApp(this);

    protected override Window CreateWindow()
    {
        return new Window()
        {
            Title = "MvvmCross WinUI 3 Playground"
        };
    }
}
