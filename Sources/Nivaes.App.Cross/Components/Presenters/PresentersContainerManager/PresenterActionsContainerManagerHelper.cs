using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Nivaes.App.Cross
{
    public static class PresenterActionsContainerManagerHelper
    {
        public sealed class ConverterManagerItem
        {
            internal PresenterActionsKeyContainerManager.KeyStoreItem PresenterActions { [DebuggerHidden] get; [DebuggerHidden] set; }
        }

        public static ConverterManagerItem New<TPressenterAction>(IServiceProvider services)
                        where TPressenterAction : IPressenterAction
        {
            var converter = ActivatorUtilities.CreateInstance<TPressenterAction>(services);

            return new ConverterManagerItem()
            {
                PresenterActions = new PresenterActionsKeyContainerManager.KeyStoreItem { Key = typeof(TPressenterAction).GetHashCode(), Value = converter }
            };
        }

        public static void RegisterPresenterActions(ConverterManagerItem[] items)
        {
            Singleton<PresenterActionsKeyContainerManager>.Instance.Merge(items.Select(x => x.PresenterActions).ToArray());
        }
    }
}
