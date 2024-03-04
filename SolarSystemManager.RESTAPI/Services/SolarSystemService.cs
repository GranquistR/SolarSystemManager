using SolarSystemManager.RESTAPI.Entities;
using SolarSystemManager.RESTAPI.Repos;

namespace SolarSystemManager.RESTAPI.Services
{
    public class SolarSystemService
    {
        BaseRepo _baseRepo = BaseRepo.Instance();
        UserService _userService = new UserService();

        public IEnumerable<SolarSystem> GetAllSolarSystems()
        {
            return _baseRepo.GetAllSolarSystems();
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
