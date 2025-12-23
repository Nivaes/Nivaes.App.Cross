namespace Nivaes.App.Cross.UIKit
{
    using System.Linq;
    using UIKit;

    public class MvxIosSystem
        : IMvxIosSystem
    {
        public MvxIosVersion Version { get; private set; }

        public MvxIosSystem()
        {
            BuildVersion();
        }

        private void BuildVersion()
        {
            var version = UIDevice.CurrentDevice.SystemVersion;
            var parts = version.Split('.').Select(int.Parse).ToArray();
            Version = new MvxIosVersion(parts);
        }
    }
}
