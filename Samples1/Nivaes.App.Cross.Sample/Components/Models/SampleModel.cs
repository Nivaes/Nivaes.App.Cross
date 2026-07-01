namespace Nivaes.App.Cross.Sample;

public record SampleModel(string Message, decimal Value)
{
    public override string ToString() => $"Message: {Message}, Value: {Value}";
}
