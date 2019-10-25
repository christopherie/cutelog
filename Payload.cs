using Newtonsoft.Json;

namespace CuteLog
{
    public class Payload
    {
        [JsonProperty("text")]
        public string AlertMessage { get; set; }

        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }
}
