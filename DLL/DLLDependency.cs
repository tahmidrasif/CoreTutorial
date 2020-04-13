using DLL.Model;
using DLL.Repository;
using DLL.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL
{
    public static class DLLDependency
    {
        public static void ALLDependency(IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork.UnitOfWork>();
            //services.AddTransient<IStudentRepository, StudentRepository>();
            //services.AddTransient<IBaseRepository<Student>, StudentRepository>();
        }
    }
}
