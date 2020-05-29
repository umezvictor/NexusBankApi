using NexusBankApi.Models;
using NexusBankApi.Repository.Interfaces;
using NexusBankApi.Repository.RepositoryClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexusBankApi.Repository
{
    /*
     creating properties that will expose the concrete repositories and
     also we have the Save() method to be used after all the modifications are finished on a 
     certain object. This is a good practice because now we can, for example, add two  Customers, 
     modify two accounts and delete one owner, all in one method, and 
     then just call the Save method once. All changes will be applied or if something fails,
     all changes will be reverted:
    */
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private AppDbContext _dbContext;

        //private readonly AppDbContext _dbContext;

        //bring in all individual repositories amd dbContext for db connection



        private IAccountRepository account;

        private ICustomerRepository customer;

        private ITransactionRepository transaction;


        public RepositoryWrapper(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //this class will be injected inside each controller that needs to access database logic
        //it is also called the unit of work class
        public ICustomerRepository Customer
        {
            get
            {
                if(customer == null)
                {
                    customer = new CustomerRepository(_dbContext);
                }
                return customer;
            }

            
        }


        public IAccountRepository Account
        {
            get
            {
                if (account == null)
                {
                    account = new AccountRepository(_dbContext);
                }
                return account;
            }


        }

        public ITransactionRepository Transaction
        {
            get
            {
                if (transaction == null)
                {
                    transaction = new TransactionRepository(_dbContext);
                }
                return transaction;
            }


        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
