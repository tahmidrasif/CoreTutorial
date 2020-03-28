using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DLL.Model;
using DLL.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class StudentController : BaseController
    {
        private readonly IStudentRepository repository;

        public StudentController(IStudentRepository repository)
        {
            this.repository = repository;
        }
        
        [HttpGet]
        [ApiVersion("1.0")]
        public ActionResult GetAll()
        {
            var students = repository.GetAllStudent();
            return Ok(students);
        }

        [HttpGet]
        [ApiVersion("1.1")]
        [Route("{email}")]
        public ActionResult GetStudent(string email)
        {
            var student = repository.GetStudentByEmail(email);
            return Ok(student);
        }

        [HttpPost]
        [ApiVersion("1.1")]
        public ActionResult Insert(Student oStudent)
        {
            var students = repository.AddStudent(oStudent);
            return Ok(students);
        }

    }
    //public class Student
    //{
    //    public string Name { get; set; }
    //    public string Email { get; set; }
    //}
    //public static class AllStudentInfo
    //{
    //    public static List<Student> Students = new List<Student>()
    //    {
    //        new Student()
    //        {
    //            Email = "tapos.aa@gmail.com",
    //            Name = "tapos"
    //        },
    //        new Student()
    //        {
    //            Email = "subir@gmail.com",
    //            Name = "subir"
    //        }
    //    };

    //    public static List<Student> GetStudents()
    //    {
    //        return Students;
    //    }

    //    public static Student GetStudentByEmail(string email)
    //    {
    //        return Students.FirstOrDefault(x => x.Email == email);
    //    }
    //    public static List<Student> AddStudent(Student aStudent)
    //    {
    //        Students.Add(aStudent);
    //        return Students;

    //    }
    //}
}