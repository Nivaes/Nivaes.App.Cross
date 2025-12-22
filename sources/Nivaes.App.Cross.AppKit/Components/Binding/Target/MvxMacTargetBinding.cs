namespace MvvmCross.Platforms.Mac.Binding.Target
{
    using Nivaes.App.Cross;

    public abstract class MvxMacTargetBinding 
        : CrossConvertingTargetBinding
    {
        protected MvxMacTargetBinding(object view)
            : base(view)
        {
        }
    }
}
