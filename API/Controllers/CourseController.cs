using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Request;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class CourseController : BaseController
    {
        private readonly ICourseService _service;

        public CourseController(ICourseService service)
        {
            this._service = service;
        }

        [HttpGet]
        //[ApiVersion("1.0")]
        //[Authorize(Roles = "teacher")]
        public async Task<ActionResult> GetAll()
        {
            var students = await _service.GetAllCourseAsync();
            return Ok(students);
        }



        [HttpGet]
        //[ApiVersion("1.1")]
        [Route("{code}")]
        //[Authorize(Roles = "teacher")]
        public async Task<ActionResult> GetSingleCourse(string code)
        {
            var student = await _service.FindACourseAsync(code);
            return Ok(student);
        }

        [HttpPost]
        //[ApiVersion("1.1")]
        //[Authorize(Roles = "teacher")]
        public async Task<ActionResult> Insert(CourseInsertRequest course)
        {
            //StudentRequest students = new StudentRequest();
            var courses = await _service.AddCourseAsync(course);
            return Ok(courses);
        }

        [HttpPut("{code}")]
        //[Authorize(Roles = "staff")]
        public async Task<ActionResult> Update(string code, CourseInsertRequest request)
        {
            return Ok(await _service.UpdateCourseAsync(code, request));
        }

        [HttpDelete("{code}")]
        //[Authorize(Roles = "teacher")]
        public async Task<ActionResult> Delete([FromForm] string code)
        {
            return Ok(await _service.DeleteCourseAsync(code));
        }
    }
}