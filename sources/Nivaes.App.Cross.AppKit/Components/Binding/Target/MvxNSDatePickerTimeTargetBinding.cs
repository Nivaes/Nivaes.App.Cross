namespace Nivaes.App.Cross.AppKit
{
    using System;
    using AppKit;
    using Foundation;
    using MvvmCross.Platforms.Mac.Binding.Target;

    public class MvxNSDatePickerTimeTargetBinding 
        : MvxBaseNSDatePickerTargetBinding
    {
        public MvxNSDatePickerTimeTargetBinding(NSDatePicker datePicker)
            : base(datePicker)
        {
        }

        protected override void SetValueImpl(object target, object? value)
        {
            var picker = this.DatePicker;
            if (picker == null)
                return;

            var time = value as DateTime?;

            var pickerDate = this.DatePicker != null ? this.GetLocalTime(this.DatePicker) : DateTime.Now;

            DateTime date;
            if (time == null)
            {
                date = new DateTime(pickerDate.Year, pickerDate.Month, pickerDate.Day,
                    0, 0, 0, DateTimeKind.Local);
            }
            else
            {
                date = new DateTime(pickerDate.Year, pickerDate.Month, pickerDate.Day,
                    time.Value.Hour, time.Value.Minute, time.Value.Second, DateTimeKind.Local);
            }

            //var date = new DateTime (2000, 1, 1).Add (timespan);
            picker.DateValue = (NSDate)(date);
        }

        protected override object GetValueFrom(NSDatePicker view)
        {
            var date = this.GetLocalTime(view);
            return date.TimeOfDay;

            //			var components = NSCalendar.CurrentCalendar.Components(
            //				NSCalendarUnit.Hour | NSCalendarUnit.Minute | NSCalendarUnit.Second,
            //				view.DateValue);
            //            return new TimeSpan((int)components.Hour, (int)components.Minute, (int)components.Second);
        }

        [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType
        {
            get { return typeof(TimeSpan); }
        }

        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("This method may perform type conversions which may not be preserved by trimming")]
        protected override object MakeSafeValue(object? value)
        {
            if (value == null)
                value = TimeSpan.FromSeconds(0);
            var time = (TimeSpan)value;
            var now = DateTime.Now;
            var date = new DateTime(
                2000,
                1,
                1,
                time.Hours,
                time.Minutes,
                time.Seconds,
                DateTimeKind.Local);

            return date;
        }
    }
}
