using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyTrackDatabaseAPI.Data;
using MoneyTrackDatabaseAPI.Models;
using MoneyTrackDatabaseAPI.Services;

namespace MoneyTrackDatabaseAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private IAuthService authService;
        private ITokenService tokenService;
        private IUserService userService;

       
        public AuthController(IAuthService AuthService, ITokenService tokenService,IUserService userService)
        {
            this.authService = AuthService;
            this.tokenService = tokenService;
            this.userService = userService;
        }
         
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<User>> Login([FromBody] User user)
        {
            try
            {
                var returnedUser = await userService.Validate(user.Email,user.Password);
                var dict = new Dictionary<string, Object>();
                var refreshToken = await authService.GenerateRefreshToken(returnedUser.Id);
                // await tokenService.AddToken(refreshToken);
                var accessToken = await authService.GenerateAccessToken(returnedUser.Id);
                dict.Add("token", accessToken);
                dict.Add("user", returnedUser);
                CookieOptions cookieOptions = new CookieOptions();
                // cookieOptions.Secure = true;
                cookieOptions.HttpOnly = true;
                cookieOptions.SameSite = SameSiteMode.None;
                HttpContext.Response.Cookies.Append("rt",refreshToken,cookieOptions);
                return Ok(dict);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(400, new ApiError(e.Message));
            }
        }
        
        [HttpGet]
        [Route("refresh")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            try
            {
                string token = Request.Cookies["rt"];
                var accessToken = await authService.GenerateAccessToken(token);
                if (accessToken != null)
                {
                    var dict = new Dictionary<string, string>();
                    dict.Add("token", accessToken);
                    return Ok(dict);
                }
                return StatusCode(403, new ApiError("Unable to refresh!"));

            }
            catch (Exception e)
            {
                return StatusCode(401, new ApiError(e.Message));
            }
            
        }
        [HttpGet]
        [Route("logout")]
        public async Task<ActionResult<string>> LogOut()
        {
            try
            {
                string token = Request.Cookies["rt"];
                if (token is "")
                {
                    return StatusCode(401,new ApiError("You are not logged in!"));
                }

                await tokenService.Logout(token);

                CookieOptions cookieOptions = new CookieOptions
                {
                    Secure = true, HttpOnly = true, SameSite = SameSiteMode.None
                };
                HttpContext.Response.Cookies.Append("rt","",cookieOptions);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(401, new ApiError(e.Message));
            }
            
        }
    }
}