using DLL.Model.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Model
{
    public class AppRole : IdentityRole<int>, ITrackable, ISoftDelete
    {
        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public static implicit operator Task<object>(AppRole v)
        {
            throw new NotImplementedException();
        }
    }
}
