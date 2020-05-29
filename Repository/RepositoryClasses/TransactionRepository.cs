using Microsoft.EntityFrameworkCore;
using NexusBankApi.Models;
using NexusBankApi.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexusBankApi.Repository.RepositoryClasses
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        /*
         Now that I have the RepositoryBase class, I create the user classes that will 
         inherit this abstract class. Every user class will have its own interface, for additional 
         model-specific methods. Furthermore, by inheriting from the RepositoryBase class they will 
         have access to all the methods from the RepositoryBase. This way, we are separating the logic,
         that is common for all our repository user classes and also specific for every user class itself.
        */

        public TransactionRepository(AppDbContext dbContext) : base(dbContext)
        {

        }

        public void Deposit(Transaction transaction)
        {
            Create(transaction);
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            //throw new NotImplementedException();
            return await GetAll().ToListAsync();
        }

        public void Transfer(Transaction transaction)
        {
            Create(transaction);
        }

        public void Withdrawal(Transaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}
