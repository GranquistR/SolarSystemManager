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
            //this should eventually have a capability to draw data from the database based on the id
            //that would then create the object and return it to the editor, me thinks -Leo
            var testSys = new SolarSystem(1, 2, "Sol", false); //Leo's dummy test

            return Ok(testSys);
        }

        [HttpPost]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("AddSpaceObject")]
        public IActionResult AddSpaceObject(string type)
        {
            //this just creates a new solar system object for the time being for testing purposes,
            //not actually drawing from the object in the editor. Data from this function WILL NOT STICK rn. -Leo
            var testSys = new SolarSystem(1, 2, "Sol", false); //Leo's dummy test
            if (type == "Star")
            {
                testSys.addSpaceObject(1, "Sun", "Star", 0, 0, 864600, 0xFFA718); //adds data about the sun
            }
            else if(type == "Planet")
            {
                testSys.addSpaceObject(2, "Earth", "Planet", 0, 93, 7926, 0x80FFF5); //adds data about earth
            }

            return Ok(testSys);
        }

        //Data table test
        [HttpGet]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("GetPublicSolarSystems")]
        public IActionResult GetPublicSolarSystems()
        {
            IEnumerable<SolarSystem> solarSystems = new List<SolarSystem>

            {
                new SolarSystem(1, 2, "Sol", false),
                new SolarSystem(2, 3, "Alpha Centauri", true),
                new SolarSystem(3, 4, "Proxima Centauri", true),
                new SolarSystem(4, 5, "Barnard's Star", true),
                new SolarSystem(5, 6, "Wolf 359", true),
                new SolarSystem(6, 7, "Lalande 21185", true),
                new SolarSystem(7, 8, "Sirius", true),
                new SolarSystem(8, 9, "Luyten 726-8", true),
                new SolarSystem(9, 10, "Ross 154", true),
                new SolarSystem(10, 11, "Ross 248", true),
                new SolarSystem(11, 12, "Epsilon Eridani", true),
                new SolarSystem(12, 13, "Lacaille 9352", true),
                new SolarSystem(13, 14, "Ross 128", true),
                new SolarSystem(14, 15, "EZ Aquarii", true),
                new SolarSystem(15, 16, "61 Cygni", true),
                new SolarSystem(16, 17, "Procyon", true),
                new SolarSystem(17, 18, "Struve 2398", true),
                new SolarSystem(18, 19, "Groombridge 34", true),
                new SolarSystem(19, 20, "DX Cancri", true),
                new SolarSystem(20, 21, "Tau Ceti", true),
                new SolarSystem(21, 22, "Luyten's Star", true),
                new SolarSystem(22, 23, "Kapteyn's Star", true),
                new SolarSystem(23, 24, "Kruger 60", true),
                new SolarSystem(24, 25, "Gliese 682", true),
                new SolarSystem(25, 26, "Epsilon Indi", true),
                new SolarSystem(26, 27, "Gliese 674", true),
                new SolarSystem(27, 28, "Gliese", false)
            };

            return Ok(solarSystems.Where(s=>!s.systemIsPrivate).ToList());
        }
    }
}
