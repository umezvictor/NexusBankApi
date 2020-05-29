using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexusBankApi.Models
{
    public class AppUser : IdentityUser
    {
        //this class extends the IdentitYUser class 
        //it adds two custom fields to the Identity Users Table

        //feel free to add more custom user fields here
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
