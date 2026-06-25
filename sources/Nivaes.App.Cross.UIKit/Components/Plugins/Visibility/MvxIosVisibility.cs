namespace Nivaes.App.Cross.UIKitOS
{
    public class MvxIosVisibility : ICrossNativeVisibility
    {
        public object ToNative(CrossVisibility visibility)
        {
            return visibility;
        }
    }
}
