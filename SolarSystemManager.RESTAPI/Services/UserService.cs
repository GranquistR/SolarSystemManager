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
        public string GetSalty(string username)
        {
            try
            {
                var users = _baseRepo.GetAllUsers();
                var user = FindUserByUsername(users, username);
                if (user != default)
                {
                    return user.salt;
                }
                else
                {
                    throw new BadHttpRequestException("Invalid user!");
                }
            }
            catch
            {
                return "Unable to connect to Database!";
            }
        }

        private User FindUserByUsername(List<User> users, string username)
        {
            return users.Find(p => p.username == username);
        }

        public User? ValidateUser(Entities.LoginRequest cred)
        {
            var users = _baseRepo.GetAllUsers();
            return users.Find(p => (p.username == cred.username) && (p.password == cred.password));           
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

        public void CreateAccount(Entities.User newAccount)
        {
            if (_baseRepo.GetAllUsers().Any(p => p.username == newAccount.username))
            {
                throw new BadHttpRequestException("Username already exists");
            }
            if(newAccount.username.Length < 1)
            {
                throw new BadHttpRequestException("Username too short!");

            }
            if(newAccount.password.Length < 7)
            {
                throw new BadHttpRequestException("Password too short, needs at least 8 characters!");
            }

            _baseRepo.CreateUser(new User { username = newAccount.username, password = newAccount.password, role = Role.Member, salt = newAccount.salt });
            
        }

        public int UserCount()
        {
            return _baseRepo.Count("User");
        }

        public void ChangeUserName(Entities.ChangeUsernameRequest ncred)
        {
            //Entities.LoginRequest cred = new Entities.LoginRequest(ncred.username, ncred.password);

            User? temp = ValidateUser(new Entities.LoginRequest(ncred.username, ncred.password));

            if(temp != null)
            {
                temp.username = ncred.newUsername;
                _baseRepo.ModifyUser(temp);
            }
            else
            {
                throw new BadHttpRequestException("No user found with given credentials");
            }
        }

        public void ChangePassword(Entities.ChangeUsernameRequest ncred)
        {
            //Entities.LoginRequest cred = new Entities.LoginRequest(ncred.username, ncred.password);

            User? temp = ValidateUser(new Entities.LoginRequest(ncred.username, ncred.password)); 

            if (temp != null)
            {
                temp.password = ncred.newUsername;
                _baseRepo.ModifyUser(temp);
            }
            else
            {
                throw new BadHttpRequestException("No user found with given credentials");
            }
        }
    }
}
