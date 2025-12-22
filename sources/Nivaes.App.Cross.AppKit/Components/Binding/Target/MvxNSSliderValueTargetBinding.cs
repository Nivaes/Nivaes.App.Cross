namespace MvvmCross.Platforms.Mac.Binding.Target
{
    using System.Reflection;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Binding;
    using Nivaes.App.Cross;

    public class MvxNSSliderValueTargetBinding : MvxPropertyInfoTargetBinding<NSSlider>
    {
        public MvxNSSliderValueTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var slider = View;
            if (slider == null)
            {
                MvxBindingLog.Instance?.LogError("NSSlider is null in MvxNSSliderValueTargetBinding");
            }
            else
            {
                slider.Activated += HandleSliderActivation;
                //slider.Action = new MonoMac.ObjCRuntime.Selector ("sliderChanged:");
            }
        }

        [Export("sliderChanged:")]
        private void HandleSliderActivation(object sender, EventArgs args)
        {
            var view = View;
            if (view == null)
                return;
            FireValueChanged(view.IntValue);
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var slider = View;
                if (slider != null)
                {
                    slider.Activated -= HandleSliderActivation;
                }
            }
        }
    }
}
