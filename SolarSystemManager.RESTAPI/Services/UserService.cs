using Microsoft.AspNetCore.Identity.Data;
using SolarSystemManager.RESTAPI.Entities;
using System.Linq;

namespace SolarSystemManager.RESTAPI.Services
{
    public class UserService
    {
        /// <summary>
        /// This validates the user
        /// Will be replaced with actual login logic
        /// WIll be used to validate the user on every endpoint that requires authentication
        /// </summary>
        /// <param name="cred"></param>
        /// <returns></returns>
        ///         

        List<User> users = new List<User> {
                new User
                {
                    userID = 1234,
                    username = "user",
                    password = "password"
                },

                new User
                {
                    userID = 4321,
                    username = "admin",
                    password = "admin"
                }
        };
        public bool ValidateUser(Entities.LoginRequest cred)
        {
            try
            {
                //replace with actual login logic
                //just need to check if the credentials are in our list of users
                if (users.Any(p => p.username == cred.username) && users.Any(p => p.password == cred.password))
                {
                    return true;
                }
                else
                {
                    throw new BadHttpRequestException("Invalid username or password");
                }
            }
            catch
            {
                return false;
            }
            
        } 
    }
}
