using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace DigitalWill.WortalSDK
{
	/// <summary>
	/// Conversion extensions for JObjects. Converts JObject to collections recursively.
	/// </summary>
    public static class JsonConversionExtensions
    {
        // See:
        // https://stackoverflow.com/a/39647961/14580686
        //

        public static IDictionary<string, object> ToDictionary(this JObject json)
        {
            Dictionary<string, object> propertyValuePairs = json.ToObject<Dictionary<string, object>>();
            ProcessJObjectProperties(propertyValuePairs);
            ProcessJArrayProperties(propertyValuePairs);
            return propertyValuePairs;
        }

        public static object[] ToArray(this JArray array)
        {
            return (array.ToObject<object[]>() ?? throw new InvalidOperationException("JArray was null in conversion."))
                   .Select(ProcessArrayEntry).ToArray();
        }

        private static void ProcessJObjectProperties(IDictionary<string, object> propertyValuePairs)
        {
            List<string> objectPropertyNames = (from property in propertyValuePairs
                let propertyName = property.Key
                let value = property.Value
                where value is JObject
                select propertyName).ToList();

            objectPropertyNames.ForEach(propertyName => propertyValuePairs[propertyName] = ToDictionary((JObject) propertyValuePairs[propertyName]));
        }

        private static void ProcessJArrayProperties(IDictionary<string, object> propertyValuePairs)
        {
            List<string> arrayPropertyNames = (from property in propertyValuePairs
                let propertyName = property.Key
                let value = property.Value
                where value is JArray
                select propertyName).ToList();

            arrayPropertyNames.ForEach(propertyName => propertyValuePairs[propertyName] = ToArray((JArray) propertyValuePairs[propertyName]));
        }

        private static object ProcessArrayEntry(object value)
        {
            return value switch
            {
                JObject jObject => ToDictionary(jObject),
                JArray array => ToArray(array),
                _ => value,
            };
        }
    }
}
