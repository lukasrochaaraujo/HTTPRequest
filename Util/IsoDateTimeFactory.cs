﻿using Newtonsoft.Json.Converters;

using HTTPRequest.Enum;

namespace HTTPRequest.Util
{
    public static class IsoDateTimeFactory
    {
        public static IsoDateTimeConverter Create(IsoDateTimeType dateTimeFormat)
        {
            var isoDate = new IsoDateTimeConverter();
            switch (dateTimeFormat)
            {
                case IsoDateTimeType.DOTNET:
                case IsoDateTimeType.SPRING:
                default:
                    isoDate.DateTimeFormat = "yyyy-MM-ddTHH:mm:ss";
                    break;
            }
            return isoDate;
        }
    }
}