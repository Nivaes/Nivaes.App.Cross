namespace Nivaes.App.Cross
{
    using System;

    public class ViewsManager : KeyContainerManager<Type>
    {
        public static KeyPresentation New<TViewModel, TView>()
            where TViewModel : IViewModel
            where TView : IView
        {
            return new KeyPresentation { Key = typeof(TViewModel).GetHashCode(), Value = typeof(TView) };
        }

        public ViewsManager()
        {
        }

        public ViewsManager(KeyPresentation[] presentations)
            : base(presentations)
        {
        }
    }
}
