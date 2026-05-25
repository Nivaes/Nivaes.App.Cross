using Android.Content;
using Microsoft.Extensions.DependencyInjection;

namespace Nivaes.App.Cross.Droid
{
    public class CrossAndroidContext :
        CrossContext, ICrossAndroidContext, ICrossContext
    {
        readonly Lazy<Context?> _context;

        public Context? Context => _context.Value;

        public CrossAndroidContext(IServiceProvider services, Context context)
            : this(services)
        {
            AddWeakSpecific(context);
        }

        public CrossAndroidContext(IServiceProvider services)
                :base(services)
        {
            _context = new Lazy<Context?>(() => _services.GetService<Context>());
        }
    }
}
