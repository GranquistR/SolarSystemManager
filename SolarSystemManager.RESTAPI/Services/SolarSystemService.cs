using SolarSystemManager.RESTAPI.Entities;
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

        //------------------------------------------------------------------------------------------------------------
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

            //User? temp = _userService.ValidateUser(cred) ?? throw new BadHttpRequestException("401");
            /*
            User? temp = ValidateUser(new Entities.LoginRequest(cred.username, cred.password));

            if (temp.userID != _baseRepo.GetSolarSystemByID(id).ownerId || temp.role != Role.Admin)
            {
                throw new BadHttpRequestException("403");
            }
            _baseRepo.DeleteSolarSystem(id);
            return true;
            */

            //-----------------------------------------------------------------------------------------------------------

            /*
                User? temp = _userService.ValidateUser(dcred) ?? throw new BadHttpRequestException("401");
                if (temp.userID != _baseRepo.GetSolarSystemByID(id).ownerId || temp.role != Role.Admin)
                {
                    throw new BadHttpRequestException("403");
                }
                _baseRepo.DeleteSolarSystem(id);
                return true;
            */
        

        //------------------------------------------------------------------------------------------------------------

        public bool DeleteSpaceObject(Entities.LoginRequest cred, int id)
        {
            User? temp = _userService.ValidateUser(cred) ?? throw new BadHttpRequestException("401");
            if (temp.userID != _baseRepo.GetSolarSystemByID(id).ownerId || temp.role != Role.Admin)
            {
                throw new BadHttpRequestException("403");
            }
            _baseRepo.DeleteSpaceObject(id);
            return true;
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

        public SolarSystem GetSolarSystemByID(int id)
        {
            return _baseRepo.GetSolarSystemByID(id);
        }

        public bool AddSpaceObject(SpaceObject spaceObject) //non secure, for front end testing only
        {
            _baseRepo.AddSpaceObject(spaceObject);
            return true;
        }
        
        public bool RemoveSpaceObject(int id)
        {
            _baseRepo.RemoveSpaceObject(id); //non secure, for front end testing only
            return true;
        }
        
    }
}
