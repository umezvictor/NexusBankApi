using Microsoft.EntityFrameworkCore;
using NexusBankApi.Models;
using NexusBankApi.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexusBankApi.Repository.RepositoryClasses
{
    //this class derives from RepositoryBase class, in that 
    //it calls the methods in that class
    //it also implements its own ICustomerRepository interface
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        //requires AppDbcontext since RepositoryBase uses it in its constructor
        public CustomerRepository(AppDbContext dbContext) : base(dbContext)
        {

        }


        //these methods implement the methods in the abstract class -- RepositoryBase.cs
        public void CreateCustomer(Customer customer)
        {
            Create(customer);
        }

        public void DeleteCustomer(Customer customer)
        {
            Delete(customer);
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            
            return await GetAll().ToListAsync();

        }

        //get customer by id
        public async Task<Customer> GetCustomerByIdAsync(int Id)
        {
            return await FindByCondition(customer => customer.CustomerId.Equals(Id)).FirstOrDefaultAsync();
        }

        //get customer by account number
        public async Task<Customer> GetCustomerByAccountNumberAsync(string AccountNumber)
        {
            return await FindByCondition(customer => customer.AccountNumber.Equals(AccountNumber)).FirstOrDefaultAsync();
        }

        //get customer by phone
        public async Task<Customer> GetCustomerByPhoneAsync(string Phone)
        {
            return await FindByCondition(customer => customer.Phone.Equals(Phone)).FirstOrDefaultAsync();
        }

        //get customer by email
        public async Task<Customer> GetCustomerByEmailAsync(string Email)
        {
            return await FindByCondition(customer => customer.Email.Equals(Email)).FirstOrDefaultAsync();
        }

        public void UpdateCustomer(Customer customer)
        {
            Update(customer);
        }

    }
}
