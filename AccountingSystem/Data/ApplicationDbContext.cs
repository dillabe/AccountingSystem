using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace AccountingSystem.Data
{
    // Разширяваме IdentityUser, за да добавим нови данни за потребителя
    public class User : IdentityUser
    {
        // Пълно име на потребителя
        public string FullName { get; set; }

        // Ниво на достъп: 0 = Гост, 1 = Потребител, 2 = Администратор
        public int RoleLevel { get; set; }
    }

    // Основен контекст на базата данни
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Таблици за сметки и транзакции
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Това е необходимо за Identity
            base.OnModelCreating(modelBuilder);

            // Настройки за таблица Accounts
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(a => a.Name)
                    .IsRequired()
                    .HasMaxLength(50); // Ограничение за името на сметката

                entity.Property(a => a.Balance)
                    .HasColumnType("decimal(18,2)"); // Дефинираме тип за баланса
            });

            // Настройки за таблица Transactions
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(t => t.Description)
                    .HasMaxLength(200); // Ограничение за дължината на описанието

                entity.Property(t => t.Amount)
                    .HasColumnType("decimal(18,2)") // Тип на числовото поле
                    .IsRequired(); // Полето е задължително

                entity.HasOne(t => t.Account)
                    .WithMany(a => a.Transactions)
                    .HasForeignKey(t => t.AccountId)
                    .OnDelete(DeleteBehavior.Cascade); // Изтриване на транзакции при изтриване на сметката
            });
        }
    }

    // Модел за сметка
    public class Account
    {
        public int Id { get; set; } // Уникален идентификатор

        public string Name { get; set; } // Име на сметката

        public decimal Balance { get; set; } // Баланс

        // Списък с транзакции към тази сметка
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }

    // Модел за транзакция
    public class Transaction
    {
        public int Id { get; set; } // Уникален идентификатор

        public decimal Amount { get; set; } // Сума на транзакцията

        public DateTime Date { get; set; } // Дата на транзакцията

        public string Description { get; set; } // Описание на транзакцията

        // Връзка с акаунт
        public int AccountId { get; set; } // Чужд ключ към сметката
        public virtual Account Account { get; set; } // Навигационна връзка към Account
    }
}
