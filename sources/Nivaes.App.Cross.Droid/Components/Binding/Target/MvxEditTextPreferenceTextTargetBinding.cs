using AndroidX.Preference;

namespace Nivaes.App.Cross.Droid
{
    public class MvxEditTextPreferenceTextTargetBinding(EditTextPreference preference)
    : MvxPreferenceValueTargetBinding(preference)
    {
        protected override void SetValueImpl(object target, object? value)
        {
            if (target is EditTextPreference t)
            {
                t.Text = (string?)value;
            }
        }
    }
}