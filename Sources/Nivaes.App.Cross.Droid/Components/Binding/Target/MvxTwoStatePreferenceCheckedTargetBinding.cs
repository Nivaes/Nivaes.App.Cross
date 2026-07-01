using AndroidX.Preference;

namespace Nivaes.App.Cross.Droid
{

    public class MvxTwoStatePreferenceCheckedTargetBinding(TwoStatePreference preference)
    : MvxPreferenceValueTargetBinding(preference)
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