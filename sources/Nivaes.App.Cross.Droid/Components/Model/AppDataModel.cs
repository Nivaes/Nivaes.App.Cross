namespace Nivaes.App.Cross.Droid
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Android.Content;

    public sealed class AppDataModel
    {
        private readonly Context _applicationContext;

        public Context ApplicationContext => _applicationContext;

        public AppDataModel(Context applicationContext)
        {
            this._applicationContext = applicationContext;
        }
    }
}
