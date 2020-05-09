using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using DLL.Model;
using DLL.Model.Interfaces;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Linq;
using DLL.ViewModel;

namespace DLL.ApplicationDbContext
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int, IdentityUserClaim<int>, IdentityUserRole<int>, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public AppDbContext(DbContextOptions options, IHttpContextAccessor httpAccessor) : base(options)
        {
            this._httpAccessor = httpAccessor;
        }
        private static readonly MethodInfo _propertyMethod = typeof(EF)
          .GetMethod(nameof(EF.Property), BindingFlags.Static | BindingFlags.Public)?.MakeGenericMethod(typeof(bool));
        private readonly IHttpContextAccessor _httpAccessor;
        private const string IsDeletedProperty = "IsDeleted";
        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<CustomerBalance> CustomerBalances { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<CourseStudent> CourseStudents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDelete).IsAssignableFrom(entity.ClrType))
                {
                    entity.AddProperty(IsDeletedProperty, typeof(bool));
                    modelBuilder.Entity(entity.ClrType).HasQueryFilter(GetIsDeletedRestriction(entity.ClrType));
                }
            }
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CourseStudent>()
        .HasKey(bc => new { bc.CourseId, bc.StudentId });
            modelBuilder.Entity<CourseStudent>()
                .HasOne(bc => bc.Course)
                .WithMany(b => b.CourseStudents)
                .HasForeignKey(bc => bc.CourseId);
            modelBuilder.Entity<CourseStudent>()
                .HasOne(bc => bc.Student)
                .WithMany(c => c.CourseStudents)
                .HasForeignKey(bc => bc.StudentId);


        }


        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            BeforeSaveChanges();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        private void BeforeSaveChanges()
        {
            var entries = ChangeTracker.Entries();
            string userEmail = GetCurrentUserEmail();

            foreach (var entity in entries)
            {
                var nowTime = DateTime.Now;
                if (entity.Entity is ITrackable trackable)
                {
                    switch (entity.State)
                    {
                        case EntityState.Added:
                            trackable.CreatedAt = nowTime;
                            trackable.UpdatedAt = nowTime;
                            trackable.CreatedBy = userEmail;
                            trackable.UpdatedBy = userEmail;
                            break;
                        case EntityState.Modified:
                            trackable.UpdatedAt = nowTime;
                            trackable.UpdatedBy = userEmail;
                            break;
                        case EntityState.Deleted:
                            entity.Property(IsDeletedProperty).CurrentValue = true;
                            entity.State = EntityState.Modified;
                            trackable.UpdatedAt = nowTime;
                            trackable.UpdatedBy = userEmail;
                            break;
                    }
                }
            }
        }

        private string GetCurrentUserEmail()
        {
            var httpContext = _httpAccessor.HttpContext;
            if (httpContext != null)
            {
                return httpContext.User.FindFirst(CustomJwtClaimName.UserName)?.Value;

            }
            return "";
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = new CancellationToken())
        {
            BeforeSaveChanges();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private static LambdaExpression GetIsDeletedRestriction(Type type)
        {
            var parm = Expression.Parameter(type, "it");
            var prop = Expression.Call(_propertyMethod, parm, Expression.Constant(IsDeletedProperty));
            var condition = Expression.MakeBinary(ExpressionType.Equal, prop, Expression.Constant(false));
            var lambda = Expression.Lambda(condition, parm);
            return lambda;
        }


    }
}
