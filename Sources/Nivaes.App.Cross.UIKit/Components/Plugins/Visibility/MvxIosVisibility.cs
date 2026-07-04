namespace Nivaes.App.Cross.UIKitLib
{
    public class MvxIosVisibility : ICrossNativeVisibility
    {
        public object ToNative(CrossVisibility visibility)
        {
            return visibility;
        }
    }
}
