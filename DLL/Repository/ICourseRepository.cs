using DLL.ApplicationDbContext;
using DLL.Model;
using DLL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Repository
{
    public interface ICourseRepository:IBaseRepository<Course> 
    {
    }

    public class CourseRepository:BaseRepository<Course>,ICourseRepository
    {
        public CourseRepository(AppDbContext context):base(context)
        {

        }
    }
}
