namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;

    public interface ICrossTargetBindingFactory
    {
        [RequiresUnreferencedCode("This method creates bindings using reflection which may not be preserved by trimming")]
        ICrossTargetBinding CreateBinding(object target, string targetName);
    }
}
