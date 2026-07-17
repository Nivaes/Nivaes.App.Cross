using System.ComponentModel;
using Android.Views;
using Google.Android.Material.TextField;
using Nivaes.App.Cross;

namespace Nivaes.App.Droid
{
    internal class TextInputLayoutValidate
    {
        private bool mActivate = false;
        private readonly string mPropertyName;
        private readonly TextInputLayout mTextInputLayout;
        private readonly IBaseViewModel mDataContext;

        public TextInputLayoutValidate(TextInputLayout textInputLayout, IBaseViewModel dataContext, string propertyName)
        {
            mDataContext = dataContext;
            mTextInputLayout = textInputLayout;
            mPropertyName = propertyName;

            var editText = textInputLayout.EditText;
            editText.FocusChange += EditTextFocusChange;

            var property = Property;
            if (property != null)
                property.PropertyChanged += PropertyPropertyChanged;
        }

        private void PropertyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ValidateProperty.IsValid))
            {
                ExecuteActions();
            }
        }

        private void EditTextFocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (!e.HasFocus)
            {
                if (!mActivate)
                {
                    if (mDataContext is IBaseViewModel baseViewModel)
                    {
                        baseViewModel.ValidateController.Validate(mPropertyName);
                    }

                    mActivate = true;
                    ExecuteActions();
                }
            }
        }

        private void ExecuteActions()
        {
            if (mActivate)
            {
                var property = Property;
                if (property == null || property.IsValid)
                {
                    mTextInputLayout.ErrorEnabled = false;
                    mTextInputLayout.Error = string.Empty;
                }
                else
                {
                    mTextInputLayout.ErrorEnabled = true;
                    mTextInputLayout.Error = property.Errors.FirstOrDefault();
                }
            }
        }

        #region Property
        private ValidateProperty mProperty;

        private ValidateProperty Property
        {
            get
            {
                if (mProperty == null)
                {
                    mProperty = mDataContext.ValidateController.GetValidateProperty(mPropertyName);
                }
                return mProperty;
            }
        }
        #endregion
    }
}
