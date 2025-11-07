namespace Nivaes.App.Cross
{
    public class CrossLiteralSourceStepDescription : 
        CrossSourceStepDescription
    {
        public object? Literal { get; set; }

        public override string? ToString()
        {
            return Literal == null ? "-null-" : Literal.ToString();
        }
    }
}
