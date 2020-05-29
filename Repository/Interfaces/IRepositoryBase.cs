using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NexusBankApi.Repository.Interfaces
{
    //step one for implementing repository pattern
    //this interface is the base interface for the logic
    //it lists all the methods to be implemented in the (repository class) -- the class that contains all database logic using efcore
    //Repositorybase class implements this class
    public interface IRepositoryBase<T>
    {
        IQueryable<T> GetAll(); //returns an Iqueryable of the type T supplied
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
