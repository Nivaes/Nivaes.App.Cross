#if IOS || MACCATALYST
namespace Nivaes.App.Cross.UIKit
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    public class MvxUIDatePickerMinMaxTargetBinding
    : MvxBaseUIDatePickerTargetBinding
    {
        public MvxUIDatePickerMinMaxTargetBinding(UIDatePicker target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var targetPropertyName = targetPropertyInfo.Name;
            if (targetPropertyName == nameof(UIDatePicker.Date))
                throw new ArgumentException("This binding cannot be used with the Date property as the target.");
        }

        protected override object GetValueFrom(UIDatePicker view)
        {
            // This method should never be called.
            throw new NotImplementedException();
        }

        [RequiresUnreferencedCode("This method may perform type conversions which may not be preserved by trimming")]
        protected override object MakeSafeValue(object? value)
        {
            if (value == null)
                return NSDate.FromTimeIntervalSince1970(0);

            var valueUtc = ToUtcTime((DateTime)value);
            var valueNSDate = valueUtc.ToNSDate();

            return valueNSDate;
        }
    }
}
#endif