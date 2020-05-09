using DLL.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Model
{
    public class Department: ISoftDelete,ITrackable
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public ICollection<Student> Students { get; set; }
    }
}
