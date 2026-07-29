using System;
using System.Collections.Generic;
using System.Linq;

namespace Nivaes.App
{
    public class ValidateProperty
        : Model
    {
        private IEnumerable<string> mErrors = Array.Empty<string>();

        public IEnumerable<string> Errors
        {
            get => mErrors;
            set
            {
                if (base.SetProperty(ref mErrors, value))
                {
                    base.RaisePropertyChanged(nameof(IsValid));
                }
            }
        }

        public bool IsValid => !mErrors.Any();
    }
}
