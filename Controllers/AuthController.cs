using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NexusBankApi.Dto;
using NexusBankApi.Models;

namespace NexusBankApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IConfiguration config;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration config)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.config = config;
        }

       
        //POST api/account/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginDto model)
        {
            try
            {
                if (model == null) return BadRequest("Object cannot be null");

                if (ModelState.IsValid)
                {
                    var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                    if (result.Succeeded)
                    {
                        //jwt token generation
                        //token can be generated without any configuration in startup.cs
                        //config in startup.cs is needed for specifying that the token will be used for authentication

                        //step 1 -- set claims --claims are user info that will be hidden in the token
                        //use non sensitive data for claims --eg id and username, email  --don't use password
                        //creating the SymmetricSecretKey with the secret key value superSecretKey@345.

                       // var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));

                        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JwtSettings:JwtSecret").Value));

                        /*
                         * we are creating the object SigningCredentials and as arguments, we provide a secret key
                         * and the name of the algorithm that we are going to use to encode the token.
                         */
                        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                        //we are creating  the JwtSecurityToken object with some important parameters:

                        //user claims
                        var claim = new[]
                        {
                            new Claim(ClaimTypes.Email, model.Email) //email will be hidden inside token
                        };

                        var tokeOptions = new JwtSecurityToken(
                            //Issuer: The first parameter is a simple string representing the name of the web server that issues the token
                            issuer: config.GetSection("JwtSettings:Issuer").Value,

                            // Audience: a string value representing valid recipients
                            audience: config.GetSection("JwtSettings:Audience").Value,
                            //Claims: a list of user roles, for example, the user can be an admin, author
                            claims: claim,
                            //represents the date and time after which the token expires
                            expires: DateTime.Now.AddDays(1),//expires after 1 day - 24hrs
                            signingCredentials: signinCredentials
                        );
                        // we create a string representation of JWT by calling the WriteToken method on JwtSecurityTokenHandler.
                        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

                        //returning JWT in a response
                        return Ok(new { token = tokenString });

                       

                    }//result.succeed

                    return BadRequest("Invalid email or password");

                }
                return BadRequest("Invalid object");
            }//end of try
            catch (Exception)
            {
                return this.StatusCode(500);
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // create new user
        //[HttpPost]
        //POST api/account/signup
        //saves user record to identity users table --AspNetUsers
        [HttpPost("signup")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto model)
        {

            try
            {
                //check if user already exists
                var existingUser = await userManager.FindByEmailAsync(model.Email);

                if (existingUser != null) return BadRequest("User already exists");

                //check if model is null
                if (model == null) return BadRequest("User object cannot be null");

                if (!ModelState.IsValid) return BadRequest("Invalid user object");

                //create user object
                var user = new AppUser
                {
                    Email = model.Email,
                    UserName = model.Email,//email serves as username as well
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                };

                //save user
                var result = await userManager.CreateAsync(user, model.Password);

                //check if successful

                if (result.Succeeded)
                {
                    return Ok("User has been created successfully");

                }
                else
                {
                    return BadRequest("User was not successfully created");
                }
            }
            catch (Exception)
            {
                return this.StatusCode(500);
            }
           
        }




        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
