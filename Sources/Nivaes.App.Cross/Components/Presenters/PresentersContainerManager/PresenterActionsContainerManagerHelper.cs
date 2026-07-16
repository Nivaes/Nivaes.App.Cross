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
            try
            {
                var pressenterAction = ActivatorUtilities.CreateInstance<TPressenterAction>(services);

                return new PresentationAttributePresenterActionItem()
                {
                    PresentationAttributePresenterActions = new KeyContainerManager<IPressenterAction>.KeyStoreItem { Key = typeof(TPresentationAttribute).TypeHandle.Value, Value = pressenterAction }
                };
            }
            catch(InvalidOperationException ex)
            {
                throw new AppException(ex, $"Could not create an instance of type {typeof(TPressenterAction)}");
            }
        }

        public static void RegisterPresenterActions(PresentationAttributePresenterActionItem[] items)
        {
            Singleton<PresentationAttributePresenterActionsKeyContainerManager>.Instance.Merge(items.Select(x => x.PresentationAttributePresenterActions).ToArray());
        }
    }
}
