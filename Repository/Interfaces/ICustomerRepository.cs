using NexusBankApi.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexusBankApi.Repository.Interfaces
{
   // interface implements another interface
    public interface ICustomerRepository : IRepositoryBase<Customer>
    {
        //in this interface we extend (uncensored) the IReposioryBase interface
        //in CustomerrRepository class we then call the methods
        //in IRepositoryBase
        Task<IEnumerable<Customer>>  GetAllCustomersAsync();
        Task<Customer> GetCustomerByIdAsync(int Id);
        Task<Customer> GetCustomerByEmailAsync(string Email);
        Task<Customer> GetCustomerByAccountNumberAsync(string AccountNumber);
        Task<Customer> GetCustomerByPhoneAsync(string Phone);
        void CreateCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(Customer customer);
       
    }
}
