namespace Nivaes.App.Cross.UIKitLib
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class MvxFromStoryboardAttribute : Attribute
    {
        public string StoryboardName { get; set; }

        public MvxFromStoryboardAttribute(string storyboardName = null)
        {
            StoryboardName = storyboardName;
        }
    }
}
