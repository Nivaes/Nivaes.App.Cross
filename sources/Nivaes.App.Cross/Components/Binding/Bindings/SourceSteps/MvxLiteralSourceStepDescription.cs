namespace Nivaes.App.Cross
{
    public class MvxLiteralSourceStepDescription 
        : MvxSourceStepDescription
    {
        public object? Literal { get; set; }

        public override string? ToString()
        {
            return Literal == null ? "-null-" : Literal.ToString();
        }
    }
}
