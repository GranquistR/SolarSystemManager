﻿using Microsoft.AspNetCore.Identity.Data;
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
            try
            {
                // Send SQL request and cast returned data to correct type 
                List<User> users = _baseRepo.GetData("SELECT UserID,Username,Password,Role FROM User").Cast<User>().ToList();
            
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
                // Send SQL request and cast returned data to correct type 
                List<User> users = _baseRepo.GetData("SELECT UserID,Username,Password,Role FROM User").Cast<User>().ToList();
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
