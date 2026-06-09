using System;
using System.Collections.Generic;
using System.Text;
using Android.Runtime;
using Java.Lang;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Droid
{
    public class AndroidCrashHandler : CrashHandler
    {
        public AndroidCrashHandler(ILogger logger)
            : base(logger)
        {
        }

        public override void Register()
        {
            base.Register();
            // Excepciones en código Java
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
        }

        private void AndroidEnvironment_UnhandledExceptionRaiser(
            object? sender,
            RaiseThrowableEventArgs e)
        {
            var ex = e.Exception;

            base.Logger.LogCritical(ex, "Unhandled Java exception occurred.");

            // Indica que la excepción ha sido manejada
            e.Handled = true;
        }
    }
}
