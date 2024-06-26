﻿using SolarSystemManager.RESTAPI.Entities;
using SolarSystemManager.RESTAPI.Repos;
using static SolarSystemManager.RESTAPI.Entities.SolarSystem;
using static SolarSystemManager.RESTAPI.Entities.SpaceObject;

namespace SolarSystemManager.RESTAPI.Services
{
    public class SolarSystemService
    {
        BaseRepo _baseRepo = BaseRepo.Instance();
        UserService _userService = new UserService();

        public IEnumerable<SolarSystem> GetAllPublicSolarSystems()
        {
            return _baseRepo.GetAllSolarSystems().Where(s => s.systemVisibility == Visibility.Public);
        }

        public IEnumerable<SolarSystem> GetMySolarSystems(LoginRequest cred)
        {
            var user = _userService.ValidateUser(cred);
            if(user == null)
            {
                throw new BadHttpRequestException("401");
            }
            return _baseRepo.GetAllSolarSystems().Where(s => s.ownerId == user.userID);
        }

        public IEnumerable<SolarSystem> GetAllSolarSystemsAdmin(LoginRequest cred)
        {
            var user = _userService.ValidateUser(cred);
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

        public bool DeleteSolarSystem(Entities.DleteSolarSystemRequest dcred)
        {
            User? temp = _userService.ValidateUser(new Entities.LoginRequest(dcred.username, dcred.password));

            if (temp != null)
            {
                //temp.dSolaySystemID = dcred.dSolarSystemID;
                _baseRepo.DeleteSolarSystem(dcred.dSolarSystemID);
                return true;
            }
            else
            {
                throw new BadHttpRequestException("No Solar System Found");
            }
        }

        public bool DeleteSpaceObject(Entities.LoginRequest cred, int id)
        {
            User? user = _userService.ValidateUser(cred) ?? throw new BadHttpRequestException("401");

            SpaceObject spaceObject = _baseRepo.GetSpaceOBjectById(id) ?? throw new BadHttpRequestException("400");

            SolarSystem system = _baseRepo.GetSolarSystemByID(spaceObject.solarSystemID) ?? throw new BadHttpRequestException("400");

            int ownerId = system.ownerId;

            if (user.userID == ownerId || user.role == Role.Admin)
            {
                _baseRepo.DeleteSpaceObject(id);
                return true;
            }
            throw new BadHttpRequestException("403");
        }

        public int CreateSolarSystem(NewSolarSystem newSystem,Entities.LoginRequest cred)
        {
            User? temp = _userService.ValidateUser(cred) ?? throw new BadHttpRequestException("401");

            SolarSystem solarSystem = new SolarSystem(-1, temp.userID, newSystem.systemName, newSystem.systemVisibility);

            var id = _baseRepo.AddSolarSystem(solarSystem);
            return id;

        }
        public int SolarSystemCount()
        {
            return _baseRepo.Count("SolarSystem");
        }

        public int SpaceObjectCount()
        {
            return _baseRepo.Count("SpaceObject");
        }

        public SolarSystem GetSolarSystemByID(int id, Entities.LoginRequest cred)
        {
            
            var system = _baseRepo.GetSolarSystemByID(id);
            if(system.systemVisibility == Visibility.Private)
            {
                User? temp = _userService.ValidateUser(cred) ?? throw new BadHttpRequestException("401");
                if(system.ownerId == temp.userID || temp.role == Role.Admin)
                {
                    return system;
                }
                else
                {
                    throw new BadHttpRequestException("403");
                }
            }
            else
            {
                return system;
            }

        }

        public int AddSpaceObject(SpaceObject spaceObject, Entities.LoginRequest cred) //non secure, for front end testing only
        {
            User? temp = _userService.ValidateUser(cred) ?? throw new BadHttpRequestException("401");
            SolarSystem system = _baseRepo.GetSolarSystemByID(spaceObject.solarSystemID) ?? throw new BadHttpRequestException("400");

            if(system.ownerId == temp.userID || temp.role == Role.Admin)
            {
                int addedSpaceObjectId = _baseRepo.AddSpaceObject(spaceObject);
                return addedSpaceObjectId;
            }   
                throw new BadHttpRequestException("403");

        }
        
        public bool RemoveSpaceObject(int id)
        {
            _baseRepo.RemoveSpaceObject(id); //non secure, for front end testing only
            return true;
        }
        
    }
}
