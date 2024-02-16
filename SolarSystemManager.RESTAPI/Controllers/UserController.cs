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
            if (_userService.ValidateUser(cred))
            {
                return Ok("Success!");
            }
            return Ok("Invalid username or password!");
        }
    }

    public class SettingsController : ControllerBase
    {

        private readonly ILogger<SolarSystemController> _logger;
        private readonly UserService _userService;

        public SettingsController(ILogger<SolarSystemController> logger)
        {
            _logger = logger;
            _userService = new UserService();
        }

        [HttpPost]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("GetSettings")]
        public IActionResult GetSettings(LoginRequest cred)
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
