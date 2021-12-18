using SearchableTest.WebApp.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace SearchableTest.WebApp.Services
{
    public class Service<T> : IService<T>
    {
        private readonly IRepository<T> _repository;
        public Service(IRepository<T> repository)
        {
            _repository = repository;
        }

        public bool Create(T entity)
        {
            return _repository.Create(entity);
        }

        public bool DeleteById(object id)
        {
            return _repository.DeleteById(id);
        }
        public bool DeleteObject(T entity)
        {
            return _repository.DeleteObject(entity);
        }

        public IQueryable<T> GetAll()
        {
            return _repository.GetAll();
        }

        public T GetById(object id)
        {
            return _repository.GetById(id);
        }

        public IQueryable<T> Search(Expression<Func<T, bool>> predicate)
        {
            return _repository.Search(predicate);
        }

        public bool Update(T entity)
        {
            return _repository.Update(entity);
        }
    }
    public interface IService<T>
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