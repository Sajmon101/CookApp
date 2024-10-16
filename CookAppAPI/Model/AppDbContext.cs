using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookAppAPI.Model
{
    class AppDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Executive> Executives { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<EmployeeType> EmployeeTypes { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<DishInOrder> DishInOrders { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-THDQ277\SQLEXPRESS;Initial Catalog=CookApp;Integrated Security=True; TrustServerCertificate=True");
        }
    }
}
