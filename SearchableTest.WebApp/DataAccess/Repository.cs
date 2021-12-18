using SearchableTest.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace SearchableTest.WebApp.DataAccess
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        public Repository(IDBFactory dbFactory)
        {
            _dbContext = dbFactory.GetDbContext();
        }
        public bool Create(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            return true;
        }
        public bool Update(T entity)
        {
            if (_dbContext.Entry(entity).State != EntityState.Detached)
            {
                _dbContext.Entry(entity).State = EntityState.Detached;
            }
            _dbContext.Set<T>().Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            return true;
        }
        public bool DeleteById(object id)
        {
            _dbContext.Set<T>().Remove(_dbContext.Set<T>().Find(id));
            return true;
        }
        public bool DeleteObject(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            return true;
        }

        public IQueryable<T> GetAll()
        {
            return _dbContext.Set<T>().AsQueryable();
        }
        public T GetById(object id)
        {
            return _dbContext.Set<T>().Find(id);
        }
        public IQueryable<T> Search(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().Where(predicate).AsQueryable();

        }
    }
    public interface IRepository<T>
    {
        bool Create(T entity);
        bool Update(T entity);
        bool DeleteById(object id);
        bool DeleteObject(T entity);
        T GetById(object id);
        IQueryable<T> GetAll();
        IQueryable<T> Search(Expression<Func<T, bool>> predicate);
    }
}