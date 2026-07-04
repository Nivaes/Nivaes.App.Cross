namespace Nivaes.App.Cross.AppKitLib;

[AttributeUsage(AttributeTargets.Class)]
public class MvxFromStoryboardAttribute : Attribute
{
    public string? StoryboardName { get; set; }

    public MvxFromStoryboardAttribute(string? storyboardName = null)
    {
        this.StoryboardName = storyboardName;
    }
}
