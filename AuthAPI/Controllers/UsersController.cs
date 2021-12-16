using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyTrackDatabaseAPI.Data;
using MoneyTrackDatabaseAPI.Models;
using MoneyTrackDatabaseAPI.Services;

namespace MoneyTrackDatabaseAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService userService;
        private IAuthService authService;
        private ITokenService tokenService;
        

        public UsersController(IUserService userService, IAuthService authService,ITokenService tokenService)
        {
            this.userService = userService;
            this.tokenService = tokenService;
            this.authService = authService;

        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<User>> Register([FromBody] User user)
        {
            try
            {
                await userService.Register(user);
                return Ok(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(400, new ApiError(e.Message));
            }
        }
        [HttpPost]
        [Route("addDevice")]
        public async Task<ActionResult<User>> AddDevice([FromQuery(Name = "eui")] String eui)
        {
            try
            {
                User user = await userService.AddDevice(eui);
                return Ok(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(400, new ApiError(e.Message));
            }
        }
        
        [HttpGet]
        [Route("devices")]
        public async Task<ActionResult<User>> GetAll()
        {
            try
            {
                return Ok(await userService.GetDevices());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(400, new ApiError(e.Message));
            }
        }
        
        [HttpDelete]
        [Route("deleteDevice")]
        public async Task<ActionResult<User>> DeleteDevice([FromQuery(Name = "eui")] String eui)
        {
            try
            {
                User user = await userService.DeleteDevice(eui);
                return Ok(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(400, new ApiError(e.Message));
            }
        }

        [HttpDelete]
        [Route("deleteProfile")]
        public async Task<ActionResult<User>> RemoveUser()
        {
            try
            {
                var returnedUser = await userService.Delete();
                return Ok(returnedUser);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(400, new ApiError(e.Message));
            }
        }
       
        [HttpPut]
        public async Task<ActionResult<User>> UpdateUser([FromBody]User user)
        {
            try
            {
                await userService.Update(user);
                return Ok(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(400, new ApiError(e.Message));
            }
        }
        
    }
}