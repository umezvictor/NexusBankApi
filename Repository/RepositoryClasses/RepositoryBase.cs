using Microsoft.EntityFrameworkCore;
using NexusBankApi.Models;
using NexusBankApi.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace NexusBankApi.Repository.RepositoryClasses
{
    /*
     This abstract class, as well as IRepositoryBase interface, uses generic type T to work with. 
     This type T gives even more reusability to the RepositoryBase class. That means we don’t have to specify the 
     exact model (class) right now for the RepositoryBase to work with, we are going to do that later on
    */
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected AppDbContext _dbContext { get; set; }

        //dependency injection
        public RepositoryBase(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        //these methods are generic, each class will implement it its own way
        public  void Create(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return _dbContext.Set<T>().Where(expression).AsNoTracking();
        }

        public IQueryable<T> GetAll()
        {
            return _dbContext.Set<T>().AsNoTracking();
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }
    }
}

