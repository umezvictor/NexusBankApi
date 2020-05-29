using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusBankApi.Models;



namespace NexusBankApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {

        private readonly MyDbContext _context;
        public HomeController(MyDbContext context)
        {
            _context = context;
        }


        [HttpGet("customers")]
        public async Task<IActionResult> GetAllCustomers()
        {
            try
            {
                var customers = await _context.Customers.ToListAsync();
                if(customers == null)
                {
                    return NotFound();
                }

                return Ok(customers);
            }
            catch (Exception)
            {
                return this.StatusCode(501);
            }
            
        }



        [HttpGet("transactions")]
        public async Task<IActionResult> GetAllTrasactions()
        {
            try
            {
                var transactions = await _context.Transactions.ToListAsync();

                if(transactions == null)
                {
                    return NotFound();
                }

                return Ok(transactions);
            }
            catch (Exception)
            {
                return this.StatusCode(501);
            }
           
        }



            

    public async Task<Customer> GetCustomerByAccountNumber(string account)
    {
        var customer = await Task.Run(()=> _context.Customers.Where(c => c.AccountNumber == account).ToList().FirstOrDefault());

        if (customer == null)
        {
                return null;
        }

        return customer;
    }

    




        [HttpPost]
        public ActionResult<Customer> AddCustomer(Customer customer)
        {
            //command is the request body
            try
            {
                _context.Customers.Add(customer);
                _context.SaveChanges();
                return Ok( new { message = "Customer has been created successfully"});
            }
            catch (Exception)
            {
                return this.StatusCode(501);
            }
            
           
        }




        //for deposit and withdrawal
        [HttpPost("dotransaction")]
        public async Task<ActionResult<Transaction>> DoTransaction(Transaction transaction)
        {
            try
            {
                if (transaction == null)
                {
                    return BadRequest();
                }

                if (ModelState.IsValid)
                {

                    //update the customers balance

                    //deposit
                    if (transaction.TransactionType == "deposit")
                    
                    {
                        //get customer by account number
                        var customerProfile = GetCustomerByAccountNumber(transaction.ToAccount).Result;

                        //var balance = customerProfile.Balance;
                        customerProfile.Balance = customerProfile.Balance + transaction.Amount;

                        //update customer profile
                        _context.Entry(customerProfile).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        
                    }

                    //withdrawal
                    if (transaction.TransactionType == "withdrawal")
                    {

                        //get customer by account number
                        var customerProfile = await GetCustomerByAccountNumber(transaction.FromAccount);

                        customerProfile.Balance = customerProfile.Balance - transaction.Amount;

                        //update customer profile
                        _context.Entry(customerProfile).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }

                    //save transaction


                    _context.Transactions.Add(transaction);
                   await _context.SaveChangesAsync();

                    return Ok();

                }
                return BadRequest();


            }
            catch (Exception)
            {
                return this.StatusCode(501);
            }

        }

        


        //for transfers
        [HttpPost("transfer")]
        public async Task<ActionResult<Transaction>> DoTransfer(Transaction transaction)
        {
            try
            {
                if (transaction == null)
                {
                    return BadRequest();
                }

                if (ModelState.IsValid)
                {
                    //transfer algo
                    //get sender account number and check if it is valid else throw 
                    //check if account balance is >=  -- debit
                    //get recipient and do same validation

                    var sender = await GetCustomerByAccountNumber(transaction.FromAccount);

                    var recipient = await GetCustomerByAccountNumber(transaction.ToAccount);

                   
                    if (recipient == null) return NotFound(new { error = "invalid recipient" });

                    if (sender == null) return NotFound(new { error = "invalid sender" });

                    //check accout
                    if (sender.Balance >= transaction.Amount)
                    {
                        //debit sender
                        sender.Balance = sender.Balance - transaction.Amount;

                        _context.Entry(sender).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                       

                        //credit recipient
                        recipient.Balance = recipient.Balance + transaction.Amount;
                        _context.Entry(recipient).State = EntityState.Modified;
                        await _context.SaveChangesAsync();


                        return Ok(new { message = "transfer successful" });
                    }
                    else
                    {
                        return BadRequest(new { error = "insufficient balance" });
                    }
                }

                return BadRequest(new { error = "invalid transaction object" });
            }
            catch (Exception)
            {
                return this.StatusCode(501);
            }
        }


    }
}
