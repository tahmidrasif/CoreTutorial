using DLL.ApplicationDbContext;
using DLL.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using DLL.UnitOfWork;
using System.Linq.Expressions;

namespace DLL.Repository
{
    public interface IStudentRepository:IBaseRepository<Student> 
    {
       
    }

    public class StudentRepository : BaseRepository<Student>,IStudentRepository
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context) :base(context)
        {

        }

        
    }
}





//public interface IStudentRepository
//{
//    public Task<Student> AddStudentAsync(Student student);
//    public Task<List<Student>> GetAllStudentAsync();
//    public Task<Student> GetStudentByEmailAsync(string email);
//    Task<bool> IsEmailExist(string email);
//}

//public class StudentRepository : IStudentRepository
//{
//    private readonly AppDbContext _context;

//    public StudentRepository(AppDbContext context)
//    {
//        _context = context;
//    }

//    public async Task<Student> AddStudentAsync(Student student)
//    {
//        await _context.Students.AddAsync(student);
//        await _context.SaveChangesAsync();
//        return student;
//    }

//    public async Task<List<Student>> GetAllStudentAsync()
//    {
//        return await _context.Students.ToListAsync();
//    }

//    public async Task<Student> GetStudentByEmailAsync(string email)
//    {
//        return await _context.Students.FirstOrDefaultAsync(x => x.Email == email);
//    }

//    public async Task<bool> IsEmailExist(string email)
//    {
//        var student = await _context.Students.FirstAsync(x => x.Email == email);
//        if (student == null)
//        {
//            return false;
//        }
//        return true;
//    }
//}