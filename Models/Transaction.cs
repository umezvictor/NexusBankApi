using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NexusBankApi.Models
{
 
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }


        public string AccountNumber { get; set; }

        public string TransactionType { get; set; }


       // [Column(TypeName = "decimal(18,4)")]//will store 18 digits with 4 of it after the decimal
        public double Amount { get; set; }

        public string status { get; set; }

        public DateTime TransactionDate { get; set; }
        
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
    }
}


