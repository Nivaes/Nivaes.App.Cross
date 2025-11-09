namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using AndroidX.Preference;

    public static class CrossPreferenceSetupHelper
    {
        [RequiresUnreferencedCode("This method may use types that are not preserved by trimming")]
        public static void FillTargetFactories(ICrossTargetBindingFactoryRegistry registry)
        {
            registry.RegisterCustomBindingFactory<Preference>(
                CrossPreferencePropertyBinding.Preference_Value,
                preference => new CrossPreferenceValueTargetBinding(preference));

            registry.RegisterCustomBindingFactory<EditTextPreference>(
                CrossPreferencePropertyBinding.EditTextPreference_Text,
                preference => new CrossEditTextPreferenceTextTargetBinding(preference));

            registry.RegisterCustomBindingFactory<ListPreference>(
                CrossPreferencePropertyBinding.ListPreference_Value,
                preference => new CrossListPreferenceTargetBinding(preference));

            registry.RegisterCustomBindingFactory<TwoStatePreference>(
                CrossPreferencePropertyBinding.TwoStatePreference_Checked,
                preference => new CrossTwoStatePreferenceCheckedTargetBinding(preference));

            registry.RegisterCustomBindingFactory<Preference>(
                CrossPreferencePropertyBinding.Preference_Click,
                preference => new CrossPreferenceClickTargetBinding(preference));
        }
    }
}
