namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    [RequiresUnreferencedCode("This class uses reflection which may not be preserved during trimming")]
    public class CrossSimpleLeafPropertyInfoSourceBinding(object source, PropertyInfo propertyInfo)
        : CrossLeafPropertyInfoSourceBinding(source, propertyInfo)
    {
        protected override object[] PropertyIndexParameters() => [];
    }
}
