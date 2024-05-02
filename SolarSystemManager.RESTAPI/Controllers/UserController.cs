using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SolarSystemManager.RESTAPI.Entities;
using SolarSystemManager.RESTAPI.Services;
using System.Text.Json;
using System.Collections.Specialized;
using System.Numerics;
using System;

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
        public IActionResult Login([FromBody] EncryptedMessage encMessage)
        {
            
            LoginRequest? cred = JsonSerializer.Deserialize<LoginRequest>(EncryptionController.dRSA(encMessage.message, encMessage.key, encMessage.n));
            try
            {
                var user = _userService.ValidateUser(cred);
                if (user != null)
                {
                    string jsonString1 = JsonSerializer.Serialize(user);
                    EncryptedMessage eData = EncryptionController.eRSA(jsonString1);

                    return Ok(new Response { success = true, status = 200, message = "Sucessfully Logged in", data = eData });
                }
                return Ok(new Response { success = false, status = 401, message = "Failed to Login. Invalid credentials." });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error in UserController");
            }
        }

        [HttpPost]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("CreateAccount")]
        public IActionResult CreateAccount([FromBody] EncryptedMessage encMessage)
        {
            User? newAccount = JsonSerializer.Deserialize<User>(EncryptionController.dRSA(encMessage.message, encMessage.key, encMessage.n));


            try
            {
                _userService.CreateAccount(newAccount);
                return Ok(new Response { success = true, status = 200, message = "Sucessfully Created Account", data = null });
            }
            catch (BadHttpRequestException e)
            {
                return Ok(new Response { success = false, status = 400, message = e.Message, data = null });
            }
            catch
            {
                return Ok(new Response { success = false, status = 500, message = "Unknown Server Error", data = null });
            }
        }


        [HttpPost]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("GetUserSettings")]
        public IActionResult GetUserSettings([FromBody] LoginRequest cred)
        {
            try
            {
                var settings = _userService.GetUserSettingsData(cred);
                return Ok(new Response { success = true, status = 200, message = "Sucessfully Retrieved Settings", data = settings });

            }
            catch (BadHttpRequestException e)
            {
                return Ok(new Response { success = false, status = 400, message = e.Message, data = null });
            }
            catch
            {
                return Ok(new Response { success = false, status = 500, message = "Unknown Server Error", data = null });
            }


        }
        [HttpPost]
        [EnableCors("AllowSpecificOrigin")]
        [Route("GetSalts")]
        public IActionResult GetSalt([FromBody] EncryptedMessage encMessage) 
        {

            string username = EncryptionController.dRSA(encMessage.message, encMessage.key, encMessage.n);

            try
            {
                string salt = _userService.GetSalty(username);

                if (salt != null)
                {
                    EncryptedMessage eData = EncryptionController.eRSA(salt);

                    return Ok(new Response { success = true, status = 200, message = "Sucessfully Retrieved Salt", data = eData });
                }
                else
                {
                    throw new BadHttpRequestException("Invalid username!");
                }
            }
            catch (BadHttpRequestException e)
            {
                // Handle the exception if necessary
                return Ok(new Response { success = false, status = 400, message = e.Message, data = null });
            }
            catch
            {
                // Handle other exceptions if necessary
                return Ok(new Response { success = false, status = 500, message = "Unknown Server Error", data = null });
            }
        }

        [HttpGet]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("GetUserCount")]
        public IActionResult GetUserCount()
        {
            try
            {
                var count = _userService.UserCount();
                return Ok(new Response { success = true, status = 200, message = "Sucessfully Retrieved User Count", data = count });

            }
            catch (BadHttpRequestException e)
            {
                return Ok(new Response { success = false, status = 400, message = e.Message, data = null });
            }
            catch
            {
                return Ok(new Response { success = false, status = 500, message = "Unknown Server Error", data = null });
            }
        }

        [HttpPost]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("ChangeUsername")]
        public IActionResult ChangeUsername([FromBody] EncryptedMessage encMessage)
        {
            ChangeCredRequest? cred = JsonSerializer.Deserialize<ChangeCredRequest>(EncryptionController.dRSA(encMessage.message, encMessage.key, encMessage.n));
            Console.WriteLine(cred);
            try
            {
                _userService.ChangeUserName(cred);
                return Ok(new Response { success = true, status = 200, message = "Successfully Changed Username", data = null });
            }
            catch (BadHttpRequestException e)
            {
                return Ok(new Response { success = false, status = 400, message = e.Message, data = null });
            }
            catch
            {
                return Ok(new Response { success = false, status = 500, message = "Unknown Server Error", data = null });
            }
        }

        [HttpPost]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("ChangePassword")]
        public IActionResult ChangePassword([FromBody] EncryptedMessage encMessage)
        {
            ChangeCredRequest? cred = JsonSerializer.Deserialize<ChangeCredRequest>(EncryptionController.dRSA(encMessage.message, encMessage.key, encMessage.n));

            try
            {
                _userService.ChangePassword(cred);
                return Ok(new Response { success = true, status = 200, message = "Successfully Changed Password", data = null });
            }
            catch (BadHttpRequestException e)
            {
                return Ok(new Response { success = false, status = 400, message = e.Message, data = null });
            }
            catch
            {
                return Ok(new Response { success = false, status = 500, message = "Unknown Server Error", data = null });
            }
        }

    }
}
