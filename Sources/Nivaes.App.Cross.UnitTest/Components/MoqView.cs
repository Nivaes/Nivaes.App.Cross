using System;
using System.Collections.Generic;
using System.Text;

namespace Nivaes.App.Cross.UnitTest.Components
{
    class MoqView : ICrossView<MoqViewModel>
    {
        public MoqViewModel? ViewModel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public object? DataContext { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        ICrossViewModel? ICrossView.ViewModel { get => ViewModel; set => throw new NotImplementedException(); }
    }
}
