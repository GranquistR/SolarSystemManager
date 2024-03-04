using SolarSystemManager.RESTAPI.Entities;
using SolarSystemManager.RESTAPI.Repos;

namespace SolarSystemManager.RESTAPI.Services
{
    public class SolarSystemService
    {
        BaseRepo _baseRepo = BaseRepo.Instance();

        public IEnumerable<SolarSystem> GetAllSolarSystems()
        {
            return _baseRepo.GetAllSolarSystems();
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

        public SolarSystem GetSolarSystemByID(int id)
        {
            return _baseRepo.GetSolarSystemByID(id);
        }
    }
}
