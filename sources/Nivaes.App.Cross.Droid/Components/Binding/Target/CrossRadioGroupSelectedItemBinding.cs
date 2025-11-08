namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Android.Views;

    public class CrossRadioGroupSelectedItemBinding
        : CrossAndroidTargetBinding
    {
        private object? _currentValue;
        private CrossAndroidTargetEventSubscription<RadioGroup, RadioGroup.CheckedChangeEventArgs>? _subscription;

        public CrossRadioGroupSelectedItemBinding(CrossRadioGroup radioGroup)
            : base(radioGroup)
        {
            _subscription = radioGroup.WeakSubscribe<RadioGroup, RadioGroup.CheckedChangeEventArgs>(
                nameof(RadioGroup.CheckedChange),
                RadioGroupCheckedChanged);
        }

        private bool CheckValueChanged(object? newValue)
        {
            bool changed;
            if (newValue == null)
            {
                changed = _currentValue != null;
            }
            else
            {
                changed = !newValue.Equals(_currentValue);
            }
            return changed;
        }

        private void RadioGroupCheckedChanged(object? sender, RadioGroup.CheckedChangeEventArgs args)
        {
            var radioGroup = (CrossRadioGroup?)Target;
            if (radioGroup == null)
                return;

            object? newValue = null;

            var r = radioGroup.FindViewById<RadioButton>(args.CheckedId);
            if (r != null)
            {
                var index = radioGroup.IndexOfChild(r);
                newValue = radioGroup.Adapter.GetRawItem(index);
            }

            bool changed = CheckValueChanged(newValue);
            if (!changed)
                return;

            _currentValue = newValue;
            FireValueChanged(newValue);
        }

        protected override void SetValueImpl(object target, object? value)
        {
            var radioGroup = (CrossRadioGroup?)target;
            if (radioGroup == null)
                return;

            bool changed = CheckValueChanged(value);
            if (!changed)
                return;

            // find the radio button associated with the new value
            var checkid = FindRadioButtonId(value, radioGroup);
            if (checkid == View.NoId)
            {
                radioGroup.ClearCheck();
            }
            else
            {
                radioGroup.Check(checkid);
            }
        }

        private static int FindRadioButtonId(object? value, CrossRadioGroup radioGroup)
        {
            int checkid = View.NoId;

            if (value != null)
            {
                for (int i = 0; i < radioGroup.ChildCount; i++)
                {
                    var li = radioGroup.GetChildAt(i);
                    var data = radioGroup.Adapter.GetRawItem(i);
                    if (!value.Equals(data) || li is not RadioButton radioButton)
                        continue;

                    checkid = radioButton.Id;
                    break;
                }
            }

            return checkid;
        }

        public override CrossBindingMode DefaultMode => CrossBindingMode.TwoWay;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(object);

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