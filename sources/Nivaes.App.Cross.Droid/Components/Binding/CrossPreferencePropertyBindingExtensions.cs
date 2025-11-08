using AndroidX.Preference;

namespace Nivaes.App.Cross.Droid
{
    public static class CrossPreferencePropertyBindingExtensions
    {
        public static string BindValue(this AndroidX.Preference.Preference preference)
            => CrossPreferencePropertyBinding.Preference_Value;

        public static string BindValue(this ListPreference listPreference)
            => CrossPreferencePropertyBinding.ListPreference_Value;

        public static string BindText(this EditTextPreference editTextPreference)
            => CrossPreferencePropertyBinding.EditTextPreference_Text;

        public static string BindChecked(this TwoStatePreference twoStatePreference)
            => CrossPreferencePropertyBinding.TwoStatePreference_Checked;

        public static string BindClick(this AndroidX.Preference.Preference preference)
            => CrossPreferencePropertyBinding.Preference_Click;
    }
}
