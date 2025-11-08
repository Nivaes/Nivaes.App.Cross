namespace Nivaes.App.Cross.Droid
{
    using AndroidX.Preference;

    public class CrossEditTextPreferenceTextTargetBinding(EditTextPreference preference)
        : CrossPreferenceValueTargetBinding(preference)
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