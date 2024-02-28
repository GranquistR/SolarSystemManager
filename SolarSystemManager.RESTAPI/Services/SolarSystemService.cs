using SolarSystemManager.RESTAPI.Repos;

namespace SolarSystemManager.RESTAPI.Services
{
    public class SolarSystemService
    {
        BaseRepo _baseRepo = BaseRepo.Instance();

        public bool DeleteSolarSystem(int id)
        {
            _baseRepo.DeleteSolarSystem(id);
            return true;
        }
    }
}
