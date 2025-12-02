using EduPlanApp.Models;
using Microsoft.VisualBasic;
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
            // Примітка: Ми перевіряємо лише одну колекцію, оскільки вони мають вставлятися разом.
            if (context.Priorities.Any())
            {
                return;
            }

            // 1. Початкові дані для Priorities
            var highPriority = new Priority { Name = "Високий", Level = 1 };
            var mediumPriority = new Priority { Name = "Середній", Level = 2 };
            var lowPriority = new Priority { Name = "Низький", Level = 3 };

            var priorities = new Priority[]
            {
                highPriority,
                mediumPriority,
                lowPriority
            };
            context.Priorities.AddRange(priorities);

            // 2. Початкові дані для Categories
            var labCategory = new Category { Name = "Лабораторні роботи", Description = "Завдання, які потребують програмування" };
            var theoryCategory = new Category { Name = "Теоретичні предмети", Description = "Читання, конспекти та підготовка до іспитів" };
            var projectCategory = new Category { Name = "Особисті проекти", Description = "Розвиток портфоліо та навичок" };

            var categories = new Category[]
            {
                labCategory,
                theoryCategory,
                projectCategory
            };
            context.Categories.AddRange(categories);

            // 3. Початкові дані для TaskItems
            var taskItems = new TaskItem[]
            {
                new TaskItem
                {
                    Title = "Лаб. 4: Налаштування ORM",
                    Description = "Створити моделі, контекст та налаштувати 4 типи БД (SQL, Postgres, SQLite, In-Memory)",
                    // Використовуємо DateTimeOffset.Parse
                    CreatedDate = DateTimeOffset.Parse("2025-11-28"),
                    DueDate = DateTimeOffset.Parse("2025-12-05 18:00:00"),
                    IsCompleted = false,
                    Category = labCategory,
                    Priority = highPriority
                },
                new TaskItem
                {
                    Title = "Читання: Паттерни проектування",
                    Description = "Розділ 3: Factory Method та Abstract Factory",
                    CreatedDate = DateTimeOffset.Parse("2025-11-30"),
                    DueDate = DateTimeOffset.Parse("2025-12-03 10:00:00"),
                    IsCompleted = false,
                    Category = theoryCategory,
                    Priority = mediumPriority
                },
                new TaskItem
                {
                    Title = "План: Новий проект на React",
                    Description = "Скласти список технологій та архітектуру для пет-проекту",
                    CreatedDate = DateTimeOffset.Parse("2025-11-25"),
                    DueDate = DateTimeOffset.Parse("2025-12-10 23:59:00"),
                    IsCompleted = true,
                    Category = projectCategory,
                    Priority = lowPriority
                },
                new TaskItem
                {
                    Title = "Підготовка до семінару з Філософії",

                    Description = "Тема 5: Етика та штучний інтелект. Зібрати джерела.",
                    CreatedDate = DateTimeOffset.Parse("2025-12-01"),
                    DueDate = DateTimeOffset.Parse("2025-12-08 14:00:00"),
                    IsCompleted = false,
                    Category = theoryCategory,
                    Priority = highPriority
                }
            };
            context.TaskItems.AddRange(taskItems);

            // Зберегти всі об'єкти в базу даних.
            context.SaveChanges();
        }
    }
}