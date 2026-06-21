namespace MvvmCross.Platforms.Mac.Binding.Target
{
    using System.Reflection;
    using AppKit;
    using Foundation;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Binding;
    using Nivaes.App.Cross;
    using ObjCRuntime;

    public class MvxNSSearchFieldTextTargetBinding 
        : MvxPropertyInfoTargetBinding<NSSearchField>
    {
        public MvxNSSearchFieldTextTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var searchField = View;
            if (searchField == null)
            {
                CrossBindingLogger.Instance?.LogError(
                                      "NSSearchField is null in MvxNSSearchFieldTextTargetBinding");
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
                    //                    searchBar.TextChanged -= HandleSearchBarValueChanged;
                }
            }
        }
    }
}
