using System.Data;
using System.Security.Cryptography;
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
            try
            {
                return Ok(_solarSystemService.GetAllSolarSystems().Where(s => s.systemVisibility == Visibility.Public).OrderBy(x=>x.systemName));
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

        //dummy data for graphics testing
        [HttpGet]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("GetSpaceObjects")]
        public IActionResult GetSpaceObjects()
        {
            IEnumerable<SpaceObject> spaceObjects = new List<SpaceObject>
            {
              //space object template
              //new SpaceObject(objID, systemId, objName, objType, x, y, objSize, objColor);
                new SpaceObject(1, 1, "Sirius A", "Star", 5, 0, 10, "18D3BC"), //two stars
                new SpaceObject(2, 1, "Sirius B", "Star", 0, 0, 7, "25BCD6"),

                new SpaceObject(3, 1, "Thor", "Planet", 15, 0, 2, "E5E21C"), //three planets
                new SpaceObject(4, 1, "Loki", "Planet", 20, 0, 1, "1CE556"),
                new SpaceObject(5, 1, "Odin", "Planet", 25, 0, 3, "2F1CE5")
            };

            return Ok(spaceObjects);
        }
    }
}
