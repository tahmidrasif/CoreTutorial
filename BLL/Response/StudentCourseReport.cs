using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Response
{
    public class StudentCourseReport
    {
        public string Name { get; set; }
        public string Roll { get; set; }
        public List<CourseResponse> Courses { get; set; }

    }
}
