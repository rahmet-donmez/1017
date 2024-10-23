using AccountManagment.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccountManagment.Data
{
    public static class SeedData
    {
        public static void Initialize(ModelBuilder modelBuilder)
        {
            // Kullanıcılar
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Rahmet", Surname = "Dönmez", Email = "rahmetdonmez@gmail.com", Phone = "05001234567", TcNo = "12345678901", Password = "Password1", City = "İstanbul", State = "İstanbul", Adress = "İstanbul Cad. No:1", IsAdmin = false },
                new User { Id = 2, Name = "Ayşe", Surname = "Kara", Email = "ayse.kara@example.com", Phone = "05007654321", TcNo = "10987654321", Password = "Password2", City = "Ankara", State = "Ankara", Adress = "Ankara Cad. No:2", IsAdmin = true },
                new User { Id = 3, Name = "Mehmet", Surname = "Demir", Email = "mehmet.demir@example.com", Phone = "05009876543", TcNo = "12345678902", Password = "Password3", City = "İzmir", State = "İzmir", Adress = "İzmir Cad. No:3", IsAdmin = false },
                new User { Id = 4, Name = "Fatma", Surname = "Çelik", Email = "fatma.celik@example.com", Phone = "05001234568", TcNo = "12345678903", Password = "Password4", City = "Bursa", State = "Bursa", Adress = "Bursa Cad. No:4", IsAdmin = false }
            );

            // Hesaplar
            modelBuilder.Entity<Account>().HasData(
                new Account { Id = 1, Number = "5678901", Iban = "TR123456789012345678901", Type = "Vadesiz", Balance = 1000, IsActive = true, UserId = 1 },
                new Account { Id = 2, Number = "6789012", Iban = "TR234567890123456789012", Type = "Vadesiz", Balance = 1500, IsActive = true, UserId = 1 },
                new Account { Id = 3, Number = "7890123", Iban = "TR345678901234567890123", Type = "Vadeli", Balance = 2000, IsActive = false, UserId = 2 },
                new Account { Id = 4, Number = "8901234", Iban = "TR456789012345678901234", Type = "Vadesiz", Balance = 2500, IsActive = true, UserId = 2 },
                new Account { Id = 5, Number = "9012345", Iban = "TR567890123456789012345", Type = "Vadesiz", Balance = 500, IsActive = true, UserId = 3 },
                new Account { Id = 6, Number = "0123456", Iban = "TR678901234567890123456", Type = "Vadeli", Balance = 7500, IsActive = true, UserId = 4 }
            );

            // Hesap İşlemleri
            modelBuilder.Entity<AccountTransaction>().HasData(
                new AccountTransaction { Id = 1, Amount = 500, Description = "Para Yatırma", Direction = true, AccountId = 1 },
                new AccountTransaction { Id = 2, Amount = 200, Description = "Para Çekme", Direction = false, AccountId = 1 },
                new AccountTransaction { Id = 3, Amount = 1000, Description = "Fatura Ödeme", Direction = false, AccountId = 2 },
                new AccountTransaction { Id = 4, Amount = 2500, Description = "Havale", Direction = true, AccountId = 3 },
                new AccountTransaction { Id = 5, Amount = 1500, Description = "Transfer", Direction = false, AccountId = 1,TransferId=1 },
                new AccountTransaction { Id = 8, Amount = 1500, Description = "Transfer", Direction = true, AccountId = 2 , TransferId =1},
                new AccountTransaction { Id = 6, Amount = 3000, Description = "Para Yatırma", Direction = true, AccountId = 5 },
                new AccountTransaction { Id = 7, Amount = 500, Description = "Para Çekme", Direction = false, AccountId = 6 }
            );

            // Transferler
            modelBuilder.Entity<Transfer>().HasData(
                new Transfer { Id = 1, Amount = 1000, Description = "Harçlık Transferi", TargetAccountId = 2, SourceAccountId = 1 }
            );
        }
    }
}
