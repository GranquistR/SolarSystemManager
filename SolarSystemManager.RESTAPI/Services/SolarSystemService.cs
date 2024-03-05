﻿using SolarSystemManager.RESTAPI.Entities;
using SolarSystemManager.RESTAPI.Repos;
using static SolarSystemManager.RESTAPI.Entities.SolarSystem;

namespace SolarSystemManager.RESTAPI.Services
{
    public class SolarSystemService
    {
        BaseRepo _baseRepo = BaseRepo.Instance();
        UserService UserService = new UserService();

        public IEnumerable<SolarSystem> GetAllPublicSolarSystems()
        {
            return _baseRepo.GetAllSolarSystems().Where(s => s.systemVisibility == Visibility.Public);
        }

        public IEnumerable<SolarSystem> GetMySolarSystems(LoginRequest cred)
        {
            var user = UserService.ValidateUser(cred);
            if(user == null)
            {
                throw new BadHttpRequestException("401");
            }
            return _baseRepo.GetAllSolarSystems().Where(s => s.ownerId == user.userID);
        }

        public IEnumerable<SolarSystem> GetAllSolarSystemsAdmin(LoginRequest cred)
        {
            var user = UserService.ValidateUser(cred);
            if(user == null)
            {
                throw new BadHttpRequestException("401");
            }
            else
            {
                if(user.role == Role.Admin)
                {
                    return _baseRepo.GetAllSolarSystems();
                }
                else 
                { 
                    throw new BadHttpRequestException("403");
                }
            }
            
        }   

        public bool DeleteSolarSystem(int id)
        {
            _baseRepo.DeleteSolarSystem(id);
            return true;
        }

        public int SolarSystemCount()
        {
            return _baseRepo.Count("SolarSystem");
        }

        public int SpaceObjectCount()
        {
            return _baseRepo.Count("SpaceObject");
        }
    }
}
