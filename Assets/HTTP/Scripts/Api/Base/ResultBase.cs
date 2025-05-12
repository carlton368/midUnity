using Newtonsoft.Json;

namespace HTTP
{
    public class ResultBase
    {
        public bool status;
        public string message;
        public string error;

        private static readonly JsonSerializerSettings SerializerSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented,
        };

        private static readonly JsonSerializerSettings DeserializerSettings = new()
        {
            MissingMemberHandling = MissingMemberHandling.Error,
        };
        
        public static T FromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, DeserializerSettings);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, SerializerSettings);
        }
    }
}