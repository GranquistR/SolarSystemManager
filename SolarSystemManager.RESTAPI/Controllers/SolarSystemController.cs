using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SolarSystemManager.RESTAPI.Entities;

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
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("GetAllSolarSystems")]
        public IActionResult GetAllSolarSystems()
        {
            string test = "Reach the API!!";
            return Ok(test);
        }

        [HttpPost]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("GetSolarSystemById")]
        public IActionResult GetSolarSystemById(int id)
        {
            return Ok("Success! " + id);
        }
        
        //Leo's dummy test
        [HttpGet]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("SolarSystemInfoTest")]
        public IActionResult SolarSystemInfoTest()
        {
            var testSys = new SolarSystem(1, 2, "Sol", false);
           // string test = "Solar System ID: " + testSys.solarSystemID + " | Owner ID: " + testSys.ownerID +
             //       " | Solar System Name: " + testSys.systemName + " | Solar System Private: " + testSys.systemIsPrivate;
            return Ok(testSys);
        }
    }
}
