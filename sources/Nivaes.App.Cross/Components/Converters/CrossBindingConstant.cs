namespace Nivaes.App.Cross
{
    [Obsolete("Quitar IoC de Cross")]
    public sealed class CrossBindingConstant
    {
        public static readonly CrossBindingConstant DoNothing = new CrossBindingConstant("DoNothing");
        public static readonly CrossBindingConstant UnsetValue = new CrossBindingConstant("UnsetValue");

        private readonly string _debug;

        private CrossBindingConstant(string debug)
        {
            _debug = debug;
        }

        public override string ToString()
        {
            return "Binding:" + _debug;
        }
    }
}
