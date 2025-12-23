namespace Nivaes.App.Cross.Droid
{
    using System;
    using Android.Content;
    using Android.Runtime;
    using Android.Util;
    using Android.Widget;
    using MvvmCross.Platforms.Android;

    [Register("mvvmcross.platforms.android.binding.views.MvxDatePicker")]
    public class MvxDatePicker
        : DatePicker
        , DatePicker.IOnDateChangedListener
    {
        private bool _initialized;

        public MvxDatePicker(Context context)
            : base(context)
        {
        }

        public MvxDatePicker(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
        }

        public MvxDatePicker(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {
        }

        public MvxDatePicker(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes)
            : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        protected MvxDatePicker(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public DateTime Value
        {
            get
            {
                return MvxJavaDateUtils.DateTimeFromJava(Year, Month, DayOfMonth);
            }
            set
            {
                var javaYear = value.Year;
                // -1 needed as Java date months are zero-based
                var javaMonth = value.Month - 1;
                var javaDay = value.Day;

                if (!_initialized)
                {
                    Init(javaYear, javaMonth, javaDay, this);
                    _initialized = true;
                }
                else if (Year != javaYear || Month != javaMonth || DayOfMonth != javaDay)
                {
                    UpdateDate(javaYear, javaMonth, javaDay);
                }
            }
        }

        public event EventHandler? ValueChanged;

        public void OnDateChanged(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            ValueChanged?.Invoke(this, null);
        }
    }
}
