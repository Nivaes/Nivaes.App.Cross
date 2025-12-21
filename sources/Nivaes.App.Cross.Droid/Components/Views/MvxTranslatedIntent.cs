namespace MvvmCross.Platforms.Android.Views
{
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class MvxTranslatedIntent
    {
        #region TranslationResult enum

        public enum TranslationResult
        {
            Request,
            ExistingViewModel
        }

        #endregion TranslationResult enum

        public MvxTranslatedIntent(CrossViewModelRequest viewModelRequest)
        {
            ViewModelRequest = viewModelRequest;
            Result = TranslationResult.Request;
        }

        public MvxTranslatedIntent(ICrossViewModel existingViewModel)
        {
            ExistingViewModel = existingViewModel;
            Result = TranslationResult.ExistingViewModel;
        }

        public TranslationResult Result { get; private set; }
        public ICrossViewModel ExistingViewModel { get; private set; }
        public CrossViewModelRequest ViewModelRequest { get; private set; }
    }
}
