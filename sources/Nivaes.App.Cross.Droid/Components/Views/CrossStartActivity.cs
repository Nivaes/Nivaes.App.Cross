using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nivaes.App.Cross.Droid
{
    public abstract class CrossStartActivity : Activity
    {
        private readonly int _resourceId;

        protected CrossStartActivity(int resourceId)
        {
            _resourceId = resourceId;
        }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var content = LayoutInflater.Inflate(_resourceId, null);
            SetContentView(content);
        }
    }
}
