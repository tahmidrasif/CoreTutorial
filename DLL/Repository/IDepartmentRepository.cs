using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DLL.ApplicationDbContext;

using DLL.Model;
using DLL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository
{
    public interface IDepartmentRepository : IBaseRepository<Department>
    {
       
    }
    public class DepartmentRepository: BaseRepository<Department>,IDepartmentRepository
    {
        public DepartmentRepository(AppDbContext context) : base(context)
        {
            
        }
    }
}