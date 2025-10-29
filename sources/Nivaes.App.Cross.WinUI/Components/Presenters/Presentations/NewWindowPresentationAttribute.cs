namespace Nivaes.App.Cross.WinUI.Presenters
{
    using System;
    using Nivaes.App.Cross.Presenters;

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class NewWindowPresentationAttribute : CrossPresentationAttribute
    {
        public NewWindowPresentationAttribute()
        {
        }

        public NewWindowPresentationAttribute(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public int? Width { get; }

        public int? Height { get; }
    }
}
