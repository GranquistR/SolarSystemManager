using System.Text.Json.Serialization;

namespace SolarSystemManager.RESTAPI.Entities
{
    public class User
    {
        int userID { get; set; }
        string username { get; set; }
        string password { get; set; }

    }

    public class LoginRequest
    {
        [JsonPropertyName("username")]
        public string username { get; set; }

        [JsonPropertyName("password")]
        public string password { get; set; }
    }
}
