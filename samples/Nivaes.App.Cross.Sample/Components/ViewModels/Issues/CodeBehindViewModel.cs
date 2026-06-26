namespace Nivaes.App.Cross.Sample
{
    using Nivaes.App.Cross;

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
    }
}
