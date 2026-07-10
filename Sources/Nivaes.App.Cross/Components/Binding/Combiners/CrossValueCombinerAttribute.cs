namespace Nivaes.App.Cross;

public class CrossValueCombinerAttribute : Attribute
{
    public string? Name { get; set; }

    public bool Register { get; set; } = true;
}
