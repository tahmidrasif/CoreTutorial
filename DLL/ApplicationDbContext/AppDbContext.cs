using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using DLL.Model;

namespace DLL.ApplicationDbContext
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions options):base(options)
        {

        }

        public DbSet<Student> Students { get; set; }
        //public DbSet<Department> Departments { get; set; }
    }
}
