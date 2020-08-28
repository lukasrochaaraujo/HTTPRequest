using Newtonsoft.Json.Converters;

using GSD.HTTPRequest.Enum;

namespace GSD.HTTPRequest.Util
{
    public static class IsoDateTimeFactory
    {
        public static IsoDateTimeConverter Create(IsoDateTimeType dateTimeFormat)
        {
            var isoDate = new IsoDateTimeConverter();
            switch (dateTimeFormat)
            {
                case IsoDateTimeType.ISO8601:
                default:
                    isoDate.DateTimeFormat = "yyyy-MM-ddTHH:mm:ss";
                    break;
            }
            return isoDate;
        }
    }
}
