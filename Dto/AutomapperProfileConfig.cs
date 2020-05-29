using AutoMapper;
using NexusBankApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexusBankApi.Dto
{
    public class AutomapperProfileConfig : Profile//inherits from Profile class of automapper
    {
        //next, add this to startup.cs --- services.AddAutoMapper(typeof(Startup));
        public AutomapperProfileConfig()
        {
            //this.CreateMap<CustomerDto, Customer>();
            CreateMap<CustomerDto, Customer>()
                .ReverseMap();
            //maps source object to destination
            //this will be used in the controller api methods
            //eg, mapping input from frontend ---eg via postman or react frontend to the user object in database (eg object retrieved from db)

            CreateMap<CustomerCreationDto, Customer>()
                .ReverseMap();

            CreateMap<TransactionDto, Transaction>()
                .ReverseMap();
        }
    }
}
