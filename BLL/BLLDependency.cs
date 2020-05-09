using BLL.Request;
using BLL.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public static class BLLDependency
    {
        public static void ALLDependency(IServiceCollection services) 
        {

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost";
                options.InstanceName = "PracticeDb";
            });

            services.AddTransient<IDepartmentService, DepartmentService>();
            services.AddTransient<IStudentService, StuedntService>();
            services.AddTransient<ICourseService, CourseService>();          
            services.AddTransient<ITestService, TestService>();
            services.AddTransient<IAccountService, AccountService>();

            ALLValidationDependency(services);
        }

        private static void ALLValidationDependency(IServiceCollection services)
        {
            services.AddTransient<IValidator<DepartInsertRequest>, DepartInsertRequestValidator>();
            services.AddTransient<IValidator<StudentRequest>, StudentRequestValidator>();
        }
    }
}
