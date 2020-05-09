using DLL.ApplicationDbContext;
using DLL.Model;
using DLL.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DLL.UnitOfWork
{
    public interface IUnitOfWork
    {
        //All repo need to added here

        IDepartmentRepository DepartmentRepository { get; }
        IStudentRepository StudentRepository { get;  }
        ICustomerBalanceRepository CustomerBalanceRepository { get; }
        IOrderRepository OrderRepository { get; }
        ICourseRepository CourseRepository { get; }
        ICourseStudentRepository CourseStudentRepository { get; }


        //All repo need to added here
        Task<bool> DbSaveChangeAsync();
        void Dispose();

    }

    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _context;
        private bool disposed;
        //private IStudentRepository _studentRepository;


        private IDepartmentRepository _departmentRepository;
        private IStudentRepository _studentRepository;
        private ICustomerBalanceRepository _customerBalanceRepository;
        private IOrderRepository  _orderRepository;
        private ICourseRepository courseRepository;
        private ICourseStudentRepository coursestudentRepository;

        public ICourseRepository CourseRepository 
        {
            get
            {
               return courseRepository == null ? new CourseRepository(_context) : courseRepository;
            }
        }


        public ICourseStudentRepository CourseStudentRepository
        {
            get
            {
                return coursestudentRepository == null ? new CourseStudentRepository(_context) : coursestudentRepository;
            }
        }

        public IStudentRepository StudentRepository =>
            _studentRepository ??= new StudentRepository(_context);


        public IDepartmentRepository DepartmentRepository =>
            _departmentRepository ??= new DepartmentRepository(_context);

        public ICustomerBalanceRepository CustomerBalanceRepository => _customerBalanceRepository ??= new CustomerBalanceRepository(_context);

        public IOrderRepository OrderRepository => _orderRepository ??= new OrderRepository(_context);


        public UnitOfWork(AppDbContext context)
        {
            this._context = context;
        }
        public async Task<bool> DbSaveChangeAsync()
        {
            return await _context.SaveChangesAsync() > 0;

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                    _context.Dispose();
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
