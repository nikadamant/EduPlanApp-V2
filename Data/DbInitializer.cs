using Microsoft.EntityFrameworkCore;
using EduPlanApp.Models;
using System;
using System.Linq;

namespace EduPlanApp.Data
{
    public static class DbInitializer
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // 1. Початкові дані для Priorities (Довідник 1)
            var priorities = new Priority[]
            {
                new Priority { Id = 1, Name = "Високий", Level = 1 },
                new Priority { Id = 2, Name = "Середній", Level = 2 },
                new Priority { Id = 3, Name = "Низький", Level = 3 }
            };
            modelBuilder.Entity<Priority>().HasData(priorities);

            // 2. Початкові дані для Categories (Довідник 2)
            var categories = new Category[]
            {
                new Category { Id = 1, Name = "Лабораторні роботи", Description = "Завдання, які потребують програмування" },
                new Category { Id = 2, Name = "Теоретичні предмети", Description = "Читання, конспекти та підготовка до іспитів" },
                new Category { Id = 3, Name = "Особисті проекти", Description = "Розвиток портфоліо та навичок" }
            };
            modelBuilder.Entity<Category>().HasData(categories);

            // 3. Початкові дані для TaskItems (Центральна таблиця)
            modelBuilder.Entity<TaskItem>().HasData(
                new TaskItem
                {
                    Id = 1,
                    Title = "Лаб. 4: Налаштування ORM",
                    Description = "Створити моделі, контекст та налаштувати 4 типи БД (SQL, Postgres, SQLite, In-Memory)",
                    CreatedDate = DateTime.Parse("2025-11-28"),
                    DueDate = DateTime.Parse("2025-12-05 18:00:00"),
                    IsCompleted = false,
                    CategoryId = 1, // Лабораторні роботи
                    PriorityId = 1  // Високий
                },
                new TaskItem
                {
                    Id = 2,
                    Title = "Читання: Паттерни проектування",
                    Description = "Розділ 3: Factory Method та Abstract Factory",
                    CreatedDate = DateTime.Parse("2025-11-30"),
                    DueDate = DateTime.Parse("2025-12-03 10:00:00"),
                    IsCompleted = false,
                    CategoryId = 2, // Теоретичні предмети
                    PriorityId = 2  // Середній
                },
                new TaskItem
                {
                    Id = 3,
                    Title = "План: Новий проект на React",
                    Description = "Скласти список технологій та архітектуру для пет-проекту",
                    CreatedDate = DateTime.Parse("2025-11-25"),
                    DueDate = DateTime.Parse("2025-12-10 23:59:00"),
                    IsCompleted = true,
                    CategoryId = 3, // Особисті проекти
                    PriorityId = 3  // Низький
                },
                new TaskItem
                {
                    Id = 4,
                    Title = "Підготовка до семінару з Філософії",
                    Description = "Тема 5: Етика та штучний інтелект. Зібрати джерела.",
                    CreatedDate = DateTime.Parse("2025-12-01"),
                    DueDate = DateTime.Parse("2025-12-08 14:00:00"),
                    IsCompleted = false,
                    CategoryId = 2, // Теоретичні предмети
                    PriorityId = 1  // Високий
                }
            );
        }
    }
}