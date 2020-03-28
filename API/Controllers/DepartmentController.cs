using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    public class DepartmentController : BaseController
    {
        [HttpGet]
        public ActionResult GetDepartment()
        {

            return Ok("ok");
        }
    }
}