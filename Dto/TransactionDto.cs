using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NexusBankApi.Dto
{
    public class TransactionDto
    {
        public int TransactionId { get; set; }

        //Sterlingprong@2020
       // public string AccountNumber { get; set; }

        public string TransactionType { get; set; }


        //[Column(TypeName = "decimal(18,4)")]//will store 18 digits with 4 of it after the decimal
        public double Amount { get; set; }

        public string status { get; set; }

        public DateTime TransactionDate { get; set; }

        public string FromAccountNumber { get; set; }
        public string ToAccountNumber { get; set; }
    }
}
