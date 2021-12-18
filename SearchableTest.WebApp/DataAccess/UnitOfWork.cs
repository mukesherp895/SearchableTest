using SearchableTest.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchableTest.WebApp.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool BeginTransaction()
        {
            _dbContext.Database.BeginTransaction();
            return true;
        }

        public bool Commit()
        {
            _dbContext.SaveChanges();
            return true;
        }

        public bool CommitTransaction()
        {
            if (Commit())
            {
                _dbContext.Database.CurrentTransaction.Commit();
                return true;
            }
            return false;
        }

        public bool Rollback()
        {
            _dbContext.Database.BeginTransaction().Rollback();
            return true;
        }
    }
    public interface IUnitOfWork
    {
        bool BeginTransaction();
        bool Commit();
        bool Rollback();
        bool CommitTransaction();
    }
}