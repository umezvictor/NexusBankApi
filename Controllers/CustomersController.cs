using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using NexusBankApi.Dto; 
using NexusBankApi.Models;
using NexusBankApi.Repository;

namespace NexusBankApi.Controllers
{
     //this controller is protected-  jwt token is needed to acces it
   // [Authorize]
    [ApiController]
   // [Route("api/customer")]
    [Route("api/[controller]")]

    public class CustomersController : ControllerBase
    {

        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
       
        public CustomersController(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }


        //get all customers
        
        [HttpGet]
        
        public async Task<IActionResult> GetAllCustomers()
        {
            try
            {
                var customers = await _repoWrapper.Customer.GetAllCustomersAsync();

                if(customers == null)
                {
                    return NotFound("No record found");
                }
                else
                {
                    return Ok(customers);
                }
            }
            catch (Exception)
            {
                return this.StatusCode(501);
            }
        }



        //get customer by phone
        [HttpGet("{phone}")]
        public async Task<IActionResult> GetCustomerByPhoneAsync(string  Phone)
        {
            try
            {
                var customer = await _repoWrapper.Customer.GetCustomerByPhoneAsync(Phone);

                if (customer == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(customer);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }


        //get customer by id
        [HttpGet("{id}")] 
        public async Task<IActionResult> GetCustomerById(int Id)
        {
            try
            {
                var customer = await _repoWrapper.Customer.GetCustomerByIdAsync(Id);
                 
                if(customer == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(customer);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }


       




       //create new customer
       //[AllowAnonymous]
       [HttpPost]
        //[FromForm] is compulsory for getting files with json data
        public async Task<IActionResult> CreateCustomer([FromForm]CustomerCreationDto customer)
        {
            try
            {
                //check if profile picture is present
                var profilePicture = customer.ProfilePicture;
                if (profilePicture.Length <= 0) return BadRequest(new { errorMessage = "Profile picture must be uploaded" });

               

                //check if customer already exists

                var existingCustomer = await _repoWrapper.Customer.GetCustomerByEmailAsync(customer.Email);

                if(existingCustomer != null)
                {
                    return BadRequest(new { errorMessage = "Customer already exists" });
                }

                //check if customer object is null
                if(customer == null)
                {
                    return BadRequest(new {errorMessage = "Customer object can not be null" });
                }
               
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { errorMessage = "Invalid customer model" });  
                }

                

                //give the file a unique id
                Guid uniqueFileId = Guid.NewGuid();

                //give file a unique name to prevent overwriting
                var uniqueFileName = uniqueFileId + profilePicture.FileName.Replace(" ", "_");//replace spaces with underscore

                //specify folder to save uploaded files
                var filePath = Path.Combine("Uploads/Images", uniqueFileName);

                //this works, but saves file inside server, not a specific folder
                // using(var fileStream = new FileStream(profilePicture.FileName, FileMode.Create))

                //save profile picture
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await profilePicture.CopyToAsync(fileStream);
                }  
                
                //map incoming customer object of CustomerCreationDto to the Customer model class
                var customerEntity = _mapper.Map<Customer>(customer);


                //add the unique filename to customerentity object and save to db
                customerEntity.ProfilePicture = uniqueFileName;

                _repoWrapper.Customer.CreateCustomer(customerEntity);
                await _repoWrapper.SaveAsync();

                //map the created customer to the CustomerCreationDto
                // var createdCustomer = _mapper.Map<CustomerCreationDto>(customerEntity);  --causes error

                //return created customer
                //return Created($"/api/customers/{createdCustomer.CustomerId}", createdCustomer); 

                return Created($"/api/customers/{customerEntity.CustomerId}", customerEntity);
            }
            catch (Exception)
            {
                //if an exception occurs
                return this.StatusCode(501);
            }
            
        }





        //update customer
        [HttpPut("{id}")]
       
        public async Task<IActionResult> UpdateCustomer(int Id, [FromBody]CustomerDto customer)
        {
            try
                
            {
                
                if (customer == null)
                {
                    return BadRequest(new { errorMessage = "Customer object is null" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new { errorMessage = "Invalid customer object" });
                }
                
                //get the customer's record
                var existingCustomerRecord = await _repoWrapper.Customer.GetCustomerByIdAsync(Id);

                if(existingCustomerRecord == null)
                {
                    return NotFound(); 
                }

               
                //map the incoming data to the data fetched from the db
                //maps the customerdto object to the user record objects retrieved from db
                _mapper.Map(customer, existingCustomerRecord);

               
                 _repoWrapper.Customer.UpdateCustomer(existingCustomerRecord);

                
                await _repoWrapper.SaveAsync();

                return NoContent();//success

            }
            catch (Exception)
            {
                return this.StatusCode(500);
            }
    
           
        }

        [HttpPut]
        //update profile picture via a separate api
        public async Task<IActionResult> UpdateProfilePicture(int Id, [FromForm]UpdateProfilePicsDto profilePicsDto)
        {
            var profilePicture = profilePicsDto.ProfilePicture;

            if (profilePicture.Length <= 0) return BadRequest("Profile picture is required");

            var existingCustomer = _repoWrapper.Customer.GetCustomerByIdAsync(Id);

            if (existingCustomer == null) return BadRequest("Customer does not exist");

          
                //give the file a unique id
                Guid uniqueFileId = Guid.NewGuid();

                //give file a unique name to prevent overwriting
                var uniqueFileName = uniqueFileId + profilePicture.FileName.Replace(" ", "_");
                //replace spaces with underscore

                //specify folder to save uploaded files
                var filePath = Path.Combine("Uploads/Images", uniqueFileName);


                //this works, but saves file inside server, not a specific folder
                // using(var fileStream = new FileStream(profilePicture.FileName, FileMode.Create))

                //save profile picture
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await profilePicture.CopyToAsync(fileStream);
                }

           //next--- update profile picture name in database


            return Ok(new { successMessage = "Profile picture was updated succcesfully"}); 

            
        }


        //delete customer
        [HttpDelete("{id}")]
       
        public async Task<IActionResult> DeleteCustomer(int Id)
        {
            try
            {
                //find the user by id
                var customer = await _repoWrapper.Customer.GetCustomerByIdAsync(Id);

                if(customer == null)
                {
                    return NotFound(new { errorMessage = "Customer does not exist" });
                }

                _repoWrapper.Customer.DeleteCustomer(customer);

                await _repoWrapper.SaveAsync();

                return Ok( new { successMessage = "Customer has been successfully deleted" });
            }
            catch (Exception)
            {
                return this.StatusCode(500);
            }
        }
    }
}
