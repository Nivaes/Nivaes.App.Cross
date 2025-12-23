namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.Binding;

    public class MvxRatingBarRatingTargetBinding
    : MvxAndroidTargetBinding
    {
        private CrossAndroidTargetEventSubscription<RatingBar, RatingBar.RatingBarChangeEventArgs>? _subscription;

        protected RatingBar? RatingBar => (RatingBar?)Target;

        public MvxRatingBarRatingTargetBinding(RatingBar target)
            : base(target)
        {
        }

        [RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public override void SubscribeToEvents()
        {
            _subscription = RatingBar?.DroidWeakSubscribe<RatingBar, RatingBar.RatingBarChangeEventArgs>(
                nameof(RatingBar.RatingBarChange),
                RatingBar_RatingBarChange);
        }

        private void RatingBar_RatingBarChange(object? sender, RatingBar.RatingBarChangeEventArgs e)
        {
            if (Target is not RatingBar target)
                return;

            var value = target.Rating;
            FireValueChanged(value);
        }

        protected override void SetValueImpl(object target, object? value)
        {
            var ratingBar = (RatingBar)target;
            if (value != null)
                ratingBar.Rating = (float)value;
        }

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(float);

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _subscription?.Dispose();
                _subscription = null;
            }
            base.Dispose(isDisposing);
        }
    }
}