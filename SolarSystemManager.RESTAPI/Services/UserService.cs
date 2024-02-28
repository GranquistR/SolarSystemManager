using Microsoft.AspNetCore.Identity.Data;
using SolarSystemManager.RESTAPI.Entities;
using System.Linq;
using System.Text.Json;
using SolarSystemManager.RESTAPI.Repos;


namespace SolarSystemManager.RESTAPI.Services
{
    public class UserService
    {
        
        BaseRepo _baseRepo = BaseRepo.Instance();
        public bool ValidateUser(Entities.LoginRequest cred)
        {
            return _baseRepo.GetAllUsers().Any(p => (p.username == cred.username) && (p.password == cred.password));
        }
        public User GetUserSettingsData(Entities.LoginRequest cred)
        {
            var user = _baseRepo.GetAllUsers().FirstOrDefault(p => (p.username == cred.username) && (p.password == cred.password));

            if (user != default)
            {
                return user;
            }
            else
            {
               throw new BadHttpRequestException("No user found with given credentials");
            }   
        }
    }
}
