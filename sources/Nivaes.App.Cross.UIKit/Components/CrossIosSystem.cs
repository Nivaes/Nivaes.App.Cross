namespace Nivaes.App.Cross.UIKit
{
    using System.Linq;
    using UIKit;

    public class CrossIosSystem
        : ICrossIosSystem
    {
        public CrossIosVersion Version { get; private set; }

        public CrossIosSystem()
        {
            BuildVersion();
        }

        private void BuildVersion()
        {
            var version = UIDevice.CurrentDevice.SystemVersion;
            var parts = version.Split('.').Select(int.Parse).ToArray();
            Version = new CrossIosVersion(parts);
        }
    }
}
