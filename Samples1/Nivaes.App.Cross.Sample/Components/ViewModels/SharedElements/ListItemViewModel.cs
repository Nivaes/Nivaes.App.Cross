namespace Nivaes.App.Cross.Sample;

public class ListItemViewModel
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public override string? ToString() => Title;
}
