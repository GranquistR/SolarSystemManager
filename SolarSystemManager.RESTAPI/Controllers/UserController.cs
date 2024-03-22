using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SolarSystemManager.RESTAPI.Entities;
using SolarSystemManager.RESTAPI.Services;
using System.Collections.Specialized;

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
            try
            {
                if (_userService.ValidateUser(cred) != null)
                {
                    return Ok("Success!");
                }
                return Ok("Invalid username or password!");
            }
            catch
            { 
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error in UserController Login");
            }
        }

        [HttpPost]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("CreateAccount")]
        public IActionResult CreateAccount([FromBody] LoginRequest newAccount, [FromBody] string salt)
        {
            try
            {
                _userService.CreateAccount(newAccount, salt);
                return Ok("Success!");
            }
            catch (BadHttpRequestException e)
            {
                return Ok(e.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error in UserController Creation");
            }
        }


        [HttpPost]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("GetUserSettings")]
        public IActionResult GetUserSettings([FromBody] LoginRequest cred)
        {
            try
            {
                return Ok(_userService.GetUserSettingsData(cred));                
            }
            catch (BadHttpRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error in UserController Settings");
            }


        }
        [HttpPost]
        [EnableCors("AllowSpecificOrigin")]
        [Route("GetSalts")]
        public string GetSalt([FromBody] string username)
        {
            try
            {
                string salt = _userService.GetSalty(username);
                if (salt != null)
                {
                    return salt;
                }
                return "Invalid username or password!";
            }
            catch (BadHttpRequestException e)
            {
                // Handle the exception if necessary
                return e.Message;
            }
            catch (Exception ex)
            {
                // Handle other exceptions if necessary
                return "An error occurred: " + ex.Message;
            }
        }




        [HttpGet]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("GetUserCount")]
        public IActionResult GetUserCount()
        {
            try
            {
                return Ok(_userService.UserCount());
            }
            catch (BadHttpRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error in UserController Count");
            }
        }

        [HttpPost]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("ChangeUsername")]
        public IActionResult ChangeUsername([FromBody] LoginRequest cred, string newUN)
        {
            try
            {
                _userService.ChangeUserName(cred, newUN);
                return Ok("Success!");
            }
            catch (BadHttpRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error in UserController");
            }
        }
    }
}
