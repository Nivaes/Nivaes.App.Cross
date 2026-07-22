using System.Diagnostics;
#if IOS || MACCATALYST
using BigTed;
#endif

namespace Nivaes.App.Cross.UIKitLib
{
    public class BusyService : IBusyService
    {
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
#if IOS || MACCATALYST
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                BTProgressHUD.Show(pregressText, maskType: MaskType.None);
            });
#endif

            await (action!.Invoke().ContinueWith((t) =>
            {
#if IOS || MACCATALYST
                UIApplication.SharedApplication.InvokeOnMainThread(BTProgressHUD.Dismiss);
#endif
            })).ConfigureAwait(false);

        }

        [DebuggerStepThrough]
        public async ValueTask<T> Show<T>(string pregressText, Func<Task<T>> action)
        {
#if IOS || MACCATALYST
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                BTProgressHUD.Show(pregressText, maskType: MaskType.None);
            });
#endif

            var task = await (action!.Invoke().ContinueWith((t) =>
            {
#if IOS || MACCATALYST
                UIApplication.SharedApplication.InvokeOnMainThread(BTProgressHUD.Dismiss);
#endif
                return t;
            })).ConfigureAwait(false);

            return await task.ConfigureAwait(false);
        }
    }
}
