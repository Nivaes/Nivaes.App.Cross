namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;

    public interface IPresentationAttribute
    {
        /// <summary>
        /// That shall be used only if you are using non generic views.
        /// </summary>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        Type? ViewModelType { get; set; }

        /// <summary>
        /// Type of the view
        /// </summary>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        Type? ViewType { get; set; }
    }
}
