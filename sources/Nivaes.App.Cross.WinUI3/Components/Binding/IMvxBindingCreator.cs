namespace Nivaes.App.Cross.WinUI3
{
    using System;
    using System.Collections.Generic;
    using Microsoft.UI.Xaml;

    public interface IMvxBindingCreator
    {
        void CreateBindings(
            object sender,
            DependencyPropertyChangedEventArgs args,
            Func<string, IEnumerable<CrossBindingDescription>> parseBindingDescriptions);
    }
}
