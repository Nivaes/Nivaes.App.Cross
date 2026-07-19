namespace Nivaes.App.Cross.Droid
{
    public class MvxTranslatedIntent
    {
        #region TranslationResult enum

        public enum TranslationResult
        {
            Request,
            ExistingViewModel
        }

        #endregion TranslationResult enum

        public MvxTranslatedIntent(ViewModelRequest viewModelRequest)
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
        public ICrossViewModel? ExistingViewModel { get; private set; }
        public ViewModelRequest ViewModelRequest { get; private set; }
    }
}
