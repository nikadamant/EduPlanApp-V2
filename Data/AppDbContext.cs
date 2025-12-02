using EduPlanApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace EduPlanApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Таблиці
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Priority> Priorities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Налаштування зв'язків
            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Tasks)
                .HasForeignKey(t => t.CategoryId);

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.Priority)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.PriorityId);

        }
    }
}