namespace Nivaes.App.Cross.UIKit
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CrossFromStoryboardAttribute : Attribute
    {
        public string StoryboardName { get; set; }

        public CrossFromStoryboardAttribute(string storyboardName = null)
        {
            StoryboardName = storyboardName;
        }
    }
}
