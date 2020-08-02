using Microsoft.AspNetCore.Razor.Language;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class ApplicationContext : DbContext
    {
        private static bool needDelete = false;
        public DbSet<Category> Categories { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<PackageReview> Reviews { get; set; }


        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            if (needDelete)
            {
                Database.EnsureDeleted();
                needDelete = false;
            }
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PackageReview>().Property(r => r.Positive).HasDefaultValue(-1);
            modelBuilder.Entity<Category>().HasData(
                new Category[]
                {
                    new Category{ Id = 1,Identificator = -1, Name="Без категории"},
                new Category { Id=2, Identificator = 0,Name="Баги"},
                new Category { Id=3, Identificator = 1,Name="Проблема с уведомлениями"},
                new Category { Id=4, Identificator = 2,Name="Прочее"},
                new Category { Id=5, Identificator = 3,Name="Платежи"}
     });
        }
    }
}
