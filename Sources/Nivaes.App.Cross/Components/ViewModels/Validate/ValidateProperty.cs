using System.ComponentModel;

namespace Nivaes.App.Cross
{
    public class ValidateProperty
        : INotifyPropertyChanged
    {
        private IEnumerable<string> _errors = Array.Empty<string>();

        public IEnumerable<string> Errors
        {
            get => _errors;
            set
            {
                if (_errors != value)
                {
                    _errors = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Errors)));
                }
            }
        }

        public bool IsValid => !_errors.Any();

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
