using System.ComponentModel;
using Android.Views;

namespace Nivaes.App.Cross.Droid
{
    internal class EditTextValidate
    {
        private bool mActivate = false;
        private readonly string mPropertyName;
        private readonly EditText mEditText;
        private readonly IBaseViewModel mDataContext;

        public EditTextValidate(EditText editText, IBaseViewModel dataContext, string propertyName)
        {
            mDataContext = dataContext;
            mEditText = editText;
            mPropertyName = propertyName;

            mEditText.FocusChange += EditTextFocusChange;

            var property = Property;
            if (property != null)
                property.PropertyChanged += PropertyPropertyChanged;
        }

        private void PropertyPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ValidateProperty.IsValid))
            {
                ExecuteActions();
            }
        }

        private void EditTextFocusChange(object? sender, View.FocusChangeEventArgs e)
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
                    mEditText.Error = string.Empty;
                }
                else
                {
                    mEditText.Error = property.Errors.FirstOrDefault();
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
