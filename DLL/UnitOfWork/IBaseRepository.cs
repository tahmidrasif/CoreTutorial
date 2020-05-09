using DLL.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DLL.UnitOfWork
{
    public interface IBaseRepository<T> where T:class
    {
         Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression = null);
         Task<T> GetSingleAsync(Expression<Func<T,bool>> expression=null );
         Task InsertAsync(T entry);
         void Update(T entry);
         void DeleteAsync(T entry);
        IQueryable<T> QueryAll(Expression<Func<T, bool>> expression = null);
       

    }

    public class BaseRepository<T> : IBaseRepository<T> where T: class
    {
        private readonly AppDbContext _context;

        public BaseRepository(ApplicationDbContext.AppDbContext context)
        {
            this._context = context;
        }



        public void DeleteAsync(T entry)
        {
            _context.Set<T>().Remove(entry);
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression = null)
        {
            if (expression == null)
            {
                return await _context.Set<T>().ToListAsync();

            }
            return await _context.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> expression = null)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(expression);
        }

        public async Task InsertAsync(T entry)
        {
           await _context.Set<T>().AddAsync(entry);
        }

        public IQueryable<T> QueryAll(Expression<Func<T, bool>> expression = null)
        {
            if (expression == null)
            {
                return  _context.Set<T>().AsQueryable().AsTracking();

            }
            return  _context.Set<T>().Where(expression).AsQueryable().AsTracking();
        }

        public void Update(T entry)
        {
             _context.Set<T>().Update(entry);
        }
    }
}
