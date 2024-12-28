namespace Nivaes.App.Cross.WinUI.Presenters
{
    using System;
    using Nivaes.App.Cross.Presenters;

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class RegionPresentationAttribute : PresentationAttribute
    {
        public string RegionName { get; private set; }

        public RegionPresentationAttribute(string regionName)
        {
            RegionName = regionName;
        }
    }
}
