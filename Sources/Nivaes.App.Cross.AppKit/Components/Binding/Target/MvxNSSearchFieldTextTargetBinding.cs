using System.Reflection;
using Microsoft.Extensions.Logging;
using ObjCRuntime;

namespace Nivaes.App.Cross.AppKitLib
{
    public class MvxNSSearchFieldTextTargetBinding
        : MvxPropertyInfoTargetBinding<NSSearchField>
    {
        public MvxNSSearchFieldTextTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var searchField = View;
            if (searchField == null)
            {
                CrossBindingLogger.GetLogger<MvxNSSearchFieldTextTargetBinding>().LogError(
                                      $"{nameof(NSTextView)} is null in {nameof(MvxNSSearchFieldTextTargetBinding)} ");
            }
            else
            {
                searchField.Action = new Selector("searchFieldAction:");
            }
        }

        [Export("searchFieldAction:")]
        private void searchFieldAction()
        {
            FireValueChanged(View.StringValue);
        }

        public override CrossBindingMode DefaultMode
        {
            get { return CrossBindingMode.TwoWay; }
        }

        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var searchBar = View;
                if (searchBar != null)
                {
                    //searchBar.TextChanged -= HandleSearchBarValueChanged;
                }
            }
        }
    }
}
