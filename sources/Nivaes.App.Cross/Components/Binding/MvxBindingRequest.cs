namespace MvvmCross.Binding
{
    using Nivaes.App.Cross;

    public class MvxBindingRequest
    {
        public MvxBindingRequest()
        {
        }

        public MvxBindingRequest(object source, object target, CrossBindingDescription description)
        {
            Target = target;
            Source = source;
            Description = description;
        }

        public object Target { get; set; }
        public object Source { get; set; }
        public CrossBindingDescription Description { get; set; }
    }
}
