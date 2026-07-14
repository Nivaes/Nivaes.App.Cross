namespace Nivaes.App.Cross
{
    public static class CrossPresentationAttributeExtensions
    {
        public static bool HasBasePresentationAttribute(this Type candidateType)
        {
            var attributes = candidateType.GetCustomAttributes(typeof(BasePresentationAttribute), true);
            return attributes.Length > 0;
        }

        public static IEnumerable<BasePresentationAttribute> GetBasePresentationAttributes(this Type fromViewType)
        {
            var attributes = fromViewType.GetCustomAttributes(typeof(BasePresentationAttribute), true);

            if (attributes.Length == 0)
                throw new InvalidOperationException($"Type does not have {nameof(BasePresentationAttribute)} attribute!");

            return attributes.Cast<BasePresentationAttribute>();
        }

        public static BasePresentationAttribute? GetBasePresentationAttribute(this Type fromViewType)
        {
            return fromViewType.GetBasePresentationAttributes().FirstOrDefault();
        }

        public static Type? GetViewModelType(this Type viewType)
        {
            if (!viewType.HasBasePresentationAttribute())
                return null;

            return viewType.GetBasePresentationAttributes()
                .Select(x => x.ViewModelType)
                .FirstOrDefault();
        }

        public static void Register<TMvxPresentationAttribute>(
            this IDictionary<Type, CrossPresentationAttributeAction> attributeTypesToActionsDictionary,
            Func<Type, TMvxPresentationAttribute, CrossViewModelRequest, Task<bool>> showAction,
            Func<ICrossViewModel, TMvxPresentationAttribute, Task<bool>> closeAction)
                where TMvxPresentationAttribute : class, IPresentationAttribute
        {
            attributeTypesToActionsDictionary.Add(
                typeof(TMvxPresentationAttribute),
                new CrossPresentationAttributeAction
                {
                    ShowAction = (view, attribute, request) => showAction(view, (attribute as TMvxPresentationAttribute)!, request),
                    CloseAction = (viewModel, attribute) => closeAction(viewModel, (attribute as TMvxPresentationAttribute)!)
                });
        }
    }
}
