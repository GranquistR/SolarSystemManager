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
        public User? ValidateUser(Entities.LoginRequest cred)
        {
            return _baseRepo.GetAllUsers().FirstOrDefault(p => (p.username == cred.username) && (p.password == cred.password));
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

        public void CreateAccount(Entities.LoginRequest newAccount)
        {
            if (_baseRepo.GetAllUsers().Any(p => p.username == newAccount.username))
            {
                throw new BadHttpRequestException("Username already exists");
            }
            if(newAccount.username.Length < 1)
            {
                throw new BadHttpRequestException("Username too short!");

            }
            if(newAccount.password.Length < 1)
            {
                throw new BadHttpRequestException("Password too short!");
            }

            _baseRepo.CreateUser(new User { username = newAccount.username, password = newAccount.password, role = Role.Member });
            
        }

        public int UserCount()
        {
            return _baseRepo.Count("User");
        }
    }
}
