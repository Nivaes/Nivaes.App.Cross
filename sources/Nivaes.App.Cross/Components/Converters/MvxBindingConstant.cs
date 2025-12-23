namespace Nivaes.App.Cross
{
    public sealed class MvxBindingConstant
    {
        public static readonly MvxBindingConstant DoNothing = new MvxBindingConstant("DoNothing");
        public static readonly MvxBindingConstant UnsetValue = new MvxBindingConstant("UnsetValue");

        private readonly string _debug;

        private MvxBindingConstant(string debug)
        {
            _debug = debug;
        }

        public override string ToString()
        {
            return "Binding:" + _debug;
        }
    }
}
