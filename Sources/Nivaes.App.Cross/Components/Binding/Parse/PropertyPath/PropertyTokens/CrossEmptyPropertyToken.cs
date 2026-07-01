namespace Nivaes.App.Cross
{
    public record CrossEmptyPropertyToken : ICrossPropertyToken
    {
        public override string ToString()
        {
            return "Property:WholeObject";
        }
    }
}