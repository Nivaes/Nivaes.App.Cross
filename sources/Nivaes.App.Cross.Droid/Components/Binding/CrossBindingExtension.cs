using Android.Views;
using Android.Webkit;
using AndroidX.Preference;
using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Platforms.Android.Binding.Target;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Plugin.Color.Platforms.Android.Binding;
using AppCompatSearchView = AndroidX.AppCompat.Widget.SearchView;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace Nivaes.App.Cross.Droid
{
    public static class CrossBindingExtension
    {
        extension(IServiceProvider service)
        {
            public IServiceProvider TargetBindingFactoryRegistry()
            {
                // Registrar con roslyn.

                var registry = service.GetRequiredService<ICrossTargetBindingFactoryRegistry>();

                registry.RegisterCustomBindingFactory<View>(
                    MvxAndroidPropertyBinding.View_Click,
                    view => new MvxViewClickBinding(view));

                registry.RegisterCustomBindingFactory<TextView>(
                    MvxAndroidPropertyBinding.TextView_Text,
                    textView => new MvxTextViewTextTargetBinding(textView));

                registry.RegisterCustomBindingFactory<TextView>(
                    MvxAndroidPropertyBinding.TextView_TextFormatted,
                    textView => new MvxTextViewTextFormattedTargetBinding(textView));

                registry.RegisterPropertyInfoBindingFactory(
                    typeof(MvxAutoCompleteTextViewPartialTextTargetBinding),
                    typeof(MvxAutoCompleteTextView),
                    MvxAndroidPropertyBinding.MvxAutoCompleteTextView_PartialText);

                registry.RegisterPropertyInfoBindingFactory(
                    typeof(MvxAutoCompleteTextViewSelectedObjectTargetBinding),
                    typeof(MvxAutoCompleteTextView),
                    MvxAndroidPropertyBinding.MvxAutoCompleteTextView_SelectedObject);

                registry.RegisterPropertyInfoBindingFactory(
                    typeof(MvxCompoundButtonCheckedTargetBinding),
                    typeof(CompoundButton),
                    MvxAndroidPropertyBinding.CompoundButton_Checked);

                registry.RegisterPropertyInfoBindingFactory(
                    typeof(MvxSeekBarProgressTargetBinding),
                    typeof(SeekBar),
                    MvxAndroidPropertyBinding.SeekBar_Progress);

                registry.RegisterPropertyInfoBindingFactory(
                    typeof(CrossNumberPickerValueTargetBinding),
                    typeof(NumberPicker),
                    MvxAndroidPropertyBinding.NumberPicker_Value);

                registry.RegisterCustomBindingFactory<NumberPicker>(
                    MvxAndroidPropertyBinding.NumberPicker_DisplayedValues,
                    view => new MvxNumberPickerDisplayedValuesTargetBinding(view));

                registry.RegisterCustomBindingFactory<View>(
                    MvxAndroidPropertyBinding.View_Visible,
                    view => new MvxViewVisibleBinding(view));

                registry.RegisterCustomBindingFactory<View>(
                    MvxAndroidPropertyBinding.View_Hidden,
                    view => new MvxViewHiddenBinding(view));

                registry.RegisterCustomBindingFactory<ImageView>(
                    MvxAndroidPropertyBinding.ImageView_Bitmap,
                    imageView => new MvxImageViewBitmapTargetBinding(imageView));

                registry.RegisterCustomBindingFactory<ImageView>(
                    MvxAndroidPropertyBinding.ImageView_Drawable,
                    imageView => new MvxImageViewImageDrawableTargetBinding(imageView));

                registry.RegisterCustomBindingFactory<ImageView>(
                    MvxAndroidPropertyBinding.ImageView_DrawableId,
                    imageView => new MvxImageViewDrawableTargetBinding(imageView));

                registry.RegisterCustomBindingFactory<ImageView>(
                    MvxAndroidPropertyBinding.ImageView_DrawableName,
                    imageView => new MvxImageViewDrawableNameTargetBinding(imageView));

                registry.RegisterCustomBindingFactory<ImageView>(
                    MvxAndroidPropertyBinding.ImageView_ResourceName,
                    imageView => new MvxImageViewResourceNameTargetBinding(imageView));

                registry.RegisterCustomBindingFactory<ImageView>(
                    MvxAndroidPropertyBinding.ImageView_AssetImagePath,
                    imageView => new MvxImageViewImageTargetBinding(imageView));

                registry.RegisterCustomBindingFactory<MvxSpinner>(
                    MvxAndroidPropertyBinding.MvxSpinner_SelectedItem,
                    spinner => new MvxSpinnerSelectedItemBinding(spinner));

                registry.RegisterCustomBindingFactory<AdapterView>(
                    MvxAndroidPropertyBinding.AdapterView_SelectedItemPosition,
                    adapterView => new MvxAdapterViewSelectedItemPositionTargetBinding(adapterView));

                registry.RegisterCustomBindingFactory<MvxListView>(
                    MvxAndroidPropertyBinding.MvxListView_SelectedItem,
                    adapterView => new MvxListViewSelectedItemTargetBinding(adapterView));

                registry.RegisterCustomBindingFactory<MvxExpandableListView>(
                    MvxAndroidPropertyBinding.MvxExpandableListView_SelectedItem,
                    adapterView => new MvxExpandableListViewSelectedItemTargetBinding(adapterView));

                registry.RegisterCustomBindingFactory<RatingBar>(
                    MvxAndroidPropertyBinding.RatingBar_Rating,
                    ratingBar => new MvxRatingBarRatingTargetBinding(ratingBar));

                registry.RegisterCustomBindingFactory<View>(
                    MvxAndroidPropertyBinding.View_LongClick,
                    view => new MvxViewLongClickBinding(view));

                registry.RegisterCustomBindingFactory<MvxRadioGroup>(
                    MvxAndroidPropertyBinding.MvxRadioGroup_SelectedItem,
                    radioGroup => new MvxRadioGroupSelectedItemBinding(radioGroup));

                registry.RegisterCustomBindingFactory<EditText>(
                    MvxAndroidPropertyBinding.EditText_TextFocus,
                    editText => new MvxTextViewFocusTargetBinding(editText));

                registry.RegisterCustomBindingFactory<SearchView>(
                    MvxAndroidPropertyBinding.SearchView_Query,
                    search => new MvxSearchViewQueryTextTargetBinding(search));

                registry.RegisterCustomBindingFactory<Preference>(
                    MvxAndroidPropertyBinding.Preference_Value,
                    preference => new MvxPreferenceValueTargetBinding(preference));

                registry.RegisterCustomBindingFactory<EditTextPreference>(
                    MvxAndroidPropertyBinding.EditTextPreference_Text,
                    preference => new MvxEditTextPreferenceTextTargetBinding(preference));

                registry.RegisterCustomBindingFactory<ListPreference>(
                    MvxAndroidPropertyBinding.ListPreference_Value,
                    preference => new MvxListPreferenceTargetBinding(preference));

                registry.RegisterCustomBindingFactory<TwoStatePreference>(
                    MvxAndroidPropertyBinding.TwoStatePreference_Checked,
                    preference => new MvxTwoStatePreferenceCheckedTargetBinding(preference));

                var allMargins = new[]
                {
                    MvxAndroidPropertyBinding.View_Margin,
                    MvxAndroidPropertyBinding.View_MarginLeft,
                    MvxAndroidPropertyBinding.View_MarginRight,
                    MvxAndroidPropertyBinding.View_MarginTop,
                    MvxAndroidPropertyBinding.View_MarginBottom,
                    MvxAndroidPropertyBinding.View_MarginStart,
                    MvxAndroidPropertyBinding.View_MarginEnd
                };

                foreach (var margin in allMargins)
                {
                    registry.RegisterCustomBindingFactory<View>(
                        margin, view => new MvxViewMarginTargetBinding(view, margin));
                }

                registry.RegisterCustomBindingFactory<View>(
                    MvxAndroidPropertyBinding.View_Focus,
                    view => new MvxViewFocusChangedTargetBinding(view));

                registry.RegisterCustomBindingFactory<VideoView>(
                    MvxAndroidPropertyBinding.VideoView_Uri,
                    view => new MvxVideoViewUriTargetBinding(view));

                registry.RegisterCustomBindingFactory<WebView>(
                    MvxAndroidPropertyBinding.WebView_Uri,
                    view => new MvxWebViewUriTargetBinding(view));

                registry.RegisterCustomBindingFactory<WebView>(
                    MvxAndroidPropertyBinding.WebView_Html,
                    view => new MvxWebViewHtmlTargetBinding(view));

                registry.RegisterPropertyInfoBindingFactory(
                    typeof(MvxAppCompatAutoCompleteTextViewPartialTextTargetBinding),
                    typeof(MvxAppCompatAutoCompleteTextView),
                    MvxAndroidPropertyBinding.MvxAppCompatAutoCompleteTextView_PartialText);

                registry.RegisterPropertyInfoBindingFactory(
                    typeof(MvxAppCompatAutoCompleteTextViewSelectedObjectTargetBinding),
                    typeof(MvxAppCompatAutoCompleteTextView),
                    MvxAndroidPropertyBinding.MvxAppCompatAutoCompleteTextView_SelectedObject);

                registry.RegisterCustomBindingFactory<MvxAppCompatSpinner>(
                    MvxAndroidPropertyBinding.MvxAppCompatSpinner_SelectedItem,
                    spinner => new MvxAppCompatSpinnerSelectedItemBinding(spinner));

                registry.RegisterCustomBindingFactory<MvxAppCompatRadioGroup>(
                    MvxAndroidPropertyBinding.MvxAppCompatRadioGroup_SelectedItem,
                    radioGroup => new MvxAppCompatRadioGroupSelectedItemBinding(radioGroup));

                registry.RegisterCustomBindingFactory<Toolbar>(
                    MvxAndroidPropertyBinding.Toolbar_Subtitle,
                    toolbar => new MvxToolbarSubtitleBinding(toolbar));

                registry.RegisterCustomBindingFactory<AppCompatSearchView>(
                    MvxAndroidPropertyBinding.SearchView_Query,
                    searchView => new MvxAppCompatSearchViewQueryTextTargetBinding(searchView));

                registry.RegisterFactory(new CrossCustomBindingFactory<View>(
                    MvxAndroidColorPropertyBinding.View_BackgroundColor,
                    view => new MvxViewBackgroundColorBinding(view)));

                registry.RegisterFactory(new CrossCustomBindingFactory<TextView>(
                    MvxAndroidColorPropertyBinding.TextView_TextColor,
                    textView => new MvxTextViewTextColorBinding(textView)));

                return service;
            }

            public IServiceProvider BindingNameRegister()
            {
                var registry = service.GetRequiredService<ICrossBindingNameRegistry>();

                registry.AddOrOverwrite(typeof(Button), MvxAndroidPropertyBinding.View_Click);
                registry.AddOrOverwrite(typeof(CheckBox), MvxAndroidPropertyBinding.CompoundButton_Checked);
                registry.AddOrOverwrite(typeof(TextView), MvxAndroidPropertyBinding.TextView_Text);
                registry.AddOrOverwrite(typeof(MvxListView), nameof(MvxListView.ItemsSource));
                registry.AddOrOverwrite(typeof(MvxLinearLayout), nameof(MvxLinearLayout.ItemsSource));
                registry.AddOrOverwrite(typeof(MvxGridView), nameof(MvxGridView.ItemsSource));
                registry.AddOrOverwrite(typeof(MvxFrameControl), nameof(MvxFrameControl.DataContext));
                registry.AddOrOverwrite(typeof(MvxDatePicker), nameof(MvxDatePicker.Value));
                registry.AddOrOverwrite(typeof(MvxTimePicker), nameof(MvxTimePicker.Value));
                registry.AddOrOverwrite(typeof(CompoundButton), MvxAndroidPropertyBinding.CompoundButton_Checked);
                registry.AddOrOverwrite(typeof(SeekBar), MvxAndroidPropertyBinding.SeekBar_Progress);
                registry.AddOrOverwrite(typeof(SearchView), MvxAndroidPropertyBinding.SearchView_Query);
                registry.AddOrOverwrite(typeof(AppCompatSearchView), MvxAndroidPropertyBinding.SearchView_Query);
                registry.AddOrOverwrite(typeof(NumberPicker), MvxAndroidPropertyBinding.NumberPicker_Value);
                registry.AddOrOverwrite(typeof(NumberPicker), MvxAndroidPropertyBinding.NumberPicker_DisplayedValues);
                registry.AddOrOverwrite(typeof(VideoView), MvxAndroidPropertyBinding.VideoView_Uri);
                registry.AddOrOverwrite(typeof(WebView), MvxAndroidPropertyBinding.WebView_Uri);

                return service;
            }
        }
    }
}
