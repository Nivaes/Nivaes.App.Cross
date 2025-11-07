namespace Nivaes.App.Cross
{
    public class CrossBindingRequest
    {
        public CrossBindingRequest()
        {
        }

        public CrossBindingRequest(object source, object target, CrossBindingDescription description)
        {
            Target = target;
            Source = source;
            Description = description;
        }

        public object? Target { get; set; }
        public object? Source { get; set; }
        public CrossBindingDescription? Description { get; set; }
    }
}
