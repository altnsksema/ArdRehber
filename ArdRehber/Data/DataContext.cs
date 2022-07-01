 using ArdRehber.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArdRehber.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options) { }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Department> Departments { get; set; }
        //public DbSet<UserType> UserTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<TitleDepartment> TitleDepartments { get; set; }

        



    }
}
