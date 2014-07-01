namespace IORPromoteTool.Models
{
    using System;

    public class DateKindHelper
    {
        public static DateTime DefaultToUtc(DateTime date)
        {
            return date.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(date, DateTimeKind.Utc) : date;
        }

        public static DateTime? DefaultToUtc(DateTime? date)
        {
            return date.HasValue && date.Value.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(date.Value, DateTimeKind.Utc) : date;
        }

    }
}
