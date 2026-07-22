using Microsoft.Extensions.DependencyInjection;

namespace Nivaes.App.Cross.Droid
{
    /// <summary>
    /// Provides a service to any code that requires a dialog to be shown to the user for more complex situations
    /// where responses and richer user interaction is required use the IInteractionRequest service.
    /// </summary>
    public class DialogService
        : IDialogService
    {
        private readonly IMvxAndroidCurrentTopActivity _currentTopActivity;

        public DialogService(IMvxAndroidCurrentTopActivity currentTopActivity)
        {
            _currentTopActivity = currentTopActivity;
        }

        /// <summary>Alerts the user with a simple OK dialog and provides a <paramref name="message"/>.</summary>
        /// <param name="message">The message.</param>
        /// <param name="title">The title.</param>
        /// <param name="okbtnText">The okbtn text.</param>
        public void Alert(string message, string title, string okbtnText)
        {
            new AlertDialog.Builder(_currentTopActivity.Activity)
                .SetTitle(title)!
                .SetMessage(message)!
                .SetIcon(Resource.Drawable.ic_notification)!
                .SetCancelable(false)!
                //.SetNegativeButton("Cancelar", (sender, args) => { })
                .SetPositiveButton(okbtnText, (sender, args) => { /* some logic */ })!
                .Create()!.Show();
        }
    }
}
