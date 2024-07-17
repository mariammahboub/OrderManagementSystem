using Microsoft.EntityFrameworkCore;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;
using OrderManagement.Repositories;
using OrderManagement.Repository.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderManagement.Services.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(OrderManagementDbContext context) : base(context)
        { }

        public async Task<User> GetByUsernameAsync(string username)
        {
            var users = await _dbSet
                .Where(u => u.Username == username)
                .ToListAsync();

            if (users.Count > 1)
            {
                throw new InvalidOperationException("Multiple users found with the same username.");
            }

            return users.SingleOrDefault();
        }
        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _dbSet.AnyAsync(u => u.Username == username);
        }



    }
}
