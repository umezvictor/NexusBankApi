using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using NexusBankApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NexusBankApi.Dto;
using NexusBankApi.Repository;

namespace NexusBankApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {


        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;

        public TransactionsController(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }


        //get all transactions 
        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            try
            {
                var transactions = await _repoWrapper.Transaction.GetAllTransactionsAsync();
                if (transactions == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(transactions);
                }

            }
            catch (Exception)
            {
                return this.StatusCode(501);
            }
        }


        //transafer
        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer(TransactionDto transaction)
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

                    var sender = await _repoWrapper.Customer.GetCustomerByAccountNumberAsync(transaction.FromAccountNumber);
                    
                    var recipient = await _repoWrapper.Customer.GetCustomerByAccountNumberAsync(transaction.ToAccountNumber);
                    
                    if (recipient == null) return NotFound(new { error = "invalid recipient" });
                    
                    if (sender == null) return NotFound(new { error = "invalid sender" });

                    //check accout
                    if (sender.Balance >= transaction.Amount)
                    {
                        //debit sender
                        sender.Balance = sender.Balance - transaction.Amount;
                        _repoWrapper.Customer.Update(sender);
                        await _repoWrapper.SaveAsync();

                        //credit recipient
                        recipient.Balance = recipient.Balance + transaction.Amount;
                        _repoWrapper.Customer.Update(recipient);
                        await _repoWrapper.SaveAsync();

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

        [HttpPost]
        public async Task<IActionResult> Deposit(TransactionDto transaction)
        {
            try
            {
                if(transaction == null)
                {
                    return BadRequest();
                }

                if (ModelState.IsValid)
                {
                   
                    //update the customers balance

                    //deposit
                    if(transaction.TransactionType == "deposit")
                    {
                        //get customer by account number
                        var customerProfile = await _repoWrapper.Customer.GetCustomerByAccountNumberAsync(transaction.ToAccountNumber);

                        customerProfile.Balance = customerProfile.Balance + transaction.Amount;
                        _repoWrapper.Customer.Update(customerProfile);
                        await _repoWrapper.SaveAsync();
                    }
                  
                    //withdrawal
                    if(transaction.TransactionType == "withdrawal")
                    {
                        //get customer by account number
                        var customerProfile = await _repoWrapper.Customer.GetCustomerByAccountNumberAsync(transaction.FromAccountNumber);

                        customerProfile.Balance = customerProfile.Balance - transaction.Amount;
                        _repoWrapper.Customer.Update(customerProfile);
                        await _repoWrapper.SaveAsync();
                    }

                    //save transaction

                    var transactionDetails = _mapper.Map<Transaction>(transaction);
                    _repoWrapper.Transaction.Create(transactionDetails);
                    await _repoWrapper.SaveAsync();
                    return Ok();

                }
                return BadRequest();


            }
            catch (Exception)
            {
                return this.StatusCode(501);
            }
            
        }
    }
}