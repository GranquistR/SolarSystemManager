using System.Text.Json.Serialization;

namespace SolarSystemManager.RESTAPI.Entities
{
    public class Response
    {
        [JsonPropertyName("message")]
        public string message { get; set; }

        [JsonPropertyName("success")]
        public bool success { get; set; }

        [JsonPropertyName("status")]
        public int status { get; set; }

        public Object? data { get; set; }
    }
}
