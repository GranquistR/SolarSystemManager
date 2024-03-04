using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SolarSystemManager.RESTAPI.Entities;
using SolarSystemManager.RESTAPI.Services;  

namespace SolarSystemManager.RESTAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {

        private readonly ILogger<SolarSystemController> _logger;
        private readonly UserService _userService;

        public LoginController(ILogger<SolarSystemController> logger)
        {
            _logger = logger;
            _userService = new UserService();
        }

        [HttpPost]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("Login")]
        public IActionResult Login(LoginRequest cred)
        {
            
                if (_userService.ValidatePass(cred) == true)
                {
                    return Ok("Success!");
                }
                return Ok("Invalid username or password!");
            }


        [HttpPost]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("GetSalts")]
        public IActionResult GetSalt([FromBody] string username)
        {

            try {
                string salt = _userService.GetSalts(username);
                if (salt != null) {

                    return Ok(salt);
                }
                return Ok("Invalid username or password!");
            } catch (BadHttpRequestException e)
            {
                return BadRequest(e.Message);
            } catch
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        } 
    }
 }
    


