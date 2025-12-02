using Microsoft.EntityFrameworkCore;
using EduPlanApp.Models;
using System;
using System.Linq;

namespace EduPlanApp.Data
{
    // Клас для універсального заповнення даних, не прив'язаного до HasData()
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            // Перевіряємо, чи вже є дані. Це важливо, щоб уникнути помилок 
            // при повторному запуску або дублювання даних при кожному старті додатку.
            if (context.Priorities.Any() || context.Categories.Any() || context.TaskItems.Any())
            {
                return;
            }

            // 1. Початкові дані для Priorities
            var priorities = new Priority[]
            {
                new Priority { Id = 1, Name = "Високий", Level = 1 },
                new Priority { Id = 2, Name = "Середній", Level = 2 },
                new Priority { Id = 3, Name = "Низький", Level = 3 }
            };
            context.Priorities.AddRange(priorities);

            // 2. Початкові дані для Categories
            var categories = new Category[]
            {
                new Category { Id = 1, Name = "Лабораторні роботи", Description = "Завдання, які потребують програмування" },
                new Category { Id = 2, Name = "Теоретичні предмети", Description = "Читання, конспекти та підготовка до іспитів" },
                new Category { Id = 3, Name = "Особисті проекти", Description = "Розвиток портфоліо та навичок" }
            };
            context.Categories.AddRange(categories);
            
            // 3. Початкові дані для TaskItems
            var taskItems = new TaskItem[]
            {
                new TaskItem
                {
                    Id = 1,
                    Title = "Лаб. 4: Налаштування ORM",
                    Description = "Створити моделі, контекст та налаштувати 4 типи БД (SQL, Postgres, SQLite, In-Memory)",
                    // Використовуємо DateTimeOffset.Parse, як і раніше
                    CreatedDate = DateTimeOffset.Parse("2025-11-28"),
                    DueDate = DateTimeOffset.Parse("2025-12-05 18:00:00"),
                    IsCompleted = false,
                    CategoryId = 1, 
                    PriorityId = 1 
                },
                new TaskItem
                {
                    Id = 2,
                    Title = "Читання: Паттерни проектування",
                    Description = "Розділ 3: Factory Method та Abstract Factory",
                    CreatedDate = DateTimeOffset.Parse("2025-11-30"),
                    DueDate = DateTimeOffset.Parse("2025-12-03 10:00:00"),
                    IsCompleted = false,
                    CategoryId = 2, 
                    PriorityId = 2 
                },
                new TaskItem
                {
                    Id = 3,
                    Title = "План: Новий проект на React",
                    Description = "Скласти список технологій та архітектуру для пет-проекту",
                    CreatedDate = DateTimeOffset.Parse("2025-11-25"),
                    DueDate = DateTimeOffset.Parse("2025-12-10 23:59:00"),
                    IsCompleted = true,
                    CategoryId = 3, 
                    PriorityId = 3 
                },
                new TaskItem
                {
                    Id = 4,
                    Title = "Підготовка до семінару з Філософії",
                    Description = "Тема 5: Етика та штучний інтелект. Зібрати джерела.",
                    CreatedDate = DateTimeOffset.Parse("2025-12-01"),
                    DueDate = DateTimeOffset.Parse("2025-12-08 14:00:00"),
                    IsCompleted = false,
                    CategoryId = 2, 
                    PriorityId = 1 
                }
            };
            context.TaskItems.AddRange(taskItems);

            // Зберегти всі об'єкти в базу даних
            context.SaveChanges();
        }
    }
}