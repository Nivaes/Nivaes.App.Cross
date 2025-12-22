namespace MvvmCross.Platforms.Tvos.Binding.Target
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Binding;
    using Nivaes.App.Cross;

    public class MvxUISearchBarTextTargetBinding 
        : MvxPropertyInfoTargetBinding<UISearchBar>
    {
        public MvxUISearchBarTextTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var searchBar = View;
            if (searchBar == null)
            {
                MvxBindingLog.Instance?.LogError("UISearchBar is null in MvxUISearchBarTextTargetBinding");
            }
            else
            {
                searchBar.TextChanged += HandleSearchBarValueChanged;
            }
        }

        private void HandleSearchBarValueChanged(object sender, UISearchBarTextChangedEventArgs e)
        {
            FireValueChanged(View.Text);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var searchBar = View;
                if (searchBar != null)
                {
                    searchBar.TextChanged -= HandleSearchBarValueChanged;
                }
            }
            base.Dispose(isDisposing);
        }
    }
}
