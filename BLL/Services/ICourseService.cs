using BLL.Request;
using DLL.Model;
using DLL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace BLL.Services
{
    public interface ICourseService
    {
        Task<Course> AddCourseAsync(CourseInsertRequest request);
        Task<List<Course>> GetAllCourseAsync();
        Task<Course> FindACourseAsync(string code);
        Task<bool> DeleteCourseAsync(string code);
        Task<Course> UpdateCourseAsync(string code, CourseInsertRequest course);
    }
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork unitOfWork;

        public CourseService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Course> AddCourseAsync(CourseInsertRequest request)
        {
            Course course = new Course()
            {
                Code = request.Code,
                Name = request.Name
            };

            await unitOfWork.CourseRepository.InsertAsync(course);

            if (await unitOfWork.DbSaveChangeAsync() == true)
            {
                return course;
            }
            throw new ExceptionManagementHelper("Data Insert Unsuccessfully");
            // return await unitOfWork.CourseRepository.InsertAsync(course);
        }

        public async Task<bool> DeleteCourseAsync(string code)
        {
            var course = await unitOfWork.CourseRepository.GetSingleAsync(x => x.Code == code);
            if (course == null)
            {
                throw new ExceptionManagementHelper("Roll wise Student not found");
            }

            unitOfWork.CourseRepository.DeleteAsync(course);
            if (await unitOfWork.DbSaveChangeAsync())
            {
                return true;
            }

            return false;
        }

        public async Task<Course> FindACourseAsync(string code)
        {
            return await unitOfWork.CourseRepository.GetSingleAsync(x => x.Code == code);
        }

        public async Task<List<Course>> GetAllCourseAsync()
        {
            return await unitOfWork.CourseRepository.GetAllAsync();
        }

        public async Task<Course> UpdateCourseAsync(string code, CourseInsertRequest course)
        {
            var objcourse = await unitOfWork.CourseRepository.GetSingleAsync(x => x.Code == code);

            if (objcourse == null)
            {
                throw new ExceptionManagementHelper("No found Data");
            }

            objcourse.Name = course.Name;
           
            unitOfWork.CourseRepository.Update(objcourse);


            if (await unitOfWork.DbSaveChangeAsync())
            {
                return objcourse;
            }
            throw new ExceptionManagementHelper("Student Data Not save");
        }
    }
}
