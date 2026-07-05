namespace Nivaes.App
{
    /// <summary>Information of element.</summary>
    internal sealed class ModelElementInformation
    {
        #region Properties
        /// <summary>Element of tree.</summary>
        internal object Element { get; private set; }

        /// <summary>Path of element in tree.</summary>
        internal string Path { get; private set; }

        /// <summary>Is collection.</summary>
        internal bool IsCollection { get; private set; }
        #endregion

        #region Constructor
        /// <summary>Create a new instance of <see cref="ModelElementInformation"/>.</summary>
        /// <param name="element">Element of tree.</param>
        /// <param name="path">Path of element in tree.</param>
        /// <param name="isCollection">Is collection.</param>
        internal ModelElementInformation(object element, string path, bool isCollection)
        {
            Element = element;
            Path = path;
            IsCollection = isCollection;
        }
        #endregion
    }
}
