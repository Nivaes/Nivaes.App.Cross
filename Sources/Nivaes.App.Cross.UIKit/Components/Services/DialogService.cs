namespace Nivaes.App.Cross.UIKitLib
{
    public class DialogService : IDialogService
    {
        public void Alert(string message, string title, string okbtnText)
        {
#if IOS || MACCATALYST
            new UIAlertView(title, message, (IUIAlertViewDelegate)null!, "OK", null!).Show();
#endif
        }
    }
}
