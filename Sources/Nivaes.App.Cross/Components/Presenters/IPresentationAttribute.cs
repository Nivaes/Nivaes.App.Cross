namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;

    public interface IPresentationAttribute
    {
        /// <summary>
        /// That shall be used only if you are using non generic views.
        /// </summary>
        [Obsolete("Usar PresentationData")]
        Type? ViewModelType { get; set; }

        /// <summary>
        /// Type of the view
        /// </summary>
        [Obsolete("Usar PresentationData")]
        Type? ViewType { get; set; }
    }
}
