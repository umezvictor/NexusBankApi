using NexusBankApi.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexusBankApi.Repository
{
    //this is the the repository wrapper, also called unit of work -- IUnitOfWork
    public interface IRepositoryWrapper
    {
        //here we will bring all the individual repository interfaces
        ICustomerRepository  Customer { get; }
        IAccountRepository Account { get; }
        ITransactionRepository Transaction { get; }
        Task SaveAsync();
    }
}
