namespace Nivaes.App.Cross.AppKitLib
{
    public class CrossMacVisibility : ICrossNativeVisibility
    {
        public object ToNative(CrossVisibility visibility)
        {
            return visibility;
        }
    }
}
