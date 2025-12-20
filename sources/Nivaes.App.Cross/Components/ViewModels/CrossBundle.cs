namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using MvvmCross.Core;

    public class CrossBundle(IDictionary<string, string>? data) 
        : ICrossBundle
    {
        public CrossBundle()
            : this(new Dictionary<string, string>())
        {
        }

        public IDictionary<string, string> Data { get; } = data ?? new Dictionary<string, string>();

        public void Write(object toStore)
        {
            ArgumentNullException.ThrowIfNull(toStore);
            Data.Write(toStore);
        }

        public T? Read<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicProperties)] T>()
            where T : new()
        {
            return Data.Read<T>();
        }

        public object? Read([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicProperties)] Type type)
        {
            return Data.Read(type);
        }

        public IEnumerable<object> CreateArgumentList(IEnumerable<ParameterInfo> requiredParameters, string? debugText)
        {
            return Data.CreateArgumentList(requiredParameters, debugText);
        }
    }
}