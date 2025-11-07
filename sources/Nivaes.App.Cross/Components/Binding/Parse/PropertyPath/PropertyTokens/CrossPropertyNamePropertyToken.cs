namespace Nivaes.App.Cross
{
    public record CrossPropertyNamePropertyToken(string PropertyName) : 
        ICrossPropertyToken
    {
        public override string ToString() => "Property:" + PropertyName;
    }
}