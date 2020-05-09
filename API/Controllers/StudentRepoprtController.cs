using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class StudentRepoprtController : BaseController
    {
        private readonly IStudentService _stuedntService;

        public StudentRepoprtController(IStudentService stuedntService)
        {
            this._stuedntService = stuedntService;
        }


        [HttpGet]
        //[ApiVersion("1.0")]
        //[Authorize(Roles = "teacher")]
        public async Task<ActionResult> GetAllStudentDepartmentReport()
        {
            var studentsreport =  _stuedntService.GetAllStudentReport();
            return Ok(studentsreport);
        }


        [HttpGet]
        [Route("GetAllStudentCourseReport")]
        //[ApiVersion("1.0")]
        //[Authorize(Roles = "teacher")]
        public async Task<ActionResult> GetAllStudentCourseReport()
        {
            var studentsreport = await _stuedntService.GetAllStudentCourseReport();
            return Ok(studentsreport);
        }


    }
}