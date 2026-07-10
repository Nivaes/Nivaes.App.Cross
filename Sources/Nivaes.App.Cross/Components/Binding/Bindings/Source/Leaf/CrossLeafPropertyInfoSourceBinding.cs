using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public abstract class CrossLeafPropertyInfoSourceBinding 
        : CrossPropertyInfoSourceBinding
    {
        protected CrossLeafPropertyInfoSourceBinding(object source, PropertyInfo propertyInfo)
            : base(source, propertyInfo)
        {
        }

        public override Type SourceType => PropertyInfo.PropertyType;

        protected override void OnBoundPropertyChanged()
        {
            FireChanged();
        }

        public override object? GetValue()
        {
            if (PropertyInfo == null)
            {
                return CrossBindingConstant.UnsetValue;
            }

            if (!PropertyInfo.CanRead)
            {
                CrossBindingLogger.GetLogger<CrossLeafPropertyInfoSourceBinding>().LogError(
                    "GetValue ignored in binding - target property {PropertyTypeName}.{PropertyName} is writeonly",
                    PropertyInfo.DeclaringType?.Name, PropertyName);
                return CrossBindingConstant.UnsetValue;
            }

            try
            {
                return PropertyInfo.GetValue(Source, PropertyIndexParameters());
            }
            catch (TargetInvocationException)
            {
                // for dictionary lookups we quite often expect this during binding
                // for list-based lookups we quite often expect this during binding
                return CrossBindingConstant.UnsetValue;
            }
        }

        protected abstract object[] PropertyIndexParameters();

        public override void SetValue(object value)
        {
            if (PropertyInfo == null)
            {
                CrossBindingLogger.GetLogger<CrossLeafPropertyInfoSourceBinding>().LogWarning("SetValue ignored in binding - source property {PropertyName} is missing", PropertyName);
                return;
            }

            if (!PropertyInfo.CanWrite)
            {
                CrossBindingLogger.GetLogger<CrossLeafPropertyInfoSourceBinding>().LogWarning(
                    "SetValue ignored in binding - target property {PropertyTypeName}.{PropertyName} is readonly",
                    PropertyInfo.DeclaringType?.Name, PropertyName);
                return;
            }

            try
            {
                var propertyType = PropertyInfo.PropertyType;
                var safeValue = propertyType.MakeSafeValue(value);

                // if safeValue matches the existing value, then don't call set
                if (EqualsCurrentValue(safeValue))
                    return;

                PropertyInfo.SetValue(Source, safeValue, PropertyIndexParameters());
            }
            catch (Exception exception)
            {
                CrossBindingLogger.GetLogger<CrossLeafPropertyInfoSourceBinding>()?.LogError(exception, "SetValue failed with exception. Property Name: {PropertyName}, Value: {Value}", PropertyName, value);
            }
        }
    }
}
