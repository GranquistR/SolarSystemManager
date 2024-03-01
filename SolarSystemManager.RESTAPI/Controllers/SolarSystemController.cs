using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SolarSystemManager.RESTAPI.Entities;
using SolarSystemManager.RESTAPI.Services;
using static SolarSystemManager.RESTAPI.Entities.SolarSystem;

namespace SolarSystemManager.RESTAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SolarSystemController : ControllerBase
    {

        private readonly ILogger<SolarSystemController> _logger;
        private readonly SolarSystemService _solarSystemService;

        public SolarSystemController(ILogger<SolarSystemController> logger)
        {
            _logger = logger;
            _solarSystemService = new SolarSystemService();
        }

        [HttpGet]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("TestGet")]
        public IActionResult TestGet()
        {
            string test = "Reach the API!!";
            return Ok(test);
        }

        [HttpPost]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("TestPost")]
        public IActionResult TestPost(int id)
        {
            _solarSystemService.DeleteSolarSystem(id);
            return Ok("Success! " + id);
        }
        
        [HttpGet]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("GetAllPublicSolarSystems")]
        public IActionResult GetAllPublicSolarSystems()
        {
            IEnumerable<SolarSystem> solarSystems = new List<SolarSystem>

            {
                new SolarSystem(1, 2, "Sol", Visibility.Public),
                new SolarSystem(2, 3, "Alpha Centauri", Visibility.Private),
                new SolarSystem(3, 4, "Proxima Centauri", Visibility.Public),
                new SolarSystem(4, 5, "Barnard's Star", Visibility.Private),
                new SolarSystem(5, 6, "Wolf 359", Visibility.Public),
                new SolarSystem(6, 7, "Lalande 21185", Visibility.Private),
                new SolarSystem(7, 8, "Sirius", Visibility.Public),
                new SolarSystem(8, 9, "Luyten 726-8", Visibility.Private),
                new SolarSystem(9, 10, "Ross 154", Visibility.Public),
                new SolarSystem(10, 11, "Ross 248", Visibility.Private),
                new SolarSystem(11, 12, "Epsilon Eridani", Visibility.Public),
                new SolarSystem(12, 13, "Lacaille 9352", Visibility.Private),
                new SolarSystem(13, 14, "Ross 128", Visibility.Public),
                new SolarSystem(14, 15, "EZ Aquarii", Visibility.Private),
                new SolarSystem(15, 16, "61 Cygni", Visibility.Public),
                new SolarSystem(16, 17, "Procyon", Visibility.Private),
                new SolarSystem(17, 18, "Struve 2398", Visibility.Public),
                new SolarSystem(18, 19, "Groombridge 34", Visibility.Private),
                new SolarSystem(19, 20, "DX Cancri", Visibility.Public),
                new SolarSystem(20, 21, "Tau Ceti", Visibility.Private),
                new SolarSystem(21, 22, "Luyten's Star", Visibility.Public),
                new SolarSystem(22, 23, "Kapteyn's Star", Visibility.Private),
                new SolarSystem(23, 24, "Kruger 60", Visibility.Public),
                new SolarSystem(24, 25, "Gliese 682", Visibility.Private),
                new SolarSystem(25, 26, "Epsilon Indi", Visibility.Public),
                new SolarSystem(26, 27, "Gliese 674", Visibility.Private),
                new SolarSystem(27, 28, "Gliese", Visibility.Public)
            };

            return Ok(solarSystems.Where(s=>s.systemVisibility == Visibility.Public).ToList());
        }

        //for testing purposes, creates a solar system object. Eventually will get a solar system object from the database
        [HttpGet]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("GetSolarSystem")]
        public IActionResult GetSolarSystem()
        {
             SolarSystem dummySolarSystem = new SolarSystem(1, 2, "Sol", Visibility.Public);

             return Ok(dummySolarSystem);
        }

        //add space object
        [HttpPost]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("AddSpaceObject")]
        public IActionResult AddSpaceObject()
        {
            //will take a solar system id and space object information and update that in the data base
            return Ok("Add Space Obejct Works");
        [HttpGet]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("GetSolarSystemCount")]
        public IActionResult GetSolarSystemCount()
        {
            try
            {
                return Ok(_solarSystemService.SolarSystemCount());
            }
            catch (BadHttpRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("GetSpaceObjectCount")]
        public IActionResult GetSpaceObjectCount()
        {
            try
            {
                return Ok(_solarSystemService.SpaceObjectCount());
            }
            catch (BadHttpRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
