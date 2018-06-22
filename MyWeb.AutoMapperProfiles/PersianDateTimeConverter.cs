using System;
using AutoMapper;
using MyWeb.Utility;

namespace MyWeb.AutoMapperProfiles
{
    public class ToPersianDateTimeConverter : ITypeConverter<DateTime, string>
    {
        private readonly bool _fullDateTime;

        public ToPersianDateTimeConverter(bool fullDateTime = true)
        {
            _fullDateTime = fullDateTime;
        }

        public string Convert(ResolutionContext context)
        {
            var dateTime = context.SourceValue;
            if (dateTime == null) return string.Empty;
            var persianDateTime = new PersianDateTime((DateTime)dateTime);

            return _fullDateTime
                ? $"{persianDateTime.ToString("d MMMM yyyy HH:mm")} ({RemainingDateTime.Calculate((DateTime) dateTime)})"
                : $"{persianDateTime.ToString(PersianDateTimeFormat.ShortDateShortTime)}, ({RemainingDateTime.Calculate((DateTime) dateTime)})";
        }


    }
}
