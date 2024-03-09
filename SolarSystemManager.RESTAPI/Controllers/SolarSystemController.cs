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
        [Route("DeleteSolarSystem")]
        public IActionResult DeleteSolarSystem([FromBody] LoginRequest cred, int id)
        {
            try
            {
                return Ok(_solarSystemService.DeleteSolarSystem(cred, id));
            }
            catch (BadHttpRequestException e)
            {
                if (e.Message == "401")
                {
                    return StatusCode(StatusCodes.Status401Unauthorized);
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden);
                }
            }
        }

        [HttpPost]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("DeleteSolarSystemAdmin")]
        public IActionResult DeleteSolarSystemAdmin([FromBody] LoginRequest cred, int id)
        {
            try
            {
                return Ok(_solarSystemService.DeleteSolarSystemAdmin(cred, id));
            }
            catch (BadHttpRequestException e)
            {
                if (e.Message == "401")
                {
                    return StatusCode(StatusCodes.Status401Unauthorized);
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden);
                }
            }
        }

        [HttpGet]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("GetAllPublicSolarSystems")]
        public IActionResult GetAllPublicSolarSystems()
        {
            try
            {
                return Ok(_solarSystemService.GetAllPublicSolarSystems().Where(s => s.systemVisibility == Visibility.Public).OrderBy(x => x.systemName));
            }
            catch (BadHttpRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error in SolarSystemController");
            }
        }

        [HttpPost]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("GetMySolarSystems")]
        public IActionResult GetMySolarSystems([FromBody] LoginRequest cred)
        {
            try
            {
                return Ok(_solarSystemService.GetMySolarSystems(cred));
            }
            catch (BadHttpRequestException e)
            {
                if (e.Message == "401")
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "Unable to validate credentials.");
                }
                return BadRequest(e.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error in SolarSystemController");
            }
        }

        [HttpPost]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("GetAllSolarSystemsAdmin")]
        public IActionResult GetAllSolarSystemsAdmin([FromBody] LoginRequest cred)
        {
            try
            {
                return Ok(_solarSystemService.GetAllSolarSystemsAdmin(cred));
            }
            catch (BadHttpRequestException e)
            {
                if (e.Message == "401")
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "Unable to validate credentials.");
                }
                if (e.Message == "403")
                {
                    return StatusCode(StatusCodes.Status403Forbidden, "You do not have access to this resource.");
                }
                return BadRequest(e.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error in SolarSystemController");
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
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error in SolarSystemController");
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
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error in SolarSystemController");
            }
        }

        [HttpPost]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("DeleteSpaceObject")]
        public IActionResult DeleteSpaceObject([FromBody] LoginRequest cred, int id)
        {
            try
            {
                return Ok(_solarSystemService.DeleteSpaceObject(cred, id));
            }
            catch (BadHttpRequestException e)
            {
                if (e.Message == "401")
                {
                    return StatusCode(StatusCodes.Status401Unauthorized);
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden);
                }
            }
        }

        [HttpGet]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("GetSolarSystemByID")]
        public IActionResult GetSolarSystemByID(int id)
        {
             return Ok(_solarSystemService.GetSolarSystemByID(id));
        }

        //add space object
        [HttpPost]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("AddSpaceObject")]
        public IActionResult AddSpaceObject(int size)
        {
           
            return Ok(_solarSystemService.AddSpaceObject(size));
        }

        //Remove space object
        [HttpGet]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("RemoveSpaceObjectByID")]
        public IActionResult RemoveSpaceObjectByID(int id)
        {

            return Ok(_solarSystemService.RemoveSpaceObjectByID(id));
        }
    }
}
