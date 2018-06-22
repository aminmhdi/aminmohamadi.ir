using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MyWeb.Utility
{
    public static class JsonConvertor
    {
        public static string SerializeWithJsonFormatter(IDictionary<string, object> data)
        {
            if (data == null || data.Keys.Count == 0) return null;

            return JsonConvert.SerializeObject(data, Formatting.Indented);
        }

        public static IDictionary<string, object> DeserializeWithJsonFormatter(string data)
        {
            return string.IsNullOrEmpty(data) ? null : JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
        }

        public static BaseResultOfanyType DeserializeSendingSMSResult(string response)
        {
            try
            {
                return JsonConvert.DeserializeObject<BaseResultOfanyType>(response);
            }
            catch
            {
                return ((BaseResultOfanyType)Activator.CreateInstance(typeof(BaseResultOfanyType)));
            }
        }
    }

    public class BaseResultOfanyType
    {
        public object Data { get; set; }

        public object ReasonPhrase { get; set; }

        public int ResultStatusCode { get; set; }
    }
}
