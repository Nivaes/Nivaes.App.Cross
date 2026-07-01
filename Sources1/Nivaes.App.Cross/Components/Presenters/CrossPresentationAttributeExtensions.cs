namespace Nivaes.App.Cross
{
    public static class CrossPresentationAttributeExtensions
    {
        public static bool HasBasePresentationAttribute(this Type candidateType)
        {
            var attributes = candidateType.GetCustomAttributes(typeof(CrossBasePresentationAttribute), true);
            return attributes.Length > 0;
        }

        public static IEnumerable<CrossBasePresentationAttribute> GetBasePresentationAttributes(this Type fromViewType)
        {
            var attributes = fromViewType.GetCustomAttributes(typeof(CrossBasePresentationAttribute), true);

            if (attributes.Length == 0)
                throw new InvalidOperationException($"Type does not have {nameof(CrossBasePresentationAttribute)} attribute!");

            return attributes.Cast<CrossBasePresentationAttribute>();
        }

        public static CrossBasePresentationAttribute? GetBasePresentationAttribute(this Type fromViewType)
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
                where TMvxPresentationAttribute : class, ICrossPresentationAttribute
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
