using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross;

namespace Nivaes.App.Cross.UIKitLib
{
    public class MvxUISearchBarTextTargetBinding(UISearchBar target, PropertyInfo targetPropertyInfo)
        : MvxPropertyInfoTargetBinding<UISearchBar>(target, targetPropertyInfo)
    {
        private CrossWeakEventSubscription<UISearchBar, UISearchBarTextChangedEventArgs>? _subscription;

        public override CrossBindingMode DefaultMode => CrossBindingMode.TwoWay;

        [RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public override void SubscribeToEvents()
        {
            var searchBar = View;
            if (searchBar == null)
            {
                CrossBindingLogger.Instance?.LogError(
                    "UISearchBar is null in {TargetBindingName}", nameof(MvxUISearchBarTextTargetBinding));
                return;
            }

            _subscription =
                searchBar.WeakSubscribe<UISearchBar, UISearchBarTextChangedEventArgs>(nameof(searchBar.TextChanged),
                    HandleSearchBarValueChanged);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (!isDisposing) return;

            _subscription?.Dispose();
            _subscription = null;
        }

        private void HandleSearchBarValueChanged(object? sender, UISearchBarTextChangedEventArgs e)
        {
            FireValueChanged(View?.Text);
        }
    }
}