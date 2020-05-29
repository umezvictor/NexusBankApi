using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexusBankApi.Dto
{
    public class UpdateProfilePicsDto
    {
        public IFormFile ProfilePicture { get; set; }
    }
}
