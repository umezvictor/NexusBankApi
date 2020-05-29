using NexusBankApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexusBankApi.Repository.Interfaces
{
    //this is an individual repository interface, that will inherit the irepository base
    public interface ITransactionRepository : IRepositoryBase<Transaction>
    {
        void Deposit(Transaction transaction);
        void Withdrawal(Transaction transaction);
        void Transfer(Transaction transaction);
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();

    }
}
