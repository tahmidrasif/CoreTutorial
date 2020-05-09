using BLL.Request;
using BLL.Response;
using DLL.Model;
using DLL.Repository;
using DLL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;
namespace BLL.Services
{
    public interface IStudentService
    {
        public Task<Student> AddStudentAsync(StudentRequest request);
        public Task<List<Student>> GetAllStudentAsync();

        public Task<Student> GetStudentByEmailAsync(string email);

        public List<StudentReport> GetAllStudentReport();
        public Task<List<StudentCourseReport>> GetAllStudentCourseReport();

        public Task<bool> IsEmailExist(string email);
        Task<Student> UpdateAsync(string roll, StudentUpdateRequest request);
        Task<bool> DeleteAsync(string roll);
        Task<bool> IsRollExit(string roll);
    }

    public class StuedntService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StuedntService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            //this._unitOfWork = repository;
        }
        public async Task<Student> AddStudentAsync(StudentRequest request)
        {
            Student oStudent = new Student();
            oStudent.Name = request.Name;
            oStudent.Email = request.Email;
            Department dept = await _unitOfWork.DepartmentRepository.GetSingleAsync(x => x.DepartmentId == request.DepartmentId);
            if (dept == null)
            {
                throw new ExceptionManagementHelper("Data Insert Unsuccessfully");
            }
            oStudent.Department = dept;
            await _unitOfWork.StudentRepository.InsertAsync(oStudent);

            if (await _unitOfWork.DbSaveChangeAsync() == true)
            {
                return oStudent;
            }
            throw new ExceptionManagementHelper("Data Insert Unsuccessfully");
        }

        public async Task<List<Student>> GetAllStudentAsync()
        {
            return await _unitOfWork.StudentRepository.GetAllAsync();

        }

        public async Task<Student> GetStudentByEmailAsync(string email)
        {
            var student = await _unitOfWork.StudentRepository.GetSingleAsync(x => x.Email == email);
            if (student == null)
            {
                throw new ExceptionManagementHelper("Data Not Found");
            }
            return student;
            //throw new NotImplementedException();
        }

        public async Task<bool> IsEmailExist(string email)
        {
            var student = await _unitOfWork.StudentRepository.GetSingleAsync(x => x.Email == email);
            if (student == null)
                return false;
            return true;

        }

        public async Task<bool> DeleteAsync(string roll)
        {
            var student = await _unitOfWork.StudentRepository.GetSingleAsync(x => x.RollNo == roll);
            if (student == null)
            {
                throw new ExceptionManagementHelper("Roll wise Student not found");
            }

            _unitOfWork.StudentRepository.DeleteAsync(student);
            if (await _unitOfWork.DbSaveChangeAsync())
            {
                return true;
            }

            return false;

        }

        public async Task<bool> IsRollExit(string roll)
        {
            var student = await _unitOfWork.StudentRepository.GetSingleAsync(x => x.RollNo == roll);
            if (student != null)
            {
                return false;
            }

            return true;
        }

        public async Task<Student> UpdateAsync(string roll, StudentUpdateRequest request)
        {

            var student = await _unitOfWork.StudentRepository.GetSingleAsync(x => x.RollNo == roll);

            if (student == null)
            {
                throw new ExceptionManagementHelper("No found Data");
            }

            student.Name = request.Name;
            student.RollNo = request.RollNo;
            student.Email = request.Email;
            _unitOfWork.StudentRepository.Update(student);


            if (await _unitOfWork.DbSaveChangeAsync())
            {
                return student;
            }
            throw new ExceptionManagementHelper("Student Data Not save");
        }

        public List<StudentReport> GetAllStudentReport()
        {
            var students = _unitOfWork.StudentRepository.QueryAll().Include(x => x.Department).ToList();
            var result = new List<StudentReport>();
            foreach (var student in students)
            {
                result.Add(new StudentReport()
                {
                    StudentName = student.Name,
                    Roll = student.RollNo,
                    DepartmentName = student.Department.Name,
                    Code = student.Department.Code
                });

            }
            return result;
        }

        public async Task<List<StudentCourseReport>> GetAllStudentCourseReport()
        {
            List<StudentCourseReport> lists = new List<StudentCourseReport>();
            //throw new NotImplementedException();
            var students =   _unitOfWork.StudentRepository.QueryAll().Include(x=>x.CourseStudents).ToList();
            
            foreach (var student in students)
            {
                List<CourseResponse> courseResponses = new List<CourseResponse>();
                foreach (var item in student.CourseStudents)
                {
                   
                    var courses =await _unitOfWork.CourseRepository.GetSingleAsync(x=>x.CourseId==item.CourseId);
                    courseResponses.Add(new CourseResponse()
                    {
                        Code = courses == null? "": courses.Code,
                        Name= courses == null ? "" : courses.Name,
                    });
                }
                //var coursstudentList = await _unitOfWork.CourseStudentRepository.GetAllAsync(x => x.StudentId == student.StudentId);
                //var courselist = new List<Course>();

                //foreach (var item in coursstudentList)
                //{
                //    var course =  await _unitOfWork.CourseRepository.GetSingleAsync(x => x.CourseId == item.CourseId);
                //    if (course != null)
                //    {
                //        courselist.Add(course);
                //    }
                //}

                lists.Add(
                     new StudentCourseReport()
                     {
                         Name = student.Name,
                         Roll = student.RollNo,
                         Courses = courseResponses
                     });
               


            }
            return lists;

        }
    }
}
