﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BankAPI;
using BankModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAPI.Models
{
    public class BankContext : DbContext
    {
        public BankContext(DbContextOptions<BankContext> options) : base(options)
        {
            // This is where we would use migrations if that shit wasn't so bugged
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("Filename=Bank.db");
        //}

        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<BankCustomer> BankCustomer { get; set; }
        public DbSet<BankTransaction> BankTransaction { get; set; }
    }
}
