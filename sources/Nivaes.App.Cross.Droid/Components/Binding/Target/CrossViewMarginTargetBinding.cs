namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Android.Content.Res;
    using Android.Views;

    public class CrossViewMarginTargetBinding : CrossAndroidTargetBinding
    {
        private readonly string _whichMargin;

        public CrossViewMarginTargetBinding(View target, string whichMargin) : base(target)
        {
            ArgumentException.ThrowIfNullOrEmpty(whichMargin);
            _whichMargin = whichMargin;
        }

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(float);
        public override CrossBindingMode DefaultMode => CrossBindingMode.OneWay;

        protected override void SetValueImpl(object target, object? value)
        {
            if (target is not View view || value == null)
                return;

            if (view.LayoutParameters is not ViewGroup.MarginLayoutParams layoutParameters)
                return;

            var dpMargin = (float)value;
            var pxMargin = DpToPx(dpMargin);

            switch (_whichMargin)
            {
                case CrossAndroidPropertyBinding.View_Margin:
                    layoutParameters.SetMargins(pxMargin, pxMargin, pxMargin, pxMargin);
                    break;
                case CrossAndroidPropertyBinding.View_MarginLeft:
                    layoutParameters.LeftMargin = pxMargin;
                    break;
                case CrossAndroidPropertyBinding.View_MarginRight:
                    layoutParameters.RightMargin = pxMargin;
                    break;
                case CrossAndroidPropertyBinding.View_MarginTop:
                    layoutParameters.TopMargin = pxMargin;
                    break;
                case CrossAndroidPropertyBinding.View_MarginBottom:
                    layoutParameters.BottomMargin = pxMargin;
                    break;
                case CrossAndroidPropertyBinding.View_MarginEnd:
                    layoutParameters.MarginEnd = pxMargin;
                    break;
                case CrossAndroidPropertyBinding.View_MarginStart:
                    layoutParameters.MarginStart = pxMargin;
                    break;
            }

            view.LayoutParameters = layoutParameters;
        }

        private static int DpToPx(float dp)
            => (int)(dp * Resources.System?.DisplayMetrics?.Density ?? 1.6f);
    }
}