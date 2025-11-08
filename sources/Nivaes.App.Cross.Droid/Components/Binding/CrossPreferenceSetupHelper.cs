using System.Diagnostics.CodeAnalysis;
using AndroidX.Preference;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Platforms.Android.Binding.Target;

namespace Nivaes.App.Cross.Droid
{
    public static class CrossPreferenceSetupHelper
    {
        [RequiresUnreferencedCode("This method may use types that are not preserved by trimming")]
        public static void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterCustomBindingFactory<Preference>(
                CrossPreferencePropertyBinding.Preference_Value,
                preference => new MvxPreferenceValueTargetBinding(preference));

            registry.RegisterCustomBindingFactory<EditTextPreference>(
                CrossPreferencePropertyBinding.EditTextPreference_Text,
                preference => new MvxEditTextPreferenceTextTargetBinding(preference));

            registry.RegisterCustomBindingFactory<ListPreference>(
                CrossPreferencePropertyBinding.ListPreference_Value,
                preference => new MvxListPreferenceTargetBinding(preference));

            registry.RegisterCustomBindingFactory<TwoStatePreference>(
                CrossPreferencePropertyBinding.TwoStatePreference_Checked,
                preference => new MvxTwoStatePreferenceCheckedTargetBinding(preference));

            registry.RegisterCustomBindingFactory<Preference>(
                CrossPreferencePropertyBinding.Preference_Click,
                preference => new MvxPreferenceClickTargetBinding(preference));
        }
    }
}
