namespace Nivaes.App.Cross
{
    public class ValidateController
    {
        private readonly IInternalBaseViewModel mBaseViewModel;

        private readonly Dictionary<string, ValidateProperty> mProperties = new Dictionary<string, ValidateProperty>();

        internal ValidateController(IInternalBaseViewModel baseViewModel)
        {
            mBaseViewModel = baseViewModel;
        }

        internal void Initialize()
        {
            DataModelPropertyChangedController controller =
                new DataModelPropertyChangedController(mBaseViewModel, DataPropertyChanged);
        }

        private void DataPropertyChanged(object sender, ExPropertyChangedEventArgs e)
        {
            if (e.FullPropertyName.StartsWith(nameof(CrossViewModel.InitializeTask)))
                return;

            Validate(e.FullPropertyName);

            mBaseViewModel.OnDataModeChanged(e);
        }

        public ValidateProperty GetValidateProperty(string propertyName)
        {
            if (!mProperties.TryGetValue(propertyName, out ValidateProperty? validateProperty))
            {
                validateProperty = new ValidateProperty();
                mProperties.Add(propertyName, validateProperty);
            }

            return validateProperty;
        }

        public async void Validate(string propertyName)
        {
            throw new NotImplementedException();
            //if (mBaseViewModel.Validator == null)
            //    return;

            //try
            //{
            //    var results = await mBaseViewModel.Validator.ValidateAsync(
            //            new ValidationContext<IInternalBaseViewModel>(mBaseViewModel, new PropertyChain(), new MemberNameValidatorSelector(new string[] { propertyName })),
            //            new CancellationTokenSource(300).Token
            //        ).ConfigureAwait(false);

            //    if (results.IsValid)
            //    {
            //        GetValidateProperty(propertyName).Errors = Array.Empty<string>();
            //    }
            //    else
            //    {
            //        GetValidateProperty(propertyName).Errors = results.Errors.Select(v => v.ErrorMessage);
            //    }
            //}
            //catch (OperationCanceledException)
            //{
            //}
        }
    }
}
