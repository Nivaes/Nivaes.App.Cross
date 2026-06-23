namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    [Obsolete("", true)]
    public interface ICrossNamedInstanceRegistry<in T>
        where T : notnull
    {
        void AddOrOverwrite(string name, T instance);

        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
        void AddOrOverwriteFrom(Assembly assembly);
    }
}
