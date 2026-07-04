namespace Nivaes.App.Cross.AppKitLib
{
    public class MvxMacTask
    {
        protected bool DoUrlOpen(NSUrl url)
        {
            var sharedWorkSpace = NSWorkspace.SharedWorkspace;
            return sharedWorkSpace.OpenUrl(url);
        }
    }
}
