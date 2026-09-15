using System.ComponentModel;
using Android.Views;

namespace Nivaes.App.Cross.Droid
{
    internal class EditTextValidate
    {
        private bool _activate = false;
        private readonly string _propertyName;
        private readonly EditText _editText;
        private readonly IBaseViewModel _dataContext;

        public EditTextValidate(EditText editText, IBaseViewModel dataContext, string propertyName)
        {
            _dataContext = dataContext;
            _editText = editText;
            _propertyName = propertyName;

            _editText.FocusChange += EditTextFocusChange;

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
                if (!_activate)
                {
                    if (_dataContext is IBaseViewModel baseViewModel)
                    {
                        baseViewModel.ValidateController.Validate(_propertyName);
                    }

                    _activate = true;
                    ExecuteActions();
                }
            }
        }

        private void ExecuteActions()
        {
            if (_activate)
            {
                var property = Property;
                if (property == null || property.IsValid)
                {
                    _editText.Error = string.Empty;
                }
                else
                {
                    _editText.Error = property.Errors.FirstOrDefault();
                }
            }
        }

        #region Property
        private ValidateProperty? mProperty;

        private ValidateProperty? Property
        {
            get
            {
                if (mProperty == null)
                {
                    mProperty = _dataContext.ValidateController.GetValidateProperty(_propertyName!);
                }
                return mProperty;
            }
        }
        #endregion
    }
}
