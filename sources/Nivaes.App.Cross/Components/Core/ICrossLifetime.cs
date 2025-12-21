namespace Nivaes.App.Cross
{
    using System;
    using MvvmCross.Core;

    public interface ICrossLifetime
    {
        event EventHandler<CrossLifetimeEventArgs>? LifetimeChanged;
    }
}
