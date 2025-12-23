namespace MvvmCross.Platforms.WinUi.Binding
{
    using System;
    using System.Collections.Generic;
    using Microsoft.UI.Xaml;
    using Nivaes.App.Cross;

    public interface IMvxBindingCreator
    {
        void CreateBindings(
            object sender,
            DependencyPropertyChangedEventArgs args,
            Func<string, IEnumerable<CrossBindingDescription>> parseBindingDescriptions);
    }
}
