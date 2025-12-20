namespace MvvmCross.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using MvvmCross.Core;
    using Nivaes.App.Cross;

    public class MvxViewModelRequest
    {
        public MvxViewModelRequest()
        {
        }

        public MvxViewModelRequest([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType)
        {
            ViewModelType = viewModelType;
        }

        public MvxViewModelRequest([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType,
            ICrossBundle? parameterBundle,
            ICrossBundle? presentationBundle)
        {
            ViewModelType = viewModelType;
            ParameterValues = parameterBundle.SafeGetData();
            PresentationValues = presentationBundle.SafeGetData();
        }

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        public Type? ViewModelType { get; set; }
        public IDictionary<string, string>? ParameterValues { get; set; }
        public IDictionary<string, string>? PresentationValues { get; set; }

        public static MvxViewModelRequest GetDefaultRequest(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewModelType)
        {
            return new MvxViewModelRequest(viewModelType, null, null);
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

    public class MvxViewModelRequest<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TViewModel>
        : MvxViewModelRequest where TViewModel : ICrossViewModel
    {
        public MvxViewModelRequest() : base(typeof(TViewModel))
        {
        }

        public MvxViewModelRequest(ICrossBundle? parameterBundle, ICrossBundle? presentationBundle)
            : base(typeof(TViewModel), parameterBundle, presentationBundle)
        {
        }

        public static MvxViewModelRequest GetDefaultRequest()
        {
            return GetDefaultRequest(typeof(TViewModel));
        }
    }
}