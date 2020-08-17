using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BankAPI;
using BankModel;

namespace BankAPI.Models
{
    public class BankContext : DbContext
    {
        public BankContext(DbContextOptions<BankContext> options) : base(options)
        {

        }

        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<BankCustomer> BankCustomer { get; set; }
    }
}
