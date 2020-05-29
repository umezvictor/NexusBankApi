using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexusBankApi.Models
{
    //AppUser class extends the inbuilt IdentityUser class, hence we usee it in place of IedentityUser
    public class MyDbContext : DbContext
    {
        //dentityDbContext<AppUser> -- tells IdentityDbContext to use AppUser
        // 
        public MyDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
