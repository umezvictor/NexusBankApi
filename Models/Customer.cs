using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NexusBankApi.Models
{
   
    public class Customer
    {
        //this is the main class that interacts with the database
        [Key]
        public int CustomerId { get; set; }
        //public int CustomerId { get; }
        //Add-Migration FirstMigration
       
        public string LastName { get; set; }

        
        public string FirstName { get; set; }

     
       
        public string Email { get; set; }
      
        
        public string ProfilePicture { get; set; }

       
        public string Phone { get; set; }
        
        public string DateOfBirth { get; set; }

        public string AccountNumber { get; set; }

        public double Balance { get; set; }



    }
}
