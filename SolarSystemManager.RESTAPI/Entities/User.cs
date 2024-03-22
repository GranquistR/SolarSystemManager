using System.Text.Json.Serialization;
using SolarSystemManager.RESTAPI.Repos;

namespace SolarSystemManager.RESTAPI.Entities
{
    public class User
    {
        public int userID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public Role role { get; set; }
        public string salt { get; set; }
    }

    public enum Role
    {
        Member=0,
        Admin=1
    }

    public class LoginRequest
    {

        [JsonPropertyName("username")]
        public string username { get; set; }

        [JsonPropertyName("password")]
        public string password { get; set; }

    }
}
