namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Android.Views;
    using Android.Webkit;
    using AndroidX.Preference;
    using MvvmCross.IoC;
    using AppCompatSearchView = AndroidX.AppCompat.Widget.SearchView;
    using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

    [Obsolete("No compatible con AoT")]
    public class CrossAndroidBindingBuilder
        : CrossBindingBuilder
    {
        private readonly Action<ICrossValueConverterRegistry> _fillValueConverters;
        private readonly Action<ICrossValueCombinerRegistry> _fillValueCombiners;
        private readonly Action<ICrossTargetBindingFactoryRegistry> _fillTargetFactories;
        private readonly Action<ICrossBindingNameRegistry> _fillBindingNames;
        private readonly Action<IMvxTypeCache> _fillViewTypes;
        private readonly Action<ICrossAxmlNameViewTypeResolver> _fillAxmlViewTypeResolver;
        private readonly Action<ICrossNamespaceListViewTypeResolver> _fillNamespaceListViewTypeResolver;

        public CrossAndroidBindingBuilder(
            Action<ICrossValueConverterRegistry> fillValueConverters,
            Action<ICrossValueCombinerRegistry> fillValueCombiners,
            Action<ICrossTargetBindingFactoryRegistry> fillTargetFactories,
            Action<ICrossBindingNameRegistry> fillBindingNames,
            Action<IMvxTypeCache> fillViewTypes,
            Action<ICrossAxmlNameViewTypeResolver> fillAxmlViewTypeResolver,
            Action<ICrossNamespaceListViewTypeResolver> fillNamespaceListViewTypeResolver)
        {
            _fillValueConverters = fillValueConverters;
            _fillValueCombiners = fillValueCombiners;
            _fillTargetFactories = fillTargetFactories;
            _fillBindingNames = fillBindingNames;
            _fillViewTypes = fillViewTypes;
            _fillAxmlViewTypeResolver = fillAxmlViewTypeResolver;
            _fillNamespaceListViewTypeResolver = fillNamespaceListViewTypeResolver;
        }

        protected override void FillValueConverters(ICrossValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);
            _fillValueConverters?.Invoke(registry);
        }

        protected override void FillValueCombiners(ICrossValueCombinerRegistry registry)
        {
            base.FillValueCombiners(registry);
            _fillValueCombiners?.Invoke(registry);
        }

        [RequiresUnreferencedCode("This method registers source steps that may not be preserved by trimming")]
        public override void DoRegistration(IMvxIoCProvider iocProvider)
        {
            InitializeAppResourceTypeFinder(iocProvider);
            InitializeBindingResources(iocProvider);
            InitializeLayoutInflation(iocProvider);
            base.DoRegistration(iocProvider);
        }

        protected virtual void InitializeLayoutInflation(IMvxIoCProvider iocProvider)
        {
            var inflaterfactoryFactory = CreateLayoutInflaterFactoryFactory();
            iocProvider.RegisterSingleton(inflaterfactoryFactory);

            var viewFactory = CreateAndroidViewFactory();
            iocProvider.RegisterSingleton(viewFactory);

            var viewBinderFactory = CreateAndroidViewBinderFactory();
            iocProvider.RegisterSingleton(viewBinderFactory);
        }

        protected virtual ICrossAndroidViewBinderFactory CreateAndroidViewBinderFactory()
        {
            return new CrossAndroidViewBinderFactory();
        }

        protected virtual ICrossLayoutInflaterHolderFactoryFactory CreateLayoutInflaterFactoryFactory()
        {
            return new CrossLayoutInflaterFactoryFactory();
        }

        protected virtual ICrossAndroidViewFactory CreateAndroidViewFactory()
        {
            return new CrossAndroidViewFactory();
        }

        protected virtual void InitializeBindingResources(IMvxIoCProvider iocProvider)
        {
            var mvxAndroidBindingResource = CreateAndroidBindingResource();
            iocProvider.RegisterSingleton(mvxAndroidBindingResource);
        }

        protected virtual ICrossAndroidBindingResource CreateAndroidBindingResource()
        {
            return new CrossAndroidBindingResource();
        }

        protected virtual void InitializeAppResourceTypeFinder(IMvxIoCProvider provider)
        {
            var resourceFinder = CreateAppResourceTypeFinder();
            provider.RegisterSingleton(resourceFinder);
        }

        protected virtual ICrossAppResourceTypeFinder CreateAppResourceTypeFinder()
        {
            return new CrossAppResourceTypeFinder();
        }

        [RequiresUnreferencedCode("This method registers target bindings that may not be preserved by trimming")]
        protected override void FillTargetFactories(ICrossTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);

            registry.RegisterCustomBindingFactory<View>(
                CrossAndroidPropertyBinding.View_Click,
                view => new CrossViewClickBinding(view));

            registry.RegisterCustomBindingFactory<TextView>(
                CrossAndroidPropertyBinding.TextView_Text,
                textView => new CrossTextViewTextTargetBinding(textView));

            registry.RegisterCustomBindingFactory<TextView>(
                CrossAndroidPropertyBinding.TextView_TextFormatted,
                textView => new CrossTextViewTextFormattedTargetBinding(textView));

            registry.RegisterPropertyInfoBindingFactory(
                typeof(CrossAutoCompleteTextViewPartialTextTargetBinding),
                typeof(CrossAutoCompleteTextView),
                CrossAndroidPropertyBinding.MvxAutoCompleteTextView_PartialText);

            registry.RegisterPropertyInfoBindingFactory(
                typeof(CrossAutoCompleteTextViewSelectedObjectTargetBinding),
                typeof(CrossAutoCompleteTextView),
                CrossAndroidPropertyBinding.MvxAutoCompleteTextView_SelectedObject);

            registry.RegisterPropertyInfoBindingFactory(
                typeof(CrossCompoundButtonCheckedTargetBinding),
                typeof(CompoundButton),
                CrossAndroidPropertyBinding.CompoundButton_Checked);

            registry.RegisterPropertyInfoBindingFactory(
                typeof(CrossSeekBarProgressTargetBinding),
                typeof(SeekBar),
                CrossAndroidPropertyBinding.SeekBar_Progress);

            registry.RegisterPropertyInfoBindingFactory(
                typeof(CrossNumberPickerValueTargetBinding),
                typeof(NumberPicker),
                CrossAndroidPropertyBinding.NumberPicker_Value);

            registry.RegisterCustomBindingFactory<NumberPicker>(
                CrossAndroidPropertyBinding.NumberPicker_DisplayedValues,
                view => new CrossNumberPickerDisplayedValuesTargetBinding(view));

            registry.RegisterCustomBindingFactory<View>(
                CrossAndroidPropertyBinding.View_Visible,
                view => new CrossViewVisibleBinding(view));

            registry.RegisterCustomBindingFactory<View>(
                CrossAndroidPropertyBinding.View_Hidden,
                view => new CrossViewHiddenBinding(view));

            registry.RegisterCustomBindingFactory<ImageView>(
                CrossAndroidPropertyBinding.ImageView_Bitmap,
                imageView => new CrossImageViewBitmapTargetBinding(imageView));

            registry.RegisterCustomBindingFactory<ImageView>(
                CrossAndroidPropertyBinding.ImageView_Drawable,
                imageView => new CrossImageViewImageDrawableTargetBinding(imageView));

            registry.RegisterCustomBindingFactory<ImageView>(
                CrossAndroidPropertyBinding.ImageView_DrawableId,
                imageView => new CrossImageViewDrawableTargetBinding(imageView));

            registry.RegisterCustomBindingFactory<ImageView>(
                CrossAndroidPropertyBinding.ImageView_DrawableName,
                imageView => new CrossImageViewDrawableNameTargetBinding(imageView));

            registry.RegisterCustomBindingFactory<ImageView>(
                CrossAndroidPropertyBinding.ImageView_ResourceName,
                imageView => new CrossImageViewResourceNameTargetBinding(imageView));

            registry.RegisterCustomBindingFactory<ImageView>(
                CrossAndroidPropertyBinding.ImageView_AssetImagePath,
                imageView => new CrossImageViewImageTargetBinding(imageView));

            registry.RegisterCustomBindingFactory<CrossSpinner>(
                CrossAndroidPropertyBinding.MvxSpinner_SelectedItem,
                spinner => new CrossSpinnerSelectedItemBinding(spinner));

            registry.RegisterCustomBindingFactory<AdapterView>(
                CrossAndroidPropertyBinding.AdapterView_SelectedItemPosition,
                adapterView => new CrossAdapterViewSelectedItemPositionTargetBinding(adapterView));

            registry.RegisterCustomBindingFactory<CrossListView>(
                CrossAndroidPropertyBinding.MvxListView_SelectedItem,
                adapterView => new CrossListViewSelectedItemTargetBinding(adapterView));

            registry.RegisterCustomBindingFactory<CrossExpandableListView>(
                CrossAndroidPropertyBinding.MvxExpandableListView_SelectedItem,
                adapterView => new CrossExpandableListViewSelectedItemTargetBinding(adapterView));

            registry.RegisterCustomBindingFactory<RatingBar>(
                CrossAndroidPropertyBinding.RatingBar_Rating,
                ratingBar => new CrossRatingBarRatingTargetBinding(ratingBar));

            registry.RegisterCustomBindingFactory<View>(
                CrossAndroidPropertyBinding.View_LongClick,
                view => new MvxViewLongClickBinding(view));

            registry.RegisterCustomBindingFactory<CrossRadioGroup>(
                CrossAndroidPropertyBinding.MvxRadioGroup_SelectedItem,
                radioGroup => new CrossRadioGroupSelectedItemBinding(radioGroup));

            registry.RegisterCustomBindingFactory<EditText>(
                CrossAndroidPropertyBinding.EditText_TextFocus,
                editText => new CrossTextViewFocusTargetBinding(editText));

            registry.RegisterCustomBindingFactory<SearchView>(
                CrossAndroidPropertyBinding.SearchView_Query,
                search => new CrossSearchViewQueryTextTargetBinding(search));

            registry.RegisterCustomBindingFactory<Preference>(
                CrossAndroidPropertyBinding.Preference_Value,
                preference => new CrossPreferenceValueTargetBinding(preference));

            registry.RegisterCustomBindingFactory<EditTextPreference>(
                CrossAndroidPropertyBinding.EditTextPreference_Text,
                preference => new CrossEditTextPreferenceTextTargetBinding(preference));

            registry.RegisterCustomBindingFactory<ListPreference>(
                CrossAndroidPropertyBinding.ListPreference_Value,
                preference => new CrossListPreferenceTargetBinding(preference));

            registry.RegisterCustomBindingFactory<TwoStatePreference>(
                CrossAndroidPropertyBinding.TwoStatePreference_Checked,
                preference => new CrossTwoStatePreferenceCheckedTargetBinding(preference));

            var allMargins = new[]
            {
                CrossAndroidPropertyBinding.View_Margin,
                CrossAndroidPropertyBinding.View_MarginLeft,
                CrossAndroidPropertyBinding.View_MarginRight,
                CrossAndroidPropertyBinding.View_MarginTop,
                CrossAndroidPropertyBinding.View_MarginBottom,
                CrossAndroidPropertyBinding.View_MarginStart,
                CrossAndroidPropertyBinding.View_MarginEnd
            };

            foreach (var margin in allMargins)
            {
                registry.RegisterCustomBindingFactory<View>(
                    margin, view => new CrossViewMarginTargetBinding(view, margin));
            }

            registry.RegisterCustomBindingFactory<View>(
                CrossAndroidPropertyBinding.View_Focus,
                view => new CrossViewFocusChangedTargetBinding(view));

            registry.RegisterCustomBindingFactory<VideoView>(
                CrossAndroidPropertyBinding.VideoView_Uri,
                view => new CrossVideoViewUriTargetBinding(view));

            registry.RegisterCustomBindingFactory<WebView>(
                CrossAndroidPropertyBinding.WebView_Uri,
                view => new CrossWebViewUriTargetBinding(view));

            registry.RegisterCustomBindingFactory<WebView>(
                CrossAndroidPropertyBinding.WebView_Html,
                view => new CrossWebViewHtmlTargetBinding(view));

            registry.RegisterPropertyInfoBindingFactory(
                typeof(CrossAppCompatAutoCompleteTextViewPartialTextTargetBinding),
                typeof(CrossAppCompatAutoCompleteTextView),
                CrossAndroidPropertyBinding.MvxAppCompatAutoCompleteTextView_PartialText);

            registry.RegisterPropertyInfoBindingFactory(
                typeof(CrossAppCompatAutoCompleteTextViewSelectedObjectTargetBinding),
                typeof(CrossAppCompatAutoCompleteTextView),
                CrossAndroidPropertyBinding.MvxAppCompatAutoCompleteTextView_SelectedObject);

            registry.RegisterCustomBindingFactory<CrossAppCompatSpinner>(
                CrossAndroidPropertyBinding.MvxAppCompatSpinner_SelectedItem,
                spinner => new CrossAppCompatSpinnerSelectedItemBinding(spinner));

            registry.RegisterCustomBindingFactory<MvxAppCompatRadioGroup>(
                CrossAndroidPropertyBinding.MvxAppCompatRadioGroup_SelectedItem,
                radioGroup => new CrossAppCompatRadioGroupSelectedItemBinding(radioGroup));

            registry.RegisterCustomBindingFactory<Toolbar>(
                CrossAndroidPropertyBinding.Toolbar_Subtitle,
                toolbar => new CrossToolbarSubtitleBinding(toolbar));

            registry.RegisterCustomBindingFactory<AppCompatSearchView>(
                CrossAndroidPropertyBinding.SearchView_Query,
                searchView => new CrossAppCompatSearchViewQueryTextTargetBinding(searchView));

            _fillTargetFactories?.Invoke(registry);
        }

        protected override void FillDefaultBindingNames(ICrossBindingNameRegistry registry)
        {
            base.FillDefaultBindingNames(registry);

            registry.AddOrOverwrite(typeof(Button), CrossAndroidPropertyBinding.View_Click);
            registry.AddOrOverwrite(typeof(CheckBox), CrossAndroidPropertyBinding.CompoundButton_Checked);
            registry.AddOrOverwrite(typeof(TextView), CrossAndroidPropertyBinding.TextView_Text);
            registry.AddOrOverwrite(typeof(CrossListView), nameof(CrossListView.ItemsSource));
            registry.AddOrOverwrite(typeof(CrossLinearLayout), nameof(CrossLinearLayout.ItemsSource));
            registry.AddOrOverwrite(typeof(CrossGridView), nameof(CrossGridView.ItemsSource));
            registry.AddOrOverwrite(typeof(CrossFrameControl), nameof(CrossFrameControl.DataContext));
            registry.AddOrOverwrite(typeof(CrossDatePicker), nameof(CrossDatePicker.Value));
            registry.AddOrOverwrite(typeof(CrossTimePicker), nameof(CrossTimePicker.Value));
            registry.AddOrOverwrite(typeof(CompoundButton), CrossAndroidPropertyBinding.CompoundButton_Checked);
            registry.AddOrOverwrite(typeof(SeekBar), CrossAndroidPropertyBinding.SeekBar_Progress);
            registry.AddOrOverwrite(typeof(SearchView), CrossAndroidPropertyBinding.SearchView_Query);
            registry.AddOrOverwrite(typeof(AppCompatSearchView), CrossAndroidPropertyBinding.SearchView_Query);
            registry.AddOrOverwrite(typeof(NumberPicker), CrossAndroidPropertyBinding.NumberPicker_Value);
            registry.AddOrOverwrite(typeof(NumberPicker), CrossAndroidPropertyBinding.NumberPicker_DisplayedValues);
            registry.AddOrOverwrite(typeof(VideoView), CrossAndroidPropertyBinding.VideoView_Uri);
            registry.AddOrOverwrite(typeof(WebView), CrossAndroidPropertyBinding.WebView_Uri);

            _fillBindingNames?.Invoke(registry);
        }

        protected override void RegisterPlatformSpecificComponents(IMvxIoCProvider iocProvider)
        {
            base.RegisterPlatformSpecificComponents(iocProvider);

            InitializeViewTypeResolver(iocProvider);
            InitializeContextStack(iocProvider);
        }

        protected virtual void InitializeContextStack(IMvxIoCProvider iocProvider)
        {
            var stack = CreateContextStack();
            iocProvider.RegisterSingleton(stack);
        }

        protected virtual ICrossBindingContextStack<ICrossAndroidBindingContext> CreateContextStack()
        {
            return new CrossAndroidBindingContextStack();
        }

        protected virtual void InitializeViewTypeResolver(IMvxIoCProvider iocProvider)
        {
            var typeCache = CreateViewTypeCache();
            iocProvider.RegisterSingleton(typeCache);

            var fullNameViewTypeResolver = new CrossAxmlNameViewTypeResolver(typeCache);
            iocProvider.RegisterSingleton<ICrossAxmlNameViewTypeResolver>(fullNameViewTypeResolver);
            var listViewTypeResolver = new CrossNamespaceListViewTypeResolver(typeCache);
            iocProvider.RegisterSingleton<ICrossNamespaceListViewTypeResolver>(listViewTypeResolver);
            var justNameTypeResolver = new CrossJustNameViewTypeResolver(typeCache);

            var composite = new CrossCompositeViewTypeResolver(fullNameViewTypeResolver, listViewTypeResolver, justNameTypeResolver);
            var cached = new CrossCachedViewTypeResolver(composite);
            iocProvider.RegisterSingleton<ICrossViewTypeResolver>(cached);

            _fillViewTypes?.Invoke(typeCache);
            _fillAxmlViewTypeResolver?.Invoke(fullNameViewTypeResolver);
            _fillNamespaceListViewTypeResolver?.Invoke(listViewTypeResolver);
        }

        protected virtual IMvxTypeCache CreateViewTypeCache()
        {
            throw new NotImplementedException();
            //return new MvxTypeCache<View>();
        }
    }
}
