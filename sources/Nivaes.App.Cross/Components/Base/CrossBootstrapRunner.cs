namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Microsoft.Extensions.Logging;
    using MvvmCross.IoC;

    [Obsolete("", true)]
    public class CrossBootstrapRunner
    {
        [RequiresUnreferencedCode("This method uses reflection to check for creatable types, which may not be preserved by trimming")]
        public virtual void Run(Assembly assembly)
        {
            var types = assembly.CreatableTypes()
                                .Inherits<ICrossBootstrapAction>();

            foreach (var type in types)
            {
                Run(type);
            }
        }

        protected virtual void Run([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type type)
        {
            ArgumentNullException.ThrowIfNull(type);

            try
            {
                var toRun = Activator.CreateInstance(type);
                if (toRun is not ICrossBootstrapAction bootstrapAction)
                {
                    CrossLogHost.Default?.Log(LogLevel.Trace,
                        "Could not run startup task {TypeName} - it's not a startup task", type.Name);
                    return;
                }

                bootstrapAction.Run();
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception exception)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                // pokemon handling
                CrossLogHost.Default?.Log(LogLevel.Trace, exception, "Error running startup task {TypeName}", type.Name);
            }
        }
    }
}
