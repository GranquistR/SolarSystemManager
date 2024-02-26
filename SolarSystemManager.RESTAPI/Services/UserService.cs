using Microsoft.AspNetCore.Identity.Data;
using SolarSystemManager.RESTAPI.Entities;
using System.Linq;
using System.Text.Json;

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

        //List<User> users1 = [
        //        new User
        //        {
        //            userID = 1234,
        //            username = "user",
        //            password = "password",
        //            settings =
        //                "{\"role\":\"member\",\"contrast\":\"1\"}"
        //        },

        //    new User
        //    {
        //        userID = 4321,
        //        username = "admin",
        //        password = "admin",
        //        settings =
        //                "{\"role\":\"administrator\",\"contrast\":\"1\"}"

        //    }
        //];

        SQLService _sqlService = new SQLService();
        public bool ValidateUser(Entities.LoginRequest cred)
        {
            try
            {
                List<User> users = _sqlService.GetUsers();
                //replace with actual login logic
                //just need to check if the credentials are in our list of users
                if (users.Any(p => (p.username == cred.username) && (p.password == cred.password)))
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
        public string GetUserData(Entities.LoginRequest cred)
        {
            try
            {
                List<User> users = _sqlService.GetUsers();
                //check Credentials of user
                User cUser = users.First(p => (p.username == cred.username) && (p.password == cred.password));

                if (cUser != null)
                {
                    return JsonSerializer.Serialize(cUser); 
                }
                else
                {
                    throw new BadHttpRequestException("Invalid username or password");
                }
            }
            catch
            {
                return "0";
            }
        }
    }
}
