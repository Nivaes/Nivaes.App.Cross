using Nivaes.App.Cross.Sample.UIKitLib;

namespace Nivaes.App.Cross.Sample.UIKitLib.MacCatalyst;

public class Application
{
    // This is the main entry point of the application.
    private static void Main(string[] args)
    {
        // if you want to use a different Application Delegate class from "AppDelegate"
        // you can specify it here.
        UIApplication.Main(args, null, typeof(AppDelegate));
    }
}
