using System.Reflection;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross;

namespace Nivaes.App.Cross.AppKitLib
{
    public class MvxNSSliderValueTargetBinding : MvxPropertyInfoTargetBinding<NSSlider>
    {
        public MvxNSSliderValueTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var slider = View;
            if (slider == null)
            {
                CrossBindingLogger.GetLogger<MvxNSSliderValueTargetBinding>().LogError($"NSSlider is null in {nameof(MvxNSSliderValueTargetBinding)}");
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
                var slider = View;
                if (slider != null)
                {
                    slider.Activated -= HandleSliderActivation;
                }
            }
        }
    }
}
