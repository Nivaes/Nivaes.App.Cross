using System.ComponentModel;

namespace Nivaes.App.Cross
{
    /// <summary>Represents the method that will handle the <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"/> event raised when a property is changed on a component.</summary>
    public delegate void ExPropertyChangedEventHandler(object? sender, ExPropertyChangedEventArgs e);

    /// <summary>Provides data for the <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"/> event.</summary>
    public sealed class ExPropertyChangedEventArgs
        : PropertyChangedEventArgs
    {
        #region Constructors
        /// <summary>Create a new instance of the <see cref="CenterPropertyChangedEventArgs"/> class.</summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        /// <param name="source">Source of event.</param>
        /// <param name="rootSource">Root elemento of source of event.</param>
        /// <param name="path">Path of element.</param>
        internal ExPropertyChangedEventArgs(string propertyName, object source, object rootSource, string path)
            : base(propertyName)
        {
            Source = source;
            RootSource = rootSource;
            FullPropertyName = string.Concat(path, propertyName);
        }
        #endregion

        #region Properties
        /// <summary>Source of event.</summary>
        public object Source { get; private set; }

        /// <summary>Root elemento of source of event.</summary>
        public object RootSource { get; private set; }

        /// <summary>Full property name of element.</summary>
        public string FullPropertyName { get; private set; }
        #endregion
    }
}
