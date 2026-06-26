namespace Nivaes.App.Cross.AppKitOS
{
    using System;
    using Microsoft.Extensions.Logging;

    public abstract class MvxBaseNSDatePickerTargetBinding
        : MvxMacTargetBinding
    {
        private bool _subscribed;

        protected NSDatePicker? DatePicker
        {
            get { return base.Target as NSDatePicker; }
        }

        protected MvxBaseNSDatePickerTargetBinding(NSDatePicker target)
            : base(target)
        {
        }

        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public override void SubscribeToEvents()
        {
            var datePicker = this.DatePicker;

            if (datePicker == null)
            {
                CrossBindingLogger.Instance?.LogError(
                                      "NSDatePicker is null in MvxBaseNSDatePickerTargetBinding");
                return;
            }
            datePicker.Activated += HandleActivated;
            this._subscribed = true;
        }

        private void HandleActivated(object? sender, EventArgs e)
        {
            var view = this.DatePicker;
            if (view == null)
                return;
            FireValueChanged(this.GetValueFrom(view));
        }

        protected abstract object GetValueFrom(NSDatePicker view);

        protected DateTime GetLocalTime(NSDatePicker view)
        {
            var tzInfo = TimeZoneInfo.Local;
            return TimeZoneInfo.ConvertTimeFromUtc((DateTime)view.DateValue, tzInfo);
        }

        public override CrossBindingMode DefaultMode
        {
            get { return CrossBindingMode.TwoWay; }
        }

        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var datePicker = this.DatePicker;
                if (datePicker != null && this._subscribed)
                {
                    datePicker.Activated -= HandleActivated;
                    this._subscribed = false;
                }
            }
        }
    }
}
