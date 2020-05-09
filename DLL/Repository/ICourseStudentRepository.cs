using DLL.ApplicationDbContext;
using DLL.Model;
using DLL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Repository
{
    public interface ICourseStudentRepository : IBaseRepository<CourseStudent>
    {
    }
    public class CourseStudentRepository : BaseRepository<CourseStudent>, ICourseStudentRepository
    {
        public CourseStudentRepository(AppDbContext context) : base(context)
        {

        }
    }

}
