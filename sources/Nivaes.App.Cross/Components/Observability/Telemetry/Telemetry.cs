using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Text;

namespace Nivaes.App.Cross.Observability;

public static class Telemetry
{
    public static readonly ActivitySource ActivitySource =
        new("MyMauiApp");

    public static readonly Meter Meter =
        new("MyMauiApp");



    public static readonly Counter<long> ButtonClicks =
        Meter.CreateCounter<long>("button_clicks");


    public static readonly Histogram<double> OperationDuration =
        Meter.CreateHistogram<double>(
            "operation_duration_ms");
}
