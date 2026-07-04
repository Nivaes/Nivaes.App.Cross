namespace Nivaes.App.Cross.UIKitLib
{
    using System;
    using Foundation;

    public static class MvxIosDateTimeExtensions
    {
        private static readonly DateTime ReferenceNSDateTime = new DateTime(2001, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        extension(NSDate date)
        {
            public DateTime ToDateTimeUtc()
            {
                return ReferenceNSDateTime.AddSeconds(date.SecondsSinceReferenceDate);
            }
        }

        extension(DateTime date)
        {
            public NSDate ToNSDate()
            {
                return NSDate.FromTimeIntervalSinceReferenceDate((date - ReferenceNSDateTime).TotalSeconds);
            }

            public DateTime WithKind(DateTimeKind kind)
            {
                return new DateTime(date.Ticks, kind);
            }
        }
    }
}
