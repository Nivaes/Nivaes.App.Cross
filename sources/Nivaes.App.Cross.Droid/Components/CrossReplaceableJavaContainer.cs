namespace Nivaes.App.Cross.Droid
{
    public class CrossReplaceableJavaContainer : Java.Lang.Object
    {
        public object? Object { get; set; }

        public override string ToString()
        {
            return Object?.ToString() ?? string.Empty;
        }
    }
}
