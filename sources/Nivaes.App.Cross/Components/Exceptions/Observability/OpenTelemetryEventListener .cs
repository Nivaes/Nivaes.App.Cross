using System;
using System.Collections.Generic;
using System.Text;

namespace Nivaes.App.Cross
{
    using System.Diagnostics.Tracing;

    public sealed class OpenTelemetryEventListener : EventListener
    {
        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            if (eventSource.Name.StartsWith("OpenTelemetry"))
            {
                EnableEvents(
                    eventSource,
                    EventLevel.Verbose);
            }
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            var payload = eventData.Payload == null
                ? string.Empty
                : string.Join(", ", eventData.Payload);

            Console.WriteLine(
                $"[{eventData.EventSource.Name}] {eventData.EventName}: {payload}");
        }
    }
}
