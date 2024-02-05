using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace SolarSystemManager.RESTAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SolarSystemController : ControllerBase
    {

        private readonly ILogger<SolarSystemController> _logger;

        public SolarSystemController(ILogger<SolarSystemController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [EnableCors]
        [Route("GetAllSolarSystems")]
        public string GetAllSolarSystems()
        {
            return "You got a solar system!";
        }

        [HttpPost]
        [EnableCors]
        [Route("GetSolarSystemById")]
        public string GetSolarSystemById(int id)
        {
            return "You got #"+ id;
        }
    }
}
