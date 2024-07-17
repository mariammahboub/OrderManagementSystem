using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;
using OrderManagement.Repository;
using OrderManagement.Repository.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Repositories
{
    public class UnitOfwork : IUnitOfWork
    {
        private readonly OrderManagementDbContext _dbContext;
        private Hashtable _repositories;
        public UnitOfwork(OrderManagementDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Hashtable();
        }
        public async Task<int> CompleteAsync()
            => await _dbContext.SaveChangesAsync();

        public async ValueTask DisposeAsync()
            => await _dbContext.DisposeAsync();

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var Key = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(Key))
            {
                var repository = new GenericRepository<TEntity>(_dbContext);
                _repositories.Add(Key, repository);
            }
            return _repositories[Key] as IGenericRepository<TEntity>;
        }
    }

}
