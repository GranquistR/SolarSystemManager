﻿using Microsoft.AspNetCore.Cors;
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
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error in UserController");
            }
        }

        [HttpPost]
        [EnableCors("AllowSpecificOrigin")] // Apply the CORS policy
        [Route("CreateAccount")]
        public IActionResult CreateAccount([FromBody] LoginRequest newAccount)
        {
            try
            {
                _userService.CreateAccount(newAccount);
                return Ok("Success!");
            }
            catch (BadHttpRequestException e)
            {
                return Ok(e.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error in UserController");
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
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error in UserController");
            }


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
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error in UserController");
            }
        }
    }
}
