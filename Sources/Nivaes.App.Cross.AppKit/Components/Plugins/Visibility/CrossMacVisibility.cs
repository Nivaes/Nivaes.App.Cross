namespace Nivaes.App.Cross.AppKitOS
{
    public class CrossMacVisibility : ICrossNativeVisibility
    {
        public object ToNative(CrossVisibility visibility)
        {
            return visibility;
        }
    }
}
