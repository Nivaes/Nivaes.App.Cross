var builder = DistributedApplication.CreateBuilder(args);

if (OperatingSystem.IsMacOS())
{
    var iosAndroid = builder.AddMauiProject("Sample-iOS", "../../../samples/Nivaes.App.Cross.Sample.UIKit.iOS/Nivaes.App.Cross.Sample.UIKit.iOS.csproj");
    iosAndroid.AddiOSSimulator();

    var macCatalystAndroid = builder.AddMauiProject("Sample-Droid", "../../../samples/Nivaes.App.Cross.Sample.UIKit.MacCatalyst/Nivaes.App.Cross.Sample.UIKit.MacCatalyst.csproj");
    macCatalystAndroid.AddMacCatalystDevice();
}

var appAndroid = builder.AddMauiProject("Sample-Droid", "../../../samples/Nivaes.App.Cross.Sample.Droid/Nivaes.App.Cross.Sample.Droid.csproj");
appAndroid.AddAndroidEmulator();

builder.AddProject<Projects.Nivaes_App_Cross_Sample_Web>("nivaes-app-cross-sample-web");


if (OperatingSystem.IsWindows())
{
    var appWinUI = builder.AddMauiProject("Sample-WinUi", "../../../samples/Nivaes.App.Cross.Sample.WinUI/Nivaes.App.Cross.Sample.WinUI.csproj");
    appWinUI.AddWindowsDevice();
}

builder.Build().Run();
