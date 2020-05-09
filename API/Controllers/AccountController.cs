using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BLL.Request;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            this._accountService = accountService;
        }

        //[HttpPost("template:login")]
        [HttpPost]
        public async Task<ActionResult> LogInAsync(LogInRequest logInRequest)
        {

           return Ok( await _accountService.LogIn(logInRequest));

            //return Ok("Ok");
        }


        [HttpPost]
        [Authorize(Roles = "staff", Policy = "AddToken")]
        public ActionResult Test1()
        {

            return Ok("Test1");

            //return Ok("Ok");
        }

        
        //[HttpGet("template:test2")]
        [HttpGet]
        [Authorize(Roles = "teacher", Policy = "AddToken")]
        public ActionResult Test2()
        {
            //_accountService.Test(User);
            return Ok("Test2");

            //return Ok("Ok");
        }

        [HttpPost]
        [Authorize(Roles = "teacher,staff", Policy = "AddToken")]
        public async Task<ActionResult> Logout()
        {
           
             return Ok(await _accountService.LogOut(User));

            //return Ok("Ok");
        }

        [HttpPost]
        public async Task<ActionResult> GetNewToken(RefreshTokenRequest refreshToken)
        {

            return Ok(await _accountService.RefreshToken(refreshToken));

            //return Ok("Ok");
        }

    }
}