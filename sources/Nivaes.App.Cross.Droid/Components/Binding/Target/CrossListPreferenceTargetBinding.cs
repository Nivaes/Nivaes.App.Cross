namespace Nivaes.App.Cross.Droid
{
    using AndroidX.Preference;

    public class CrossListPreferenceTargetBinding(ListPreference preference)
        : CrossPreferenceValueTargetBinding(preference)
    {
        protected override void SetValueImpl(object target, object? value)
        {
            if (target is ListPreference pref)
                pref.Value = (string?)value;
        }
    }
}