namespace Nivaes.App.Cross.UIKit
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    public class CrossUIDatePickerCountdownDurationTargetBinding(UIDatePicker target, PropertyInfo targetPropertyInfo)
        : CrossBaseUIDatePickerTargetBinding(target, targetPropertyInfo)
    {
        protected override object GetValueFrom(UIDatePicker view)
        {
            return view.CountDownDuration;
        }

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(double);
    }
}