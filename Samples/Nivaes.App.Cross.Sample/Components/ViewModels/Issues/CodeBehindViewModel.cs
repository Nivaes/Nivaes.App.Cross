using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample
{
    public class CodeBehindViewModel
        : CrossViewModel
    {
        private string _bindableText = "I'm bound!";
        public string BindableText
        {
            get
            {
                return _bindableText;
            }
            set
            {
                if (BindableText != value)
                {
                    _bindableText = value;
                    RaisePropertyChanged();
                }
            }
        }

        public CodeBehindViewModel(ILogger<CodeBehindViewModel> logger)
            : base(logger)
        { }
    }
}
