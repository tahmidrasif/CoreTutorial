using System.Threading.Tasks;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utility.Helpers;

namespace API.Controllers
{
    public class TestController :  BaseController
    {
        private readonly ITestService _testService;
        private readonly TaposRSA _taposRsa;

        public TestController(ITestService testService,TaposRSA taposRsa)
        {
            _testService = testService;
            this._taposRsa = taposRsa;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // _taposRsa.GenerateRsaKey("v1");
            //await  _testService.SaveAllData();
            await _testService.UpdateBalance();
            return Ok("hello");
        }

        //[HttpGet]
        //[Authorize(Roles ="teacher",Policy = "TokenPolicy")]
        //public async Task<IActionResult> Test1()
        //{
            
        //    //await  _testService.SaveAllData();
        //    return Ok("hello");
        //}

        //[HttpGet]
        //[Authorize(Roles = "staff", Policy = "TokenPolicy")]
        //public async Task<IActionResult> Test2()
        //{

        //    //await  _testService.SaveAllData();
        //    return Ok("hello");
        //}
    }
}