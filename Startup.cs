using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using NexusBankApi.Models;
using NexusBankApi.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace NexusBankApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //install entityframeworkcore.sqlserver to use the UseSqlServer function
           // services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DBConnection")));

            //identity
            //custom class, AppUser is used in place of IdentityUser, cos it extends IdentityUser
            /*
            services.AddIdentity<AppUser, IdentityRole>(options => {
                //password configuration
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredLength = 5;

            }).AddEntityFrameworkStores<AppDbContext>();

                */



            // configure dependency injection for DbContext of type EmployeeContext
            services.AddDbContext<MyDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DBConnection")));// creates an instance of employeecontext class





            //repository wrapper --- same as db wrapper or aka unitOfWork
           // services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

            //automapper config
            //automapper and automapper.extensions.microsoft.DependencyInjection needs to be installed 
          //  services.AddAutoMapper(typeof(Startup));

            // configuring jwt as the authentication mechanism
            services.AddAuthentication(opt => {
                //authentication scheme
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //challenge scheme
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                         .AddJwtBearer(options =>
                         {
                             options.TokenValidationParameters = new TokenValidationParameters
                             {
                                 ValidateIssuer = true,
                                 ValidateAudience = true,
                                 ValidateLifetime = true,
                                 ValidateIssuerSigningKey = true,

                                 ValidIssuer = Configuration.GetSection("JwtSettings:Issuer").Value,
                                 ValidAudience = Configuration.GetSection("JwtSettings:Audience").Value,
                                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("JwtSettings:JwtSecret").Value))
                                 // = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("JwtSettings:JwtSecret").Value))
                             };
                         });
            services.AddControllers();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //enable authentication
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
