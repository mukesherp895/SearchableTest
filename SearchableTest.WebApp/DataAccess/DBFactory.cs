using SearchableTest.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchableTest.WebApp.DataAccess
{
    public class DBFactory : IDBFactory
    {
        private readonly ApplicationDbContext _dbContext;
        public DBFactory(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public ApplicationDbContext GetDbContext()
        {
            return _dbContext;
        }
    }
    public interface IDBFactory
    {
        ApplicationDbContext GetDbContext();
    }
}