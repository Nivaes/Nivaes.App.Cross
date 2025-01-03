namespace Nivaes.App.Cross.Presenters
{
    public class ViewPresentationsManager : KeyContainerManager<Type>
    {
        public static KeyPresentation New<TView, TPresentationType>()
            where TView : IView
            where TPresentationType : IViewPresentation
        {
            return new KeyPresentation { Key = typeof(TView).GetHashCode(), Value = typeof(TPresentationType) };
        }

        public ViewPresentationsManager()
        {
        }

        public ViewPresentationsManager(KeyPresentation[] presentations)
            : base(presentations)
        {
        }   
    }
}
