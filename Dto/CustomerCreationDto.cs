using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NexusBankApi.Dto
{
    public class CustomerCreationDto
    {
        public int CustomerId { get; set; }

        //this class is used for creating a new customer 
        //represenrts the data to be sent from the api
        //here we specify certain subsets of the main Customer class in models folder that we mighht want to modify

        //add validation here as well -- will be shown to api users
        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }
        
     

        [Required]
        public string Email { get; set; }
        
        [Required]

        public string Phone { get; set; }
      

        [Required]
        //the property name "ProfilePicture" must be the same in the Customer model class
        public IFormFile ProfilePicture { get; set; }

        public string DateOfBirth { get; set; }

        public string AccountNumber { get; set; }

        public double Balance { get; set; }

    }
}

