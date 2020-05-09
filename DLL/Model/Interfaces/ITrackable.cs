using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Model.Interfaces
{
    public interface ITrackable
    {
        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
