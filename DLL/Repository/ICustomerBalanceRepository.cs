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
    public interface ICustomerBalanceRepository : IBaseRepository<CustomerBalance> 
    {
        Task UpdateCustomerBalanceAsync(decimal amount);
    }

    public class CustomerBalanceRepository : BaseRepository<CustomerBalance>, ICustomerBalanceRepository
    {
        private readonly AppDbContext _context;

        public CustomerBalanceRepository(AppDbContext context) :base(context)
        {
            _context = context;
        }

        public async Task UpdateCustomerBalanceAsync(decimal amount)
        {
            var custbalance = await _context.CustomerBalances.FirstOrDefaultAsync(x => x.CustomerBalanceId == 1);
            custbalance.Balance += amount;
            _context.CustomerBalances.Update(custbalance);
            var saved = false;

            do
            {
                try
                {
                    if(await _context.SaveChangesAsync() > 0)
                    {
                        saved = true;
                    }
                    // Attempt to save changes to the database
                     
                    
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is CustomerBalance)
                        {
                            var databaseEntry = entry.GetDatabaseValues();
                            var databaseValue = (CustomerBalance)databaseEntry.ToObject();

                            databaseValue.Balance += amount;
                            entry.OriginalValues.SetValues(databaseEntry);
                            entry.CurrentValues.SetValues(databaseValue);
                        }
                        
                    }
                }
            }
            while (!saved);
            
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