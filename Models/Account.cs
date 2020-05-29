using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NexusBankApi.Models
{
   
    public class Account
    {


        // public int AccountId { get; }
        [Key]
        public int AccountId { get; set; }
        [Required]
        public string AccountNumber { get; set; }

        [Required]
        public string DateOpened { get; set; }

        public bool isFlagged { get; set; }

        
        public bool getEmailAlert { get; set; }
        
        public bool getSmsAlert { get; set; }
        
        public DateTime LastTransactionDate { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal CurrentBalance { get; set; }


        //foreign key
        
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }//foreign key, CustomerId is the primary key in Customer table
        //navigation property -- won't show as a database field
        public Customer Customer { get; set; }
        

     
    }
}
