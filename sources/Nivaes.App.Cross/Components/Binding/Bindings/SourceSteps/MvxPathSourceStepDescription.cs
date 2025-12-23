namespace Nivaes.App.Cross
{
    public class MvxPathSourceStepDescription 
        : MvxSourceStepDescription
    {
        public string? SourcePropertyPath { get; set; }

        public override string ToString()
        {
            return SourcePropertyPath ?? "-empty-";
        }
    }
}
