using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NexusBankApi.Dto
{
    public class CustomerDto
    {
        // public int CustomerId { get; set; } id is commented since it won't be sent as part of the input fields

        //this class is used for updating customer

        
        public string LastName { get; set; }

       
        public string FirstName { get; set; }



       
        public string Email { get; set; }

        

        public string Phone { get; set; }

        //the property name "ProfilePicture" must be the same in the Customer model class
        public IFormFile ProfilePicture { get; set; }

        public string DateOfBirth { get; set; }

        public string AccountNumber { get; set; }

        public double Balance { get; set; }


    }
}
