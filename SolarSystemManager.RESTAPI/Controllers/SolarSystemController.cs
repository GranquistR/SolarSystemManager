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
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("GetAllSolarSystems")]
        public IActionResult GetAllSolarSystems()
        {
            string test = "Reached the API!!";
            return Ok(test);
        }

        [HttpPost]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("GetSolarSystemById")]
        public IActionResult GetSolarSystemById(int id)
        {
            return Ok("Success!");
        }
    }
}
