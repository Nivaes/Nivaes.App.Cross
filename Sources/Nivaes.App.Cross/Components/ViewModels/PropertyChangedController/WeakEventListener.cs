using System.ComponentModel;

namespace Nivaes.App.Cross
{
    public interface IWeakEventListener
    {
        void Detach();
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public class WeakEventListener<TInstance, TSource, TEventArgs>
        : IWeakEventListener
    {
        /// <summary>
        /// WeakReference to the instance listening for the event.
        /// </summary>
        private WeakReference _weakInstance;

        internal TInstance? Source
        {
            get
            {
                if (_weakInstance.IsAlive)
                    return (TInstance?)_weakInstance.Target;

                return default;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakEventListener{TInstance, TSource, TEventArgs}"/> class.
        /// </summary>
        /// <param name="instance">Instance subscribing to the event.</param>
        public WeakEventListener(TInstance instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            _weakInstance = new WeakReference(instance);
        }

        /// <summary>
        /// Gets or sets the method to call when the event fires.
        /// </summary>
        public Action<TInstance, TSource?, TEventArgs>? OnEventAction { get; set; }

        /// <summary>
        /// Gets or sets the method to call when detaching from the event.
        /// </summary>
        public Action<WeakEventListener<TInstance, TSource, TEventArgs>>? OnDetachAction { get; set; }

        /// <summary>
        /// Handler for the subscribed event calls OnEventAction to handle it.
        /// </summary>
        /// <param name="source">Event source.</param>
        /// <param name="eventArgs">Event arguments.</param>
        public void OnEvent(TSource? source, TEventArgs eventArgs)
        {
            TInstance? target = (TInstance?)_weakInstance.Target;
            if (target != null)
            {
                // Call registered action
                OnEventAction?.Invoke(target, source, eventArgs);
            }
            else
            {
                // Detach from event
                Detach();
            }
        }

        /// <summary>
        /// Detaches from the subscribed event.
        /// </summary>
        public void Detach()
        {
            OnDetachAction?.Invoke(this);
            OnDetachAction = null;
        }
    }
}
