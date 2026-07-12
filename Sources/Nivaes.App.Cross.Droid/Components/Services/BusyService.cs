using System.Diagnostics;
using AndroidHUD;
using Microsoft.Extensions.DependencyInjection;

namespace Nivaes.App.Cross.Droid
{
    public class BusyService
        : IBusyService
    {
        public BusyService()
        {
        }

        [DebuggerStepThrough]
        public ValueTask Show(Func<Task> action)
        {
            return Show(string.Empty, action);
        }

        [DebuggerStepThrough]
        public ValueTask<T> Show<T>(Func<Task<T>> action)
        {
            return Show(string.Empty, action);
        }

        [DebuggerStepThrough]
        public async ValueTask Show(string pregressText, Func<Task> action)
        {
            var activity = IPlatformApplication.Current!.Services.GetRequiredService<IMvxAndroidCurrentTopActivity>().Activity;

            AndHUD.Shared.Show(activity, pregressText);

            await (action?.Invoke().ContinueWith((t) =>
            {
                AndHUD.Shared.Dismiss(/*activity*/);
            })).ConfigureAwait(false);
        }

        [DebuggerStepThrough]
        public async ValueTask<T> Show<T>(string pregressText, Func<Task<T>> action)
        {
            var activity = IPlatformApplication.Current!.Services.GetRequiredService<IMvxAndroidCurrentTopActivity>().Activity;

            AndHUD.Shared.Show(activity, pregressText);
            var task = await (action?.Invoke().ContinueWith((t) =>
            {
                AndHUD.Shared.Dismiss(/*activity*/);
                return t;
            })).ConfigureAwait(false);

            return await task.ConfigureAwait(false);
        }
    }
}
