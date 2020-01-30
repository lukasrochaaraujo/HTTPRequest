using System;
using System.Collections.Generic;
using System.Dynamic;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HTTPRequest.Exceptions
{
    public class HttpException : Exception
    {
        public dynamic HttpResponseDynamic;

        public HttpException(string responseJson, string message) : base(message)
        {
            HttpResponseDynamic = ToDynamicObject(responseJson);
        }

        public HttpException(string responseJson, string message, Exception innerException) : base(message, innerException)
        {
            HttpResponseDynamic = ToDynamicObject(responseJson);
        }

        private dynamic ToDynamicObject(string json)
        {
            return (dynamic)JsonConvert.DeserializeObject<List<ExpandoObject>>(json, new ExpandoObjectConverter());
        }
    }
}
