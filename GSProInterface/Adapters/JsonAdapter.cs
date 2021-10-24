using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSProInterface.Adapters
{
    public class JsonAdapter
    {
        public static string ToJsonString(object payload)
        {
            string jsonIgnoreNullValues = JsonConvert.SerializeObject(payload, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            return jsonIgnoreNullValues;
        }

        public static T JsonStringTo<T>(string payload)
        {
            return JsonConvert.DeserializeObject<T>(payload);

        }
    }
}
