namespace Nivaes.App.Cross.Droid
{
    public static class PresentationAttributeExtensions
    {
        public static bool IsFragmentCacheable(this Type fragmentType, Type fragmentActivityParentType)
        {
            if (!fragmentType.HasBasePresentationAttribute())
                return false;

            var fragmentAttributes =
                fragmentType.GetBasePresentationAttributes()
                    .Select(baseAttribute => baseAttribute as FragmentPresentationAttribute)
                    .Where(fragmentAttribute => fragmentAttribute != null);

            var currentAttribute = fragmentAttributes.FirstOrDefault(
                fragmentAttribute => fragmentAttribute != null &&
                fragmentAttribute.ActivityHostViewModelType == fragmentActivityParentType);

            return currentAttribute?.IsCacheableFragment == true;
        }

        public static PopBackStackFlags ToNativePopBackStackFlags(this PopBackStack mvxPopBackStack) => mvxPopBackStack switch
        {
            PopBackStack.None => PopBackStackFlags.None,
            PopBackStack.Inclusive => PopBackStackFlags.Inclusive,
            _ => throw new ArgumentOutOfRangeException(nameof(mvxPopBackStack), mvxPopBackStack, $"No matching {nameof(PopBackStackFlags)} enum is defined"),
        };
    }
}