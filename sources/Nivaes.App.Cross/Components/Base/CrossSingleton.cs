namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;

    [Obsolete("", true)]
    public abstract class CrossSingleton
        : IDisposable
    {
        ~CrossSingleton()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract void Dispose(bool isDisposing);

        private static readonly List<CrossSingleton> Singletons = new List<CrossSingleton>();

        protected CrossSingleton()
        {
            lock (Singletons)
            {
                Singletons.Add(this);
            }
        }

        public static void ClearAllSingletons()
        {
            lock (Singletons)
            {
                foreach (var s in Singletons)
                {
                    s.Dispose();
                }

                Singletons.Clear();
            }
        }
    }

    [Obsolete("")]
    public abstract class CrossSingleton<TInterface>
        : CrossSingleton
        where TInterface : class
    {
        protected CrossSingleton()
        {
            if (Instance != null)
                throw new CrossException("You cannot create more than one instance of MvxSingleton");

            Instance = this as TInterface;
        }

        public static TInterface? Instance { get; private set; }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                Instance = null;
            }
        }
    }
}
