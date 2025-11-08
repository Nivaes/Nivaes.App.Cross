namespace Nivaes.App.Cross.Droid
{
    using AndroidX.Preference;

    public class CrossTwoStatePreferenceCheckedTargetBinding(TwoStatePreference preference)
        : CrossPreferenceValueTargetBinding(preference)
    {
        protected override void SetValueImpl(object target, object? value)
        {
            if (target is TwoStatePreference t && value != null)
            {
                t.Checked = (bool)value;
            }
        }
    }
}