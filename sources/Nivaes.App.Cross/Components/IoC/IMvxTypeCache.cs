namespace MvvmCross.IoC
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    public interface IMvxTypeCache
    {
        Dictionary<string, Type> LowerCaseFullNameCache { get; }
        Dictionary<string, Type> FullNameCache { get; }
        Dictionary<string, Type> NameCache { get; }

        [RequiresUnreferencedCode("This method uses reflection which may not be preserved in trimming scenarios")]
        void AddAssembly(Assembly assembly);
    }
}
