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
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        //dentityDbContext<AppUser> -- tells IdentityDbContext to use AppUser
        // public AppDbContext(DbContextOptions options) : base(options)
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
