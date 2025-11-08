namespace Nivaes.App.Cross.WinUI3
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.UI.Xaml.Controls;

    public interface ICrossSuspensionManager
    {
        Dictionary<string, object> SessionState { get; }

        List<Type> KnownTypes { get; }

        Task SaveAsync();

        Task RestoreAsync();

        /// <summary>
        /// Registers a <see cref="Frame"/> instance to allow its navigation history to be saved to
        /// and restored from <see cref="CrossSuspensionManager.SessionState"/>.  Frames should be registered once
        /// immediately after creation if they will participate in session state management.  Upon
        /// registration if state has already been restored for the specified key
        /// the navigation history will immediately be restored.  Subsequent invocations of
        /// <see cref="CrossSuspensionManager.RestoreAsync"/> will also restore navigation history.
        /// </summary>
        /// <param name="frame">An instance whose navigation history should be managed by
        /// <see cref="CrossSuspensionManager"/></param>
        /// <param name="sessionStateKey">A unique key into <see cref="CrossSuspensionManager.SessionState"/> used to
        /// store navigation-related information.</param>
        void RegisterFrame(ICrossWindowsFrame frame, string sessionStateKey);

        /// <summary>
        /// Disassociates a <see cref="Frame"/> previously registered by <see cref="RegisterFrame"/>
        /// from <see cref="SessionState"/>.  Any navigation state previously captured will be
        /// removed.
        /// </summary>
        /// <param name="frame">An instance whose navigation history should no longer be
        /// managed.</param>
        void UnregisterFrame(ICrossWindowsFrame frame);

        /// <summary>
        /// Provides storage for session state associated with the specified <see cref="Frame"/>.
        /// Frames that have been previously registered with <see cref="CrossSuspensionManager.RegisterFrame"/> have
        /// their session state saved and restored automatically as a part of the global
        /// <see cref="CrossSuspensionManager.SessionState"/>.  Frames that are not registered have transient state
        /// that can still be useful when restoring pages that have been discarded from the
        /// navigation cache.
        /// </summary>
        /// <remarks>Apps may choose to rely on <see cref="LayoutAwarePage"/> to manage
        /// page-specific state instead of working with frame session state directly.</remarks>
        /// <param name="frame">The instance for which session state is desired.</param>
        /// <returns>A collection of state subject to the same serialization mechanism as
        /// <see cref="CrossSuspensionManager.SessionState"/>.</returns>
        Dictionary<string, object> SessionStateForFrame(ICrossWindowsFrame frame);
    }
}
