namespace Nivaes.App.Cross.Droid
{
    using Android.Preferences;
    using Android.Views;
    using Android.Webkit;

    public static class CrossAndroidPropertyBindingExtensions
    {
        public static string BindClick(this View view)
            => CrossAndroidPropertyBinding.View_Click;

        public static string BindText(this TextView textview)
            => CrossAndroidPropertyBinding.TextView_Text;

        public static string BindTextFormatted(this TextView textview)
            => CrossAndroidPropertyBinding.TextView_TextFormatted;

        public static string BindPartialText(this CrossAutoCompleteTextView mvxAutoCompleteTextView)
            => CrossAndroidPropertyBinding.MvxAutoCompleteTextView_PartialText;

        public static string BindSelectedObject(this CrossAutoCompleteTextView mvxAutoCompleteTextView)
            => CrossAndroidPropertyBinding.MvxAutoCompleteTextView_SelectedObject;

        public static string BindChecked(this CompoundButton compoundButton)
            => CrossAndroidPropertyBinding.CompoundButton_Checked;

        public static string BindProgress(this SeekBar seekBar)
            => CrossAndroidPropertyBinding.SeekBar_Progress;

        public static string BindVisible(this View view)
            => CrossAndroidPropertyBinding.View_Visible;

        public static string BindHidden(this View view)
            => CrossAndroidPropertyBinding.View_Hidden;

        public static string BindBitmap(this ImageView imageView)
            => CrossAndroidPropertyBinding.ImageView_Bitmap;

        public static string BindDrawable(this ImageView imageView)
            => CrossAndroidPropertyBinding.ImageView_Drawable;

        public static string BindDrawableId(this ImageView imageView)
            => CrossAndroidPropertyBinding.ImageView_DrawableId;

        public static string BindDrawableName(this ImageView imageView)
            => CrossAndroidPropertyBinding.ImageView_DrawableName;

        public static string BindResourceName(this ImageView imageView)
            => CrossAndroidPropertyBinding.ImageView_ResourceName;

        public static string BindAssetImagePath(this ImageView imageView)
            => CrossAndroidPropertyBinding.ImageView_AssetImagePath;

        public static string BindSelectedItem(this CrossSpinner mvxSpinner)
            => CrossAndroidPropertyBinding.MvxSpinner_SelectedItem;

        public static string BindSelectedItemPosition(this AdapterView adapterView)
            => CrossAndroidPropertyBinding.AdapterView_SelectedItemPosition;

        public static string BindSelectedItem(this CrossListView mvxListView)
            => CrossAndroidPropertyBinding.MvxListView_SelectedItem;

        public static string BindSelectedItem(this CrossExpandableListView mvxExpandableListView)
            => CrossAndroidPropertyBinding.MvxExpandableListView_SelectedItem;

        public static string BindRating(this RatingBar ratingBar)
            => CrossAndroidPropertyBinding.RatingBar_Rating;

        public static string BindLongClick(this View view)
            => CrossAndroidPropertyBinding.View_LongClick;

        public static string BindSelectedItem(this CrossRadioGroup mvxRadioGroup)
            => CrossAndroidPropertyBinding.MvxRadioGroup_SelectedItem;

        public static string BindTextFocus(this EditText editText)
            => CrossAndroidPropertyBinding.EditText_TextFocus;

        public static string BindQuery(this SearchView searchView)
            => CrossAndroidPropertyBinding.SearchView_Query;

        public static string BindValue(this Preference preference)
            => CrossAndroidPropertyBinding.Preference_Value;

        public static string BindText(this EditTextPreference editTextPreference)
            => CrossAndroidPropertyBinding.EditTextPreference_Text;

        public static string BindValue(this ListPreference listPreference)
            => CrossAndroidPropertyBinding.ListPreference_Value;

        public static string BindChecked(this TwoStatePreference twoStatePreference)
            => CrossAndroidPropertyBinding.TwoStatePreference_Checked;

        public static string BindDisplayedValues(this NumberPicker numberPicker)
            => CrossAndroidPropertyBinding.NumberPicker_DisplayedValues;

        public static string BindValue(this NumberPicker numberPicker)
            => CrossAndroidPropertyBinding.NumberPicker_Value;

        public static string BindMargin(this View view)
            => CrossAndroidPropertyBinding.View_Margin;

        public static string BindMarginLeft(this View view)
            => CrossAndroidPropertyBinding.View_MarginLeft;

        public static string BindMarginRight(this View view)
            => CrossAndroidPropertyBinding.View_MarginRight;

        public static string BindMarginTop(this View view)
            => CrossAndroidPropertyBinding.View_MarginTop;

        public static string BindMarginBottom(this View view)
            => CrossAndroidPropertyBinding.View_MarginBottom;

        public static string BindMarginStart(this View view)
            => CrossAndroidPropertyBinding.View_MarginStart;

        public static string BindMarginEnd(this View view)
            => CrossAndroidPropertyBinding.View_MarginEnd;

        public static string BindFocus(this View view)
            => CrossAndroidPropertyBinding.View_Focus;

        public static string BindVideoUri(this VideoView view)
            => CrossAndroidPropertyBinding.VideoView_Uri;

        public static string BindWebViewUri(this WebView view)
            => CrossAndroidPropertyBinding.WebView_Uri;

        public static string BindWebViewHtml(this WebView view)
            => CrossAndroidPropertyBinding.WebView_Html;
    }
}
