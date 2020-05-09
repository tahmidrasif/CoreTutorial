using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Request;
using BLL.Services;
using DLL.Model;
using DLL.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class StudentController : BaseController
    {
        private readonly IStudentService _service;

        public StudentController(IStudentService service)
        {
            this._service = service;
        }
        
        [HttpGet]
        //[ApiVersion("1.0")]
        //[Authorize(Roles = "teacher")]
        public async Task<ActionResult> GetAll()
        {
            var students = await _service.GetAllStudentAsync();
            return Ok(students);
        }



        [HttpGet]
        //[ApiVersion("1.1")]
        [Route("{email}")]
        [Authorize(Roles = "teacher")]
        public async Task<ActionResult> GetStudent(string email)
        {
            var student = await _service.GetStudentByEmailAsync(email);
            return Ok(student);
        }

        [HttpPost]
        //[ApiVersion("1.1")]
        [Authorize(Roles = "teacher")]
        public async Task<ActionResult> Insert(StudentRequest oStudent)
        {
            //StudentRequest students = new StudentRequest();
            var students = await _service.AddStudentAsync(oStudent);
            return Ok(students);
        }

        [HttpPut("{roll}")]
        [Authorize(Roles = "staff")]
        public async Task<ActionResult> Update(string roll,   StudentUpdateRequest request)
        {
            return Ok(await _service.UpdateAsync(roll, request));
        }

        [HttpDelete("{roll}")]
        [Authorize(Roles = "teacher")]
        public async Task<ActionResult> Delete([FromForm] string roll)
        {
            return Ok(await _service.DeleteAsync(roll));
        }

    }
    
}