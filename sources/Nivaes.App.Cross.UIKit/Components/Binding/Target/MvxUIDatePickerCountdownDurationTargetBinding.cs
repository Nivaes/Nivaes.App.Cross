#if IOS || MACCATALYST
namespace Nivaes.App.Cross.UIKitOS
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
 
    public class MvxUIDatePickerCountDownDurationTargetBinding(UIDatePicker target, PropertyInfo targetPropertyInfo)
        : MvxBaseUIDatePickerTargetBinding(target, targetPropertyInfo)
    {
        protected override object GetValueFrom(UIDatePicker view)
        {
            return view.CountDownDuration;
        }

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(double);
    }
}
#endif