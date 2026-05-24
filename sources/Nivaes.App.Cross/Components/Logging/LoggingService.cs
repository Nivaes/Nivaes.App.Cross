using System;
using System.Collections.Generic;
using System.Text;

namespace Nivaes.App.Cross;

[Obsolete]
internal class LoggingService : ILoggingService
{
    public LoggingService()
    {
        using var sentry = SentrySdk.Init(options =>
        {
            // A Sentry Data Source Name (DSN) is required.
            // See https://docs.sentry.io/concepts/key-terms/dsn-explainer/
            // You can set it in the SENTRY_DSN environment variable, or you can set it in code here.
            options.Dsn = Nivaes.App.Cross.Secrets.SentryDns;
            // When debug is enabled, the Sentry client will emit detailed debugging information to the console.
            // This might be helpful, or might interfere with the normal operation of your application.
            // We enable it here for demonstration purposes when first trying Sentry.
            // You shouldn't do this in your applications unless you're troubleshooting issues with Sentry.
#if DEBUG
            options.Debug = true;
#else
            options.Debug = false;
#endif
            // Adds request URL and headers, IP and name for users, etc.
            options.SendDefaultPii = true;
            // This option is recommended. It enables Sentry's "Release Health" feature.
            options.AutoSessionTracking = true;
            // Enabling this option is recommended for client applications only. It ensures all threads use the same global scope.
            options.IsGlobalModeEnabled = false;
            // Example sample rate for your transactions: captures 10% of transactions
            options.TracesSampleRate = 1.0; // 0.1

            // Enable logs to be sent to Sentry
            options.EnableLogs = true;

            // Sample rate for profiling, applied on top of othe TracesSampleRate,
            // e.g. 0.2 means we want to profile 20 % of the captured transactions.
            // We recommend adjusting this value in production.
            options.ProfilesSampleRate = 1.0;
        // Requires NuGet package: Sentry.Profiling
        // Note: By default, the profiler is initialized asynchronously. This can
        // be tuned by passing a desired initialization timeout to the constructor.
        //options.AddIntegration(new ProfilingIntegration(
        //    // During startup, wait up to 500ms to profile the app startup code.
        //    // This could make launching the app a bit slower so comment it out if you
        //    // prefer profiling to start asynchronously
        //    TimeSpan.FromMilliseconds(500)
        //    ));
        });
    }
}
