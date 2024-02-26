using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SolarSystemManager.RESTAPI.Entities;
using SolarSystemManager.RESTAPI.Services;

namespace SolarSystemManager.RESTAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly ILogger<SolarSystemController> _logger;
        private readonly UserService _userService;

        public UserController(ILogger<SolarSystemController> logger)
        {
            _logger = logger;
            _userService = new UserService();
        }

        [HttpPost]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("Login")]
        public IActionResult Login([FromBody]LoginRequest cred)
        {
            if (_userService.ValidateUser(cred))
            {
                return Ok("Success!");
            }
            return Ok("Invalid username or password!");
        }

        [HttpPost]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("GetUserSettings")]
        public IActionResult GetUserSettings([FromBody] LoginRequest cred)
        {
            string data = _userService.GetUserData(cred);

            if (data != "0")
            {
                return Ok(data);
            }
            return Ok("Could Not Find Data");
        }
    }
}
