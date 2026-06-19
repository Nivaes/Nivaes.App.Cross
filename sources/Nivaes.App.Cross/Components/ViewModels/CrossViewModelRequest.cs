namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    public class CrossViewModelRequest
    {
        public CrossViewModelRequest()
        {
        }

        public CrossViewModelRequest([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType)
        {
            ViewModelType = viewModelType;
        }

        public CrossViewModelRequest([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType,
            ICrossBundle? parameterBundle,
            ICrossBundle? presentationBundle)
        {
            ViewModelType = viewModelType;
            ParameterValues = parameterBundle.SafeGetData();
            PresentationValues = presentationBundle.SafeGetData();
        }

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        public Type? ViewModelType 
        { 
            get;
            set; 
        }

        public IDictionary<string, string>? ParameterValues { get; set; }
        public IDictionary<string, string>? PresentationValues { get; set; }

        public static CrossViewModelRequest GetDefaultRequest(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType)
        {
            return new CrossViewModelRequest(viewModelType, null, null);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"MvxViewModelRequest - ViewModelType: '{ViewModelType}'");
            if (ParameterValues != null)
            {
                sb.Append(
                    $", ParameterValues: '{string.Join(", ", ParameterValues.Select(kv => $"{{{kv.Key}: {kv.Value}}}"))}'");
            }

            if (PresentationValues != null)
            {
                sb.Append(
                    $", PresentationValues: '{string.Join(", ", PresentationValues.Select(kv => $"{{{kv.Key}: {kv.Value}}}"))}'");
            }

            return sb.ToString();
        }
    }

    public class CrossViewModelRequest<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>
        : CrossViewModelRequest where TViewModel : ICrossViewModel
    {
        public CrossViewModelRequest() : base(typeof(TViewModel))
        {
        }

        public CrossViewModelRequest(ICrossBundle? parameterBundle, ICrossBundle? presentationBundle)
            : base(typeof(TViewModel), parameterBundle, presentationBundle)
        {
        }

        public static CrossViewModelRequest GetDefaultRequest()
        {
            return GetDefaultRequest(typeof(TViewModel));
        }
    }
}