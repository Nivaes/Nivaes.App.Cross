namespace Nivaes.App.Cross.WinUI3
{
    using System;
    using Nivaes.App.Cross.Presenters;

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CrossRegionPresentationAttribute : 
        CrossPresentationAttribute
    {
        public string RegionName { get; private set; }

        public CrossRegionPresentationAttribute(string regionName)
        {
            RegionName = regionName;
        }

        public string? Name { get; private set; }
    }
}
