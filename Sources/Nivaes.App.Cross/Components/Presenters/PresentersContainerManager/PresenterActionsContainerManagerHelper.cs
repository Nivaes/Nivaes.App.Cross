using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Nivaes.App.Cross
{
    public static class PresenterActionsContainerManagerHelper
    {
        public sealed class PresentationAttributePresenterActionItem
        {
            internal PresentationAttributePresenterActionsKeyContainerManager.KeyStoreItem PresentationAttributePresenterActions { [DebuggerHidden] get; [DebuggerHidden] set; }
        }

        public static PresentationAttributePresenterActionItem New<TPresentationAttribute, TPressenterAction>(IServiceProvider services)
            where TPresentationAttribute : IPresentationAttribute
            where TPressenterAction : IPressenterAction
        {
            var pressenterAction = ActivatorUtilities.CreateInstance<IPressenterAction>(services);

            return new PresentationAttributePresenterActionItem()
            {
                PresentationAttributePresenterActions = new KeyContainerManager<IPressenterAction>.KeyStoreItem { Key = typeof(TPresentationAttribute).GetHashCode(), Value = pressenterAction }
            };
        }

        public static void RegisterPresenterActions(PresentationAttributePresenterActionItem[] items)
        {
            Singleton<PresentationAttributePresenterActionsKeyContainerManager>.Instance.Merge(items.Select(x => x.PresentationAttributePresenterActions).ToArray());
        }
    }
}
