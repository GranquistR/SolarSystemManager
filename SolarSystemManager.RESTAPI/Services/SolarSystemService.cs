using SolarSystemManager.RESTAPI.Entities;
using SolarSystemManager.RESTAPI.Repos;
using static SolarSystemManager.RESTAPI.Entities.SolarSystem;

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

        public bool DeleteSolarSystem(Entities.LoginRequest cred, int id)
        {
            User? temp = _userService.ValidateUser(cred) ?? throw new BadHttpRequestException("401");
            if (temp.userID != _baseRepo.GetSolarSystemByID(id).ownerId) 
            {
                throw new BadHttpRequestException("403");
            }
            _baseRepo.DeleteSolarSystem(id);
            return true;
        }
        public bool DeleteSolarSystemAdmin(Entities.LoginRequest cred, int id)
        {
            User? temp = _userService.ValidateUser(cred) ?? throw new BadHttpRequestException("401");
            if (temp.role != Role.Admin)
            {
                throw new BadHttpRequestException("403");
            }
            _baseRepo.DeleteSolarSystem(id);
            return true;
        }

        public bool DeleteSpaceObject(Entities.LoginRequest cred, int id)
        {
            User? temp = _userService.ValidateUser(cred) ?? throw new BadHttpRequestException("401");
            if (temp.userID != _baseRepo.GetSolarSystemByID(id).ownerId)
            {
                throw new BadHttpRequestException("403");
            }
            _baseRepo.DeleteSpaceObject(id);
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

        public SolarSystem GetSolarSystemByID(int id)
        {
            return _baseRepo.GetSolarSystemByID(id);
        }

        public int AddSpaceObject(int size)
        {
            return _baseRepo.AddSpaceObject(size);
        }

        public int RemoveSpaceObjectByID(int targetID)
        {
            return _baseRepo.RemoveSpaceObjectByID(targetID);
        }
    }
}
