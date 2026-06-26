namespace Nivaes.App.Cross
{
    public class CrossPathSourceStepDescription
        : CrossSourceStepDescription
    {
        public string? SourcePropertyPath { get; set; }

        public override string ToString()
        {
            return SourcePropertyPath ?? "-empty-";
        }
    }
}
