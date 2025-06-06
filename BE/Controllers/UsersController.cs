﻿using Microsoft.AspNetCore.Mvc;
using APIs.Contracts;
using BLL.Services.Interface;
using DAL;
using DTOs.Contracts;
using APIs;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Data;
using BLL.Services;


namespace APIs.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public UsersController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        // POST: Users
        [HttpPost]
        public async Task<ActionResult> PostUser([FromBody]CreateUserRequest user)
        {
            try
            {
                var token = await _userService.CreateUser(user);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddHours(12)
                };
                Response.Cookies.Append("jwt", token, cookieOptions);

                return Ok(await _userService.GetRole(user.Email));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An error occurred while processing your request.",
                    Detailed = ex.Message
                });
            }
        }

        // PUT: Users/Login
        [HttpPut("Login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var loginResponse = await _userService.Login(loginRequest);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddHours(12)
                };
                Response.Cookies.Append("jwt", loginResponse.Token, cookieOptions);

                return Ok(loginResponse);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("deactivated"))
                {
                    return Unauthorized(new { message = ex.Message });
                }
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: Users/Logout
        [HttpGet("Logout")]
        public async Task<ActionResult> Logout()
        {
            var token = Request.Cookies["jwt"];
            if (!string.IsNullOrEmpty(token))
            {
                _tokenService.InvalidateToken(token);
            }
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(-1),
                  Path = "/"
            };

            Response.Cookies.Append("jwt", string.Empty, cookieOptions);

            return Ok(new { message = "Logout successful" });
        }

        // DELETE: Users/deactivate/{email}
        [HttpDelete("deactivate/{email}")]
        public async Task<IActionResult> DeactivateUser(string email)
        {
            await _userService.DeactivateUserAsync(email);
            return Ok(new { message = "User deactivated successfully." });
        }

        // GET: Users/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            if (users == null)
            {
                return NotFound();
            }

            return Ok(users);
        }

    }
}
