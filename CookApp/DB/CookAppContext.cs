using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CookApp.DB;

public partial class CookAppContext : DbContext
{
    public CookAppContext()
    {
    }

    public CookAppContext(DbContextOptions<CookAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Dish> Dishes { get; set; }

    public virtual DbSet<DishInOrder> DishInOrders { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeType> EmployeeTypes { get; set; }

    public virtual DbSet<Executive> Executives { get; set; }

    public virtual DbSet<Login> Logins { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DishInOrder>(entity =>
        {
            entity.HasIndex(e => e.DishId, "IX_DishInOrders_DishId");

            entity.HasIndex(e => e.OrderId, "IX_DishInOrders_OrderId");

            entity.HasOne(d => d.Dish).WithMany(p => p.DishInOrders).HasForeignKey(d => d.DishId);

            entity.HasOne(d => d.Order).WithMany(p => p.DishInOrders).HasForeignKey(d => d.OrderId);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasIndex(e => e.EmployeeTypeId, "IX_Employees_EmployeeTypeId");

            entity.HasOne(d => d.EmployeeType).WithMany(p => p.Employees).HasForeignKey(d => d.EmployeeTypeId);
        });

        modelBuilder.Entity<Executive>(entity =>
        {
            entity.HasIndex(e => e.DishInOrderId, "IX_Executives_DishInOrderId");

            entity.HasIndex(e => e.EmployeeId, "IX_Executives_EmployeeId");

            entity.HasOne(d => d.DishInOrder).WithMany(p => p.Executives).HasForeignKey(d => d.DishInOrderId);

            entity.HasOne(d => d.Employee).WithMany(p => p.Executives).HasForeignKey(d => d.EmployeeId);
        });

        modelBuilder.Entity<Login>(entity =>
        {
            entity.HasIndex(e => e.EmployeeId, "IX_Logins_EmployeeId").IsUnique();

            entity.HasOne(d => d.Employee).WithOne(p => p.Login).HasForeignKey<Login>(d => d.EmployeeId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
