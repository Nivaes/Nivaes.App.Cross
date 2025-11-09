// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace Nivaes.App.Cross.Droid
{
    [Register("mvvmcross.platforms.android.binding.views.MvxDatePicker")]
    public class CrossDatePicker
        : DatePicker
        , DatePicker.IOnDateChangedListener
    {
        private bool _initialized;

        public CrossDatePicker(Context context)
            : base(context)
        {
        }

        public CrossDatePicker(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
        }

        public CrossDatePicker(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {
        }

        public CrossDatePicker(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes)
            : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        protected CrossDatePicker(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public DateTime Value
        {
            get
            {
                return CrossJavaDateUtils.DateTimeFromJava(Year, Month, DayOfMonth);
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

        public event EventHandler ValueChanged;

        public void OnDateChanged(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            ValueChanged?.Invoke(this, null);
        }
    }
}
