using NexusBankApi.Models;
using NexusBankApi.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexusBankApi.Repository.RepositoryClasses
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        
        public AccountRepository(AppDbContext dbContext) : base(dbContext)
        {
           
        }
    }
}
