using System.Data;
using System.Drawing;
using System.Security.Cryptography;
using System.Xml.Linq;
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

        [HttpPost]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("DeleteSolarSystem")]
        public IActionResult DeleteSolarSystem([FromBody] LoginRequest cred, int id)
        {
            try
            {
                var result = _solarSystemService.DeleteSolarSystem(cred, id);
                if (result)
                {
                    return Ok(new Response { success = true, status = 200, message = "Sucessfully Deleted Solar System", data = null });
                }
                return Ok(new Response { success = false, status = 400, message = "Failed to delete Solar System", data = null });
            }
            catch (BadHttpRequestException e)
            {
                if (e.Message == "401")
                {
                    return Ok(new Response { success = false, status = 401, message = "Failed to delete Solar System", data = null });
                }
                else
                {
                    return Ok(new Response { success = false, status = 403, message = "Failed to delete Solar System", data = null });
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
                var result = _solarSystemService.DeleteSolarSystemAdmin(cred, id);
                if (result)
                {
                    return Ok(new Response { success = true, status = 200, message = "Sucessfully Deleted Solar System", data = null });
                }
                return Ok(new Response { success = false, status = 400, message = "Failed to delete Solar System", data = null });
            }
            catch (BadHttpRequestException e)
            {
                if (e.Message == "401")
                {
                    return Ok(new Response { success = false, status = 401, message = "Failed to delete Solar System", data = null });
                }
                else
                {
                    return Ok(new Response { success = false, status = 403, message = "Failed to delete Solar System", data = null });
                }
            }
        }

        [HttpPost]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("CreateSolarSystem")]
        public IActionResult CreateSolarSystem([FromBody] NewSolarSystemRequest solarSystemRequest)
        {
            try
            {
                var result = _solarSystemService.CreateSolarSystem(solarSystemRequest.solarSystem, solarSystemRequest.credentials);
                if (result != null)
                {
                    return Ok(new Response { success = true, status = 200, message = "Sucessfully Created Solar System", data = result });
                }
                return Ok(new Response { success = false, status = 400, message = "Failed to create Solar System", data = null });
            }
            catch (BadHttpRequestException e)
            {
                return Ok(new Response { success = false, status = 400, message = e.Message, data = null });
            }
            catch
            {
                return Ok(new Response { success = false, status = 500, message = "Unknown Error in CreateSolarSystem", data = null });
            }
        }   

        [HttpGet]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("GetAllPublicSolarSystems")]
        public IActionResult GetAllPublicSolarSystems()
        {
            try
            {
                var solarSystems = _solarSystemService.GetAllPublicSolarSystems().Where(s => s.systemVisibility == Visibility.Public).OrderBy(x => x.systemName);
                return Ok(new Response { success = true, status = 200, message = "Sucessfully Got Solar System", data = solarSystems });
            }
            catch (BadHttpRequestException e)
            {
                return Ok(new Response { success = false, status = 400, message = e.Message, data = null });
            }
            catch
            {
                return Ok(new Response { success = false, status = 500, message ="Unknown Error in GetAllPublicSolarSystems", data = null });
            }
        }

        [HttpPost]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("GetMySolarSystems")]
        public IActionResult GetMySolarSystems([FromBody] LoginRequest cred)
        {
            try
            {
                var solarSystems = _solarSystemService.GetMySolarSystems(cred);
                return Ok(new Response { success = true, status = 200, message = "Successfully got your solar systems", data = solarSystems });
            }
            catch (BadHttpRequestException e)
            {
                if (e.Message == "401")
                {
                    return Ok(new Response { success = false, status = 401, message = "Invalid credentials", data = null });
                }
                return Ok(new Response { success = false, status = 400, message = e.Message, data = null });
            }
            catch
            {
                return Ok(new Response { success = false, status = 500, message = "unknown Error in GetMySolarSYstems", data = null });
            }
        }

        [HttpPost]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("GetAllSolarSystemsAdmin")]
        public IActionResult GetAllSolarSystemsAdmin([FromBody] LoginRequest cred)
        {
            try
            {
                var solarSystems = _solarSystemService.GetAllSolarSystemsAdmin(cred);
                return Ok(new Response { success = true, status = 200, message = "Successfully got all solar systems", data = solarSystems });
            }
            catch (BadHttpRequestException e)
            {
                if (e.Message == "401")
                {
                    return Ok(new Response { success = false, status = 401, message = "Invalid login credentials", data = null });
                }
                if (e.Message == "403")
                {
                    return Ok(new Response { success = false, status = 403, message = "You do not have access to this resource", data = null });
                }
                return Ok(new Response { success = false, status = 400, message = e.Message, data = null });
            }
            catch
            {
                return Ok(new Response { success = false, status = 500, message = "Unknown error in GetAllSolarSystemsAdmin", data = null });
            }
        }


        [HttpGet]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("GetSolarSystemCount")]
        public IActionResult GetSolarSystemCount()
        {
            try
            {
                var count = _solarSystemService.SolarSystemCount();
                return Ok(new Response { success = false, status = 200, message = "Successfully got solar system count", data = count });
            }
            catch (BadHttpRequestException e)
            {
                return Ok(new Response { success = false, status = 400, message = e.Message, data = null });
            }
            catch
            {
                return Ok(new Response { success = false, status = 500, message = "Unknown error in GetSolarSystemCount", data = null });
            }
        }

        [HttpGet]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("GetSpaceObjectCount")]
        public IActionResult GetSpaceObjectCount()
        {
            try
            {
                var count = _solarSystemService.SpaceObjectCount();
                return Ok(new Response { success = false, status = 200, message = "Successfully got space object count", data = count });
            }
            catch (BadHttpRequestException e)
            {
                return Ok(new Response { success = false, status = 400, message = e.Message, data = null });

            }
            catch
            {
                return Ok(new Response { success = false, status = 500, message = "Unknown error in GetSpaceObjectCount", data = null });
            }
        }

        [HttpPost]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("DeleteSpaceObject")]
        public IActionResult DeleteSpaceObject([FromBody] LoginRequest cred, int id)
        {
            try
            {   
                var count = _solarSystemService.DeleteSpaceObject(cred, id);
                return Ok(new Response { success = true, status = 200, message = "Successfully deleted space object", data = count });
            }
            catch (BadHttpRequestException e)
            {
                if (e.Message == "401")
                {
                    return Ok(new Response { success = false, status = 401, message ="Invalid login credentials", data = null });

                }
                else
                {
                    return Ok(new Response { success = false, status = 403, message = "You are not authorized to delete space object with id: " + id, data = null });
                }
            }
            catch
            {
                return Ok(new Response { success = false, status = 500, message = "Unknown error in DeleteSpaceObject", data = null });
            }
        }

        [HttpGet]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("GetSolarSystemByID")]
        public IActionResult GetSolarSystemByID(int id)
        {
            try
            {
                var solarSystem = _solarSystemService.GetSolarSystemByID(id);
                return Ok(new Response { success = true, status = 200, message = "Successfully got solar system", data = solarSystem });
            }
            catch (BadHttpRequestException e)
            {
                return Ok(new Response { success = false, status = 400, message = e.Message, data = null });
            }
            catch
            {
                return Ok(new Response { success = false, status = 500, message = "Unknown error in GetSolarSystemByID", data = null });
            }
        }

        [HttpGet]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("AddSpaceObject")]
        public IActionResult AddSpaceObject(int size, string type, string name)
        {
            try
            {
                var result = _solarSystemService.AddSpaceObject(size, type, name);
                if (result)
                {
                    return Ok(new Response { success = true, status = 200, message = "Successfully added space object", data = null });
                }
                return Ok(new Response { success = false, status = 400, message = "Failed to add space object", data = null });
            }
            catch (BadHttpRequestException e)
            {
                return Ok(new Response { success = false, status = 400, message = e.Message, data = null });
            }
            catch
            {
                return Ok(new Response { success = false, status = 500, message = "Unknown error in AddSpaceObject", data = null });
            }
        }

        [HttpGet]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("RemoveSpaceObject")]
        public IActionResult RemoveSpaceObject(int id)
        {
            try
            {
                var result = _solarSystemService.RemoveSpaceObject(id);
                if (result)
                {
                    return Ok(new Response { success = true, status = 200, message = "Successfully removed space object", data = null });
                }
                return Ok(new Response { success = false, status = 400, message = "Failed to remove space object", data = null });
            }
            catch (BadHttpRequestException e)
            {
                return Ok(new Response { success = false, status = 400, message = e.Message, data = null });
            }
            catch
            {
                return Ok(new Response { success = false, status = 500, message = "Unknown error in RemoveSpaceObject", data = null });
            }
        }
    }
}