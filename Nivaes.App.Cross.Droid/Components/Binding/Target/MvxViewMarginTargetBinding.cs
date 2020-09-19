// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Content.Res;
using Android.Views;

namespace MvvmCross.Platforms.Android.Binding.Target
{
    using System;
    using MvvmCross.Binding;

    public class MvxViewMarginTargetBinding : MvxAndroidTargetBinding
    {
        private readonly string mWhichMargin;

        public MvxViewMarginTargetBinding(View target, string whichMargin) : base(target)
        {
            mWhichMargin = whichMargin ?? throw new ArgumentNullException(nameof(whichMargin));
        }

        public override Type TargetType => typeof(int);
        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        protected override void SetValueImpl(object target, object value)
        {
            if (target is not View view) return;

            if (view.LayoutParameters is not ViewGroup.MarginLayoutParams layoutParameters) return;

            var dpMargin = (int)value;
            var pxMargin = DpToPx(dpMargin);

            switch (mWhichMargin)
            {
                case MvxAndroidPropertyBinding.View_Margin:
                    layoutParameters.SetMargins(pxMargin, pxMargin, pxMargin, pxMargin);
                    break;
                case MvxAndroidPropertyBinding.View_MarginLeft:
                    layoutParameters.LeftMargin = pxMargin;
                    break;
                case MvxAndroidPropertyBinding.View_MarginRight:
                    layoutParameters.RightMargin = pxMargin;
                    break;
                case MvxAndroidPropertyBinding.View_MarginTop:
                    layoutParameters.TopMargin = pxMargin;
                    break;
                case MvxAndroidPropertyBinding.View_MarginBottom:
                    layoutParameters.BottomMargin = pxMargin;
                    break;
                case MvxAndroidPropertyBinding.View_MarginEnd:
                    layoutParameters.MarginEnd = pxMargin;
                    break;
                case MvxAndroidPropertyBinding.View_MarginStart:
                    layoutParameters.MarginStart = pxMargin;
                    break;
            }
        }

        private int DpToPx(int dp)
            => (int)(dp * Resources.System.DisplayMetrics.Density);
    }
}
