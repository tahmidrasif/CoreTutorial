﻿using DLL.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Model
{
    public class Course : ITrackable, ISoftDelete
    {
        public int CourseId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public ICollection<CourseStudent> CourseStudents { get; set; }
    }
}
