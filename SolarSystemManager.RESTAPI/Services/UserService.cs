using Microsoft.AspNetCore.Identity.Data;
using SolarSystemManager.RESTAPI.Entities;

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
        public bool ValidateUser(Entities.LoginRequest cred)
        {
            try
            {
                //replace with actual login logic
                //just need to check if the credentials are in our list of users
                if (cred.username == "admin" && cred.password == "admin")
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
