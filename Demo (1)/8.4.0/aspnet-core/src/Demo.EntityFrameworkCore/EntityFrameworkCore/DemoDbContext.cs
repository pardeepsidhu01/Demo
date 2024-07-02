using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Demo.Authorization.Roles;
using Demo.Authorization.Users;
using Demo.MultiTenancy;
using Castle.MicroKernel.Registration;

namespace Demo.EntityFrameworkCore
{
    public class DemoDbContext : AbpZeroDbContext<Tenant, Role, User, DemoDbContext>
    {
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Designation> Designations { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }

        public DemoDbContext(DbContextOptions<DemoDbContext> options)
            : base(options)
        {
        }
    }
}
