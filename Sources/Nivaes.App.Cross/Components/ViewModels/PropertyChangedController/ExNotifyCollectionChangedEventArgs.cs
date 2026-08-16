using System.Collections;
using System.Collections.Specialized;

namespace Nivaes.App.Cross
{
    /// <summary>Represents the method that will handle the <see cref="System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged"/> event raised when a property is changed on a component.</summary>
    public delegate void ExNotifyCollectionChangedEventHandler(object? sender, ExNotifyCollectionChangedEventArgs e);

    /// <summary>Provides data for the <see cref="System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged"/> event.</summary>
    public sealed class ExNotifyCollectionChangedEventArgs : NotifyCollectionChangedEventArgs
    {
        #region Constructors
        // <summary>Create a new instance of the <see cref="CenterPropertyChangedEventArgs"/> class.</summary>
        /// <param name="source">Source of event.</param>
        /// <param name="rootSource">Root elemento of source of event.</param>
        /// <param name="path">Path of element.</param>
        internal ExNotifyCollectionChangedEventArgs(object source, object rootSource, string path)
            : base(NotifyCollectionChangedAction.Reset)
        {
            Source = source;
            RootSource = rootSource;
            Path = path;
        }

        /// <summary>Create a new instance of the <see cref="CenterPropertyChangedEventArgs"/> class.</summary>
        /// <param name="source">Source of event.</param>
        /// <param name="rootSource">Root elemento of source of event.</param>
        /// <param name="path">Path of element.</param>
        /// <param name="handler">The event mHandler.</param>
        /// <param name="rootHandler">Root handler.</param>
        /// <param name="action">The action that caused the event. This must be set to <see cref="System.Collections.Specialized.NotifyCollectionChangedAction.Reset" />.</param>
        /// <param name="changedItems">The item that is affected by the change.</param>
        internal ExNotifyCollectionChangedEventArgs(object source,
                object rootSource, string path,
                NotifyCollectionChangedAction action, IList changedItems) :
            base(action, changedItems)
        {
            Source = source;
            RootSource = rootSource;
            Path = path;
        }

        /// <summary>Create a new instance of the <see cref="CenterPropertyChangedEventArgs"/> class.</summary>
        /// <param name="source">Source of event.</param>
        /// <param name="rootSource">Root elemento of source of event.</param>
        /// <param name="path">Path of element.</param>
        /// <param name="handler">The event mHandler.</param>
        /// <param name="rootHandler">Root handler.</param>
        /// <param name="action">The action that caused the event. This must be set to <see cref="System.Collections.Specialized.NotifyCollectionChangedAction.Reset" />.</param>
        /// <param name="newItems">The new items that are replacing the original items.</param>
        /// <param name="oldItems">The original items that are replaced.</param>
        internal ExNotifyCollectionChangedEventArgs(object? source, object rootSource, string path,
                NotifyCollectionChangedAction action, IList newItems, IList oldItems) :
            base(action, newItems, oldItems)
        {
            Source = source;
            RootSource = rootSource;
            Path = path;
        }
        #endregion

        #region Properties
        /// <summary>Source of event.</summary>
        public object? Source { get; private set; }

        /// <summary>Root elemento of source of event.</summary>
        public object RootSource { get; private set; }

        /// <summary>Path of element.</summary>
        public string Path { get; private set; }
        #endregion
    }
}
