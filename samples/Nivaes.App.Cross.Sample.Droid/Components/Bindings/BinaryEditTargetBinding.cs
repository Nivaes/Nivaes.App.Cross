namespace Playground.Droid.Bindings
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.Droid;
    using Playground.Droid.Controls;

    [RequiresUnreferencedCode("MvxBindings requires unreferenced code")]
    public class BinaryEditTargetBinding 
        : MvxAndroidTargetBinding<BinaryEdit, int>
    {
        public BinaryEditTargetBinding(BinaryEdit target) : base(target)
        {
        }

        public override void SubscribeToEvents()
        {
            Target.MyCountChanged += TargetOnMyCountChanged;
        }

        private void TargetOnMyCountChanged(object sender, EventArgs eventArgs)
        {
            var target = Target;

            if (target == null)
                return;

            var value = target.GetCount();
            FireValueChanged(value);
        }

        protected override void SetValueImpl(BinaryEdit target, int value)
        {
            var binaryEdit = target;
            binaryEdit.SetThat(value);
        }

        public override CrossBindingMode DefaultMode => CrossBindingMode.TwoWay;

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var target = Target;
                if (target != null)
                {
                    target.MyCountChanged -= TargetOnMyCountChanged;
                }
            }
            base.Dispose(isDisposing);
        }
    }
}
