using AndroidX.Preference;

namespace Nivaes.App.Cross.Droid
{

    public class MvxListPreferenceTargetBinding(ListPreference preference)
    : MvxPreferenceValueTargetBinding(preference)
    {
        protected override void SetValueImpl(object target, object? value)
        {
            if (target is ListPreference pref)
                pref.Value = (string?)value;
        }
    }
}