namespace Nivaes.App.Cross.UIKit
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
