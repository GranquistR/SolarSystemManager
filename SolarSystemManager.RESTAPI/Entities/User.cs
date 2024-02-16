using System.Text.Json.Serialization;

namespace SolarSystemManager.RESTAPI.Entities
{
    public class User
    {
        public int userID { get; set; }
        public string username { get; set; }
        public string password { get; set; }

    }

    public class LoginRequest
    {
        [JsonPropertyName("username")]
        public string username { get; set; }

        [JsonPropertyName("password")]
        public string password { get; set; }
    }
}
