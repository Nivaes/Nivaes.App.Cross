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
        /// <summary>Alerts the user with a simple OK dialog and provides a <paramref name="message"/>.</summary>
        /// <param name="message">The message.</param>
        /// <param name="title">The title.</param>
        /// <param name="okbtnText">The okbtn text.</param>
        public void Alert(string message, string title, string okbtnText)
        {
            var top = IPlatformApplication.Current!.Services.GetRequiredService<IMvxAndroidCurrentTopActivity>();
            var act = top.Activity;

            new AlertDialog.Builder(act)
                .SetTitle(title)
                .SetMessage(message)
                .SetIcon(Resource.Drawable.ic_notification)
                .SetCancelable(false)
                //.SetNegativeButton("Cancelar", (sender, args) => { })
                .SetPositiveButton(okbtnText, (sender, args) => { /* some logic */ })
                .Create().Show();
        }
    }
}
