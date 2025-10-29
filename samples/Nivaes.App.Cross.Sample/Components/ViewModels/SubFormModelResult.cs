namespace Nivaes.App.Cross.Sample
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SubFormModelResult 
        : Model
    {
        public string? mStringValue;

        public string? StringValue
        {
            [DebuggerStepThrough]
            get => mStringValue;
            [DebuggerStepThrough]
            set
            {
                if (mStringValue != value)
                {
                    mStringValue = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
