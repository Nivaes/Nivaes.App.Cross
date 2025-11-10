namespace Nivaes.App.Cross.WinUI3
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CrossNewWindowPresentationAttribute : CrossPresentationAttribute
    {
        public CrossNewWindowPresentationAttribute()
        {
        }

        public CrossNewWindowPresentationAttribute(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public int? Width { get; }

        public int? Height { get; }
    }
}
