namespace Nivaes.App.Cross.Droid
{
    public class CrossTranslatedIntent
    {
        #region TranslationResult enum

        public enum TranslationResult
        {
            Request,
            ExistingViewModel
        }

        #endregion TranslationResult enum

        public CrossTranslatedIntent(ICrossViewModelRequest viewModelRequest)
        {
            ViewModelRequest = viewModelRequest;
            Result = TranslationResult.Request;
        }

        public CrossTranslatedIntent(ICrossViewModel existingViewModel)
        {
            ExistingViewModel = existingViewModel;
            Result = TranslationResult.ExistingViewModel;
        }

        public TranslationResult Result { get; private set; }
        public ICrossViewModel? ExistingViewModel { get; private set; }
        public ICrossViewModelRequest? ViewModelRequest { get; private set; }
    }
}
