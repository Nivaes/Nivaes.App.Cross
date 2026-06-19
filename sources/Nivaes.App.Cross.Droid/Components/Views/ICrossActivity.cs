using System;
using System.Collections.Generic;
using System.Text;

namespace Nivaes.App.Cross.Droid
{
    public interface ICrossActivity
    {
        global::AndroidX.Fragment.App.FragmentManager SupportFragmentManager { get; }
    }
}
